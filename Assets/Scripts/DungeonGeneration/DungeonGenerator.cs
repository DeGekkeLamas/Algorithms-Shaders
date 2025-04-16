using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Generation properties")]
    [Tooltip("Leave as 0 to use a random seed")]
    public int seed;
    public bool shouldRemoveSmallestRooms;
    public RectInt initialRoom = new(0, 0, 100, 100);
    RectInt originRoom;
    public float height = 5;

    float fraction = 0.5f;
    public Vector2Int splitFractionRange = new(35, 66);
    public float roomMaxSize = 45;
    public int maxDoorsForOriginRoom = 1;
    public int maxDoorsPerRoom = 3;

    [Header("Display properties")]
    public bool displayVisualDebugging = true;
    public float generationInterval = .1f;
    public int splitOffset = 2;
    public int doorWidth = 1;
    public bool showDeletedDoors = true;

    [Header("Asset generation properties")]
    public bool disableVisualDebuggingAfterAssetGeneration = true;
    public GameObject wall;
    public GameObject wallBound;
    public GameObject floor;
    public GameObject player;
    public GameObject enemy;
    public EnemyProbabilities[] enemiesPerRoom;
    public float wallHeight = 5;
    public float wallBoundHeight = 1;
    public float wallBoundThickness = 1.25f;
    [System.Serializable]
    public struct ItemLootLable
    {
        public string itemName;
        public int probability;
    }
    [System.Serializable]
    public struct RoomSpecificAssets
    {
        public PickupItem itemPickup;
        [Header("Bakery")]
        public Material bakeryWall;
        public Material bakeryFloor;
        [Space]
        public int bakeryTotalItemChance;
        public ItemLootLable[] bakeryItemSpawns;
        [Header("Break room")]
        public Material breakRoomWall;
        public Material breakRoomFloor;
        [Space]
        public int breakTotalItemChance;
        public ItemLootLable[] breakItemSpawns;
        [Header("Kitchen")]
        public Material kitchenWall;
        public Material kitchenFloor;
        public int counterLength;
        public GameObject counter;
        public GameObject counterCornerL;
        public GameObject counterCornerR;
        [Space]
        public int kitchenTotalItemChance;
        public ItemLootLable[] kitchenItemSpawns;
        [Header("Storage")]
        public Material storageWall;
        public Material storageFloor;
        [Space]
        public int storageTotalItemChance;
        public ItemLootLable[] storageItemSpawns;
        [Header("Seating")]
        public Material seatingWall;
        public Material seatingFloor;
        [Space]
        public int seatingTotalItemChance;
        public ItemLootLable[] seatingItemSpawns;
    }
    public RoomSpecificAssets roomSpecificAssets;

    [Header("Generated stuff")]
    public NavMeshSurface navMeshSurface;
    GameObject assetContainer;
    public List<RectInt> rooms = new(1);
    public List<RectInt> doors = new(1);
    public List<RectInt> removedDoors = new(1);
    public enum RoomType {bakery, breakRoom, kitchen, seating, storage };
    public RoomType[] roomTypes;
    List<GameObject> wallsGenerated = new();
    Graph<Vector2> dungeonGraph = new();
    System.Random _random = new System.Random();

    private void OnValidate()
    {
        splitFractionRange = new(
            splitFractionRange.x,
            Mathf.Max(splitFractionRange.x, splitFractionRange.y)
            );
        roomSpecificAssets.bakeryTotalItemChance = GetTotalItemProbability(roomSpecificAssets.bakeryItemSpawns);
        roomSpecificAssets.breakTotalItemChance = GetTotalItemProbability(roomSpecificAssets.breakItemSpawns);
        roomSpecificAssets.kitchenTotalItemChance = GetTotalItemProbability(roomSpecificAssets.kitchenItemSpawns);
        roomSpecificAssets.storageTotalItemChance = GetTotalItemProbability(roomSpecificAssets.storageItemSpawns);
        roomSpecificAssets.seatingTotalItemChance = GetTotalItemProbability(roomSpecificAssets.seatingItemSpawns);
    }
    private void Awake()
    {
        if (seed != 0) _random = new System.Random(seed);

        StartCoroutine(GenerateDungeon());
    }
    bool coroutineIsDone;
    IEnumerator GenerateDungeon()
    {
        assetContainer = new GameObject("AssetContainer");

        coroutineIsDone = false;
        StartCoroutine(GenerateRooms());
        yield return new WaitUntil(() => coroutineIsDone);
        coroutineIsDone = false;
        StartCoroutine(GenerateDoors());
        yield return new WaitUntil(() => coroutineIsDone);
        coroutineIsDone = false;
        StartCoroutine(RemoveRooms());
        yield return new WaitUntil(() => coroutineIsDone);
        coroutineIsDone = false;
        StartCoroutine(AssignRoomTypes());
        yield return new WaitUntil(() => coroutineIsDone);
        coroutineIsDone = false;
        StartCoroutine(GenerateInitialWalls());
        yield return new WaitUntil(() => coroutineIsDone);
        coroutineIsDone = false;
        StartCoroutine(ModifyWalls());
        yield return new WaitUntil(() => coroutineIsDone);
        coroutineIsDone = false;
        StartCoroutine(GenerateFloor());
        yield return new WaitUntil(() => coroutineIsDone);
        coroutineIsDone = false;
        StartCoroutine(Brickify());
        yield return new WaitUntil(() => coroutineIsDone);
        coroutineIsDone = false;
        StartCoroutine(SpawnObjects());
        Debug.Log("Generated all room assets!");
    }
    IEnumerator GenerateRooms()
    {
        rooms.Add(initialRoom);

        yield return new WaitForSeconds(0.1f);

        // Room generation
        while (rooms[GetBiggestRoom(out int _biggestRoom)].size.magnitude > roomMaxSize)
        {
            RectInt biggestRoom = rooms[_biggestRoom];
            RandomizeFraction();
            if (biggestRoom.width > biggestRoom.height)
            {
                SplitVertical(biggestRoom, fraction, out RectInt newRoomA, out RectInt newRoomB);
                rooms.Add(newRoomA);
                rooms[_biggestRoom] = newRoomB;
            }
            else
            {
                SplitHorizontal(biggestRoom, fraction, out RectInt newRoomA, out RectInt newRoomB);
                rooms.Add(newRoomA);
                rooms[_biggestRoom] = newRoomB;
            }
            yield return new WaitForSeconds(generationInterval);
        }
        foreach (var room in rooms) { dungeonGraph.AddNode(room.center); }
        originRoom = rooms[GetNearestToOrigin()];

        Debug.Log($"Generated all rooms, from {this}");
        coroutineIsDone = true;
    }
    IEnumerator GenerateDoors()
    {
        // Door generation
        int _tolerance = 3; // variable for determining if a door is too close to the edge of its room

        for (int i = 0; i < rooms.Count; i++)
        {
            for (int j = i+1; j < rooms.Count; j++)
            {
                int roomsConnectedI = dungeonGraph.adjacencyList[rooms[i].center].Count;
                int roomsConnectedJ = dungeonGraph.adjacencyList[rooms[j].center].Count;
                if (AlgorithmsUtils.Intersects(rooms[i], rooms[j]) &&
                    (rooms[i] != originRoom && roomsConnectedI < maxDoorsPerRoom ||
                    rooms[i] == originRoom && roomsConnectedI < maxDoorsForOriginRoom) &&
                    (rooms[j] != originRoom && roomsConnectedJ < maxDoorsPerRoom ||
                    rooms[j] == originRoom && roomsConnectedJ < maxDoorsForOriginRoom))
                {
                    var _newDoor = AlgorithmsUtils.Intersect(rooms[i], rooms[j]);

                    _newDoor = new(
                        (_newDoor.width <= 0) ? _newDoor.xMin - doorWidth / 2 : (_newDoor.xMin) + _newDoor.width / 2,
                        (_newDoor.height <= 0) ? _newDoor.yMin - doorWidth / 2 : (_newDoor.yMin) + _newDoor.height / 2,
                        (_newDoor.width <= 0) ? _newDoor.width + doorWidth : doorWidth,
                        (_newDoor.height <= 0) ? _newDoor.height + doorWidth : doorWidth);

                    if (Mathf.Abs(rooms[i].center.x - _newDoor.center.x) > rooms[i].width / _tolerance &&
                        Mathf.Abs(rooms[i].center.y - _newDoor.center.y) > rooms[i].height / _tolerance ||
                        Mathf.Abs(rooms[j].center.x - _newDoor.center.x) > rooms[j].width / _tolerance &&
                        Mathf.Abs(rooms[j].center.y - _newDoor.center.y) > rooms[j].height / _tolerance)
                    {
                        Debug.Log($"Removed corner door, from {this}");
                        removedDoors.Add(_newDoor);
                        yield return new WaitForSeconds(generationInterval);
                        continue;
                    }

                    doors.Add(_newDoor);

                    dungeonGraph.AddNode(_newDoor.center);
                    dungeonGraph.AddEdge(_newDoor.center, rooms[i].center);
                    dungeonGraph.AddEdge(_newDoor.center, rooms[j].center);

                    yield return new WaitForSeconds(generationInterval);
                }
            }
        }
        Debug.Log($"Generated all doors, from {this}");
        coroutineIsDone = true;
    }
    IEnumerator RemoveRooms()
    {
        // Removes rooms with 0 doors
        int _doorsRemoved = 0;
        for (int i = 0; i < rooms.Count; i++)
        {
            if (dungeonGraph.adjacencyList[rooms[i].center].Count == 0)
            {
                dungeonGraph.adjacencyList.Remove(rooms[i].center);
                rooms.RemoveAt(i);
                _doorsRemoved++;
                i--;
                yield return new WaitForSeconds(0.1f);
            }
        }

        // removes rooms with doors but no way to reach origin
        List<Vector2> _accessibleRooms = dungeonGraph.BFS(originRoom.center);

        List<Vector2> _roomsToRemove = new();
        foreach (KeyValuePair<Vector2, List<Vector2>> point in dungeonGraph.adjacencyList)
        {
            if (!_accessibleRooms.Contains(point.Key)) _roomsToRemove.Add(point.Key);
        }
        foreach (Vector2 room in _roomsToRemove)
        {
            foreach (Vector2 door in dungeonGraph.adjacencyList[room])
            {
                doors.Remove(GetDoorByCenter(door));
            }

            rooms.Remove(GetRoomByCenter(room));
            dungeonGraph.adjacencyList.Remove(room);
            yield return new WaitForSeconds(generationInterval);
        }
        Debug.Log($"Removed {_doorsRemoved} inaccessible rooms, from {this}");

        // Reset seed to random after generation
        _random = new System.Random((int)DateTime.Now.Ticks);

        // Removes smallest rooms
        if (shouldRemoveSmallestRooms)
        {
            // Sorts rooms from biggest to smallest
            rooms = SortListFromSmallToBig(rooms);


        }

        Debug.Log("Room generation done!");
        if (disableVisualDebuggingAfterAssetGeneration) displayVisualDebugging = false;
        coroutineIsDone = true;
    }
    IEnumerator AssignRoomTypes()
    {
        roomTypes = new RoomType[rooms.Count];
        int bakeries = 0;
        int kitchens = 0;
        int seatings = 0;
        int storages = 0;
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i] == originRoom)
            {
                roomTypes[i] = RoomType.breakRoom;
                continue;
            }
            int lowestQTY = Mathf.Min(bakeries,kitchens,seatings,storages);
            if (lowestQTY == bakeries)
            {
                roomTypes[i] = RoomType.bakery;
                bakeries++;
            }
            else if (lowestQTY == kitchens)
            {
                roomTypes[i] = RoomType.kitchen;
                kitchens++;
            }
            else if (lowestQTY == seatings)
            {
                roomTypes[i] = RoomType.seating;
                seatings++;
            }
            else if (lowestQTY == storages)
            {
                roomTypes[i] = RoomType.storage;
                storages++;
            }
        }
        yield return new();
        coroutineIsDone = true;
    }
    IEnumerator GenerateInitialWalls()
    {
        // Generate walls
        GameObject wallContainer = new("WallContainer");
        wallContainer.transform.parent = assetContainer.transform;

        for (int i = 0; i < rooms.Count; i++)
        {
            GameObject roomWallContainer = new($"Room{i}WallContainer");
            roomWallContainer.transform.parent = wallContainer.transform;

            RectInt room = rooms[i];
            Vector3 _center = new(room.center.x, 0, room.center.y);

            GameObject wallXPlus = Instantiate(wall, _center + new Vector3(room.width * 0.5f, 0, 0),
                Quaternion.identity, roomWallContainer.transform);
            GameObject wallXMin = Instantiate(wall, _center + new Vector3(-room.width * 0.5f, 0, 0),
                Quaternion.identity, roomWallContainer.transform);
            GameObject wallYPlus = Instantiate(wall, _center + new Vector3(0, 0, room.height * 0.5f),
                Quaternion.identity, roomWallContainer.transform);
            GameObject wallYMin = Instantiate(wall, _center + new Vector3(0, 0, -room.height * 0.5f),
                Quaternion.identity, roomWallContainer.transform);

            wallXPlus.name = "wallXPlus";
            wallXMin.name = "wallXMin";
            wallYPlus.name = "wallYPlus";
            wallYMin.name = "wallYMin";

            wallXPlus.transform.localScale = new(1, wallHeight, room.height);
            wallXMin.transform.localScale = new(1, wallHeight, room.height);
            wallYPlus.transform.localScale = new(room.width, wallHeight, 1);
            wallYMin.transform.localScale = new(room.width, wallHeight, 1);

            WallGenerator wallXPlusGen = wallXPlus.AddComponent<WallGenerator>(); 
            wallXPlusGen.zMin = true;
            WallGenerator wallXMinGen = wallXMin.AddComponent<WallGenerator>();
            wallXMinGen.zPlus = true;
            if (room.xMin == 0) wallXMinGen.zMin = true;
            WallGenerator wallYPlusGen = wallYPlus.AddComponent<WallGenerator>();
            wallYPlusGen.xPlus = true;
            WallGenerator wallYMinGen = wallYMin.AddComponent<WallGenerator>();
            wallYMinGen.xMin = true;
            if (room.yMin == 0) wallYMinGen.xPlus = true;

            wallsGenerated.Add(wallXPlus);
            wallsGenerated.Add(wallYPlus);
            wallsGenerated.Add(wallXMin);
            wallsGenerated.Add(wallYMin);

            Material materialToAssign;
            if (room == originRoom)
                materialToAssign = roomSpecificAssets.breakRoomWall;
            else switch(roomTypes[i])
                {
                    case RoomType.bakery:
                        materialToAssign = roomSpecificAssets.bakeryWall;
                        break;
                    case RoomType.breakRoom:
                        materialToAssign = roomSpecificAssets.breakRoomWall;
                        break;
                    case RoomType.kitchen:
                        materialToAssign = roomSpecificAssets.kitchenWall;
                        break;
                    case RoomType.seating:
                        materialToAssign = roomSpecificAssets.seatingWall;
                        break;
                    case RoomType.storage:
                        materialToAssign = roomSpecificAssets.storageWall;
                        break;
                    default:
                        materialToAssign = roomSpecificAssets.kitchenWall;
                        break;
                }

            wallXPlus.GetComponent<MeshRenderer>().sharedMaterial = materialToAssign;
            wallYPlus.GetComponent<MeshRenderer>().sharedMaterial = materialToAssign;
            wallXMin.GetComponent<MeshRenderer>().sharedMaterial = materialToAssign;
            wallYMin.GetComponent<MeshRenderer>().sharedMaterial = materialToAssign;

            yield return new WaitForSeconds(generationInterval);
        }
        coroutineIsDone = true;
        yield return new();
    }
    IEnumerator ModifyWalls()
    {
        // Check for door intersections
        int wallsQTY = wallsGenerated.Count;
        GameObject doorBoundsContainer = new("DoorBounds");
        doorBoundsContainer.transform.parent = assetContainer.transform;
        for (int i = 0; i < wallsQTY; i++)
        {
            RectInt wallRect = new(
                (int)(wallsGenerated[i].transform.position.x - .5f * wallsGenerated[i].transform.localScale.x),
                (int)(wallsGenerated[i].transform.position.z - .5f * wallsGenerated[i].transform.localScale.z),
                (int)wallsGenerated[i].transform.localScale.x,
                (int)wallsGenerated[i].transform.localScale.z
                );
            for (int j = 0; j < doors.Count; j++)
            {
                if (AlgorithmsUtils.Intersects(doors[j], wallRect))
                {
                    GameObject wallDupe = Instantiate(wallsGenerated[i], wallsGenerated[i].transform.position,
                        Quaternion.identity, wallsGenerated[i].transform.parent);
                    wallDupe.name = "WallB";
                    GameObject doorBoundA;
                    GameObject doorBoundB;

                    RectInt newWallA;
                    RectInt newWallB;
                    RectInt intersectDoor = doors[j];

                    bool intersectOverHeight = wallRect.width < wallRect.height;
                    if (intersectOverHeight)
                    {
                        newWallA = new(
                            wallRect.xMin,
                            wallRect.yMin,
                            wallRect.width,
                            intersectDoor.yMin - wallRect.yMin
                            );
                        newWallB = new(
                            wallRect.xMin,
                            intersectDoor.yMin + intersectDoor.height,
                            wallRect.width,
                            wallRect.height - newWallA.height - intersectDoor.height
                            );
                    }
                    else
                    {
                        newWallA = new(
                            wallRect.xMin,
                            wallRect.yMin,
                            intersectDoor.xMin - wallRect.xMin,
                            wallRect.height
                            );
                        newWallB = new(
                            intersectDoor.xMin + intersectDoor.width,
                            wallRect.yMin,
                            wallRect.width - newWallA.width - intersectDoor.width,
                            wallRect.height
                            );
                    }
                    yield return new WaitForSeconds(generationInterval);

                    wallsGenerated[i].transform.position = new Vector3(newWallA.center.x, 0, newWallA.center.y);
                    wallDupe.transform.position = new Vector3(newWallB.center.x, 0, newWallB.center.y);
                    wallsGenerated[i].transform.localScale = new Vector3(newWallA.width, wallHeight, newWallA.height);
                    wallDupe.transform.localScale = new Vector3(newWallB.width, wallHeight, newWallB.height);
                    wallsGenerated.Add(wallDupe);

                    if (intersectOverHeight)
                    {
                        doorBoundA = Instantiate(wallBound, new Vector3(
                            wallDupe.transform.position.x, wallDupe.transform.position.y, intersectDoor.yMin
                            ), Quaternion.identity, doorBoundsContainer.transform);
                        doorBoundB = Instantiate(wallBound, new Vector3(
                            wallDupe.transform.position.x, wallDupe.transform.position.y, intersectDoor.yMax
                            ), Quaternion.identity, doorBoundsContainer.transform);
                    }
                    else
                    {
                        doorBoundA = Instantiate(wallBound, new Vector3(
                            intersectDoor.xMin, wallDupe.transform.position.y, wallDupe.transform.position.z
                            ), Quaternion.identity, doorBoundsContainer.transform);
                        doorBoundB = Instantiate(wallBound, new Vector3(
                            intersectDoor.xMax, wallDupe.transform.position.y, wallDupe.transform.position.z
                            ), Quaternion.identity, doorBoundsContainer.transform);
                    }
                    doorBoundA.transform.localScale = new Vector3(wallBoundThickness, 
                        wallHeight + wallBoundHeight, wallBoundThickness);
                    doorBoundB.transform.localScale = new Vector3(wallBoundThickness, 
                        wallHeight + wallBoundHeight, wallBoundThickness);
                }
            }
        }

        // Generate bounds on top of and below walls
        GameObject wallBoundContainer = new("WallBoundContainer");
        wallBoundContainer.transform.parent = assetContainer.transform;
        foreach (GameObject wall in wallsGenerated)
        {
            GameObject wallboundTop = Instantiate(wallBound,
                new Vector3(wall.transform.position.x, 
        wall.transform.position.y + 0.5f * wall.transform.localScale.y, wall.transform.position.z), 
                Quaternion.identity, wallBoundContainer.transform);

            Vector3 wallScale = wall.transform.localScale;
            wallboundTop.transform.localScale = new Vector3(
                wallScale.x + (wallScale.x < wallScale.z ? wallBoundThickness - 1 : 0), 
                wallBoundHeight,
                wallScale.z + (wallScale.x > wallScale.z ? wallBoundThickness - 1 : 0));

            GameObject wallboundBottom = Instantiate(wallBound,
                new Vector3(wall.transform.position.x, 
        wall.transform.position.y - 0.5f * wall.transform.localScale.y, wall.transform.position.z), 
                Quaternion.identity, wallBoundContainer.transform);

            wallboundBottom.transform.localScale = wallboundTop.transform.localScale;
            yield return new WaitForSeconds(generationInterval);
        }
        // Generate bounds in map corners
        GameObject cornerOrigin = Instantiate(wallBound, new(initialRoom.xMin, 0, initialRoom.yMin),Quaternion.identity, wallBoundContainer.transform);
        GameObject cornerOriginPlusX = Instantiate(wallBound, new(initialRoom.xMax, 0, initialRoom.yMin),Quaternion.identity, wallBoundContainer.transform);
        GameObject cornerOriginPlusY = Instantiate(wallBound, new(initialRoom.xMin, 0, initialRoom.yMax),Quaternion.identity, wallBoundContainer.transform);
        GameObject cornerOriginPlusXY = Instantiate(wallBound, new(initialRoom.xMax, 0, initialRoom.yMax),Quaternion.identity, wallBoundContainer.transform);
        cornerOrigin.transform.localScale = new(wallBoundThickness, wallHeight + wallBoundHeight, wallBoundThickness);
        cornerOriginPlusX.transform.localScale = new(wallBoundThickness, wallHeight + wallBoundHeight, wallBoundThickness);
        cornerOriginPlusY.transform.localScale = new(wallBoundThickness, wallHeight + wallBoundHeight, wallBoundThickness);
        cornerOriginPlusXY.transform.localScale = new(wallBoundThickness, wallHeight + wallBoundHeight, wallBoundThickness);

        coroutineIsDone = true;
    }
    IEnumerator GenerateFloor()
    {
        // Generate floor
        GameObject floorContainer = new GameObject("FloorContainer");
        floorContainer.transform.parent = assetContainer.transform;
        for(int i = 0; i < rooms.Count; i++)
        {
            RectInt room = rooms[i];
            GameObject _floor = Instantiate(floor, new Vector3(room.center.x, -wallHeight * .5f, room.center.y),
                Quaternion.identity, floorContainer.transform);
            _floor.transform.localScale = new Vector3(room.width, 1, room.height) / 10;
            yield return new WaitForSeconds(generationInterval);

            Material materialToAssign;
            if (room == originRoom)
                materialToAssign = roomSpecificAssets.breakRoomFloor;
            else switch (roomTypes[i])
                {
                    case RoomType.bakery:
                        materialToAssign = roomSpecificAssets.bakeryFloor;
                        break;
                    case RoomType.breakRoom:
                        materialToAssign = roomSpecificAssets.breakRoomFloor;
                        break;
                    case RoomType.kitchen:
                        materialToAssign = roomSpecificAssets.kitchenFloor;
                        break;
                    case RoomType.seating:
                        materialToAssign = roomSpecificAssets.seatingFloor;
                        break;
                    case RoomType.storage:
                        materialToAssign = roomSpecificAssets.storageFloor;
                        break;
                    default:
                        materialToAssign = roomSpecificAssets.kitchenFloor;
                        break;
                }
            _floor.GetComponent<MeshRenderer>().sharedMaterial = materialToAssign;
        }

        coroutineIsDone = true;
        yield return new();

    }
    IEnumerator Brickify()
    {
        // Brickifies generated walls
        WallGenerator[] walls = GameObject.FindObjectsByType<WallGenerator>(FindObjectsSortMode.None);
        foreach (WallGenerator wall in walls)
        {
            wall.GenerateWalls();
        }

        coroutineIsDone = true;
        yield return new();
    }
    IEnumerator SpawnObjects()
    {

        // Place objects in rooms
        GameObject roomAssetContainer = new("RoomAssetContainer");
        roomAssetContainer.transform.parent = assetContainer.transform;
        GameObject itemSpawnsContainer = new("ItemSpawnsContainer");
        itemSpawnsContainer.transform.parent = roomAssetContainer.transform;

        GameObject counterContainer = new("CounterContainer");
        counterContainer.transform.parent = roomAssetContainer.transform;
        for (int i = 0; i < rooms.Count; i++)
        {
            // Place assets based on room type
            switch (roomTypes[i])
            {
                case RoomType.bakery:
                    break;
                case RoomType.kitchen:
                    RectInt room = rooms[i];
                    // Kitchen island
                    Vector2 offset = new(-.5f * roomSpecificAssets.counterLength, -0.5f);
                    bool roomvertical = room.height > room.width;
                    float counterSize = roomSpecificAssets.counter.transform.lossyScale.x;

                    if (roomvertical) offset = new(offset.y, offset.x);
                    for (int j = 0; j < 2; j++)
                    {   for (int k = 0; k < roomSpecificAssets.counterLength; k++)
                        {
                            Transform counter = Instantiate(roomSpecificAssets.counter, new( 
                                room.center.x + offset.x,
                                -wallHeight * .5f + counterSize * .5f,
                                room.center.y + offset.y
                                ), 
                                j == 1 ? Quaternion.Euler(0, !roomvertical ? 0 : 90, 0) : Quaternion.Euler(0, !roomvertical ? 180 : 270, 0), 
                                counterContainer.transform).transform;
                            if (roomvertical) offset.y += counterSize;
                            else offset.x += counterSize;

                            string itemToSpawn = GetItemFromLoottable(roomSpecificAssets.kitchenItemSpawns);
                            if (itemToSpawn != string.Empty)
                            {
                                PickupItem itemSpawned = Instantiate(roomSpecificAssets.itemPickup, new(
                                    counter.position.x, wallHeight, counter.position.z
                                    ), Quaternion.identity, itemSpawnsContainer.transform);
                                itemSpawned.itemToGive = ItemPresets.presets[itemToSpawn];
                            }
                        }
                        if (roomvertical) offset = new(offset.x + counterSize, -.5f * roomSpecificAssets.counterLength);
                        else offset = new(-.5f * roomSpecificAssets.counterLength, offset.y + counterSize);
                    }

                    break;
                case RoomType.seating:
                    break;
                case RoomType.storage:
                    break;
            }
            yield return new WaitForSeconds(generationInterval);
        }

        // Spawn enemies
        GameObject enemyContainer = new("EnemyContainer");
        enemyContainer.transform.parent = assetContainer.transform;
        navMeshSurface.BuildNavMesh();
        foreach (RectInt room in rooms)
        {
            if (room == originRoom) continue;
            int enemiesToSpawn = 0;
            int enemyroll = _random.Next(0, 100);
            for (int i = enemiesPerRoom.Length-1; i >= 0; i--)
            {
                if (enemyroll > 100 - enemiesPerRoom[i].probability)
                {
                    enemiesToSpawn = enemiesPerRoom[i].Quantity;
                    break;
                }
            }
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                Instantiate(enemy, new Vector3(room.center.x + i, -wallHeight * .5f, room.center.y + i + 3),
                    Quaternion.identity, enemyContainer.transform);
                yield return new WaitForSeconds(generationInterval);
            }
            yield return new();
        }

        // Spawn Player
        Destroy(Camera.main.gameObject);
        Instantiate(player, new Vector3(originRoom.center.x, -wallHeight * .5f, originRoom.center.y), Quaternion.identity);
        coroutineIsDone = true;
        yield return new WaitForSeconds(generationInterval);
    }

    int GetBiggestRoom(out int biggestIndex)
    {
        biggestIndex = 0;
        RectInt biggestRoom = new();

        for (int i = 0;i < rooms.Count; i++)
        {
            if (rooms[i].size.magnitude > biggestRoom.size.magnitude)
            {
                biggestIndex = i;
                biggestRoom = rooms[i];
            }
        }
        return biggestIndex;
    }
    int GetSmallestRoom(out int smallesttIndex)
    {
        smallesttIndex = 0;
        RectInt smallestRoom = originRoom;

        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].size.magnitude < smallestRoom.size.magnitude)
            {
                smallesttIndex = i;
                smallestRoom = rooms[i];
            }
        }
        return smallesttIndex;
    }
    int GetNearestToOrigin()
    {
        int _origin = 0;
        for(int i = 0; i < rooms.Count; i++)
            if (rooms[i].center.magnitude < rooms[_origin].center.magnitude) _origin = i;
        return _origin;
    }
    RectInt GetRoomByCenter(Vector2 center)
    {
        foreach(RectInt room in rooms) if (room.center == center) return room;
        return new();
    }
    RectInt GetDoorByCenter(Vector2 center)
    {
        foreach (RectInt door in doors) if (door.center == center) return door;
        return new();
    }

    string GetItemFromLoottable(ItemLootLable[] lootTable)
    {
        int probabilityPassed = lootTable[0].probability;
        int lootRoll = _random.Next(0, 100);
        for (int i = 0; i < lootTable.Length; i++)
        {
            if (lootRoll < probabilityPassed) return lootTable[i].itemName;
            else probabilityPassed += lootTable[i].probability;
        }
        return string.Empty;
    }
    public static int GetTotalItemProbability(ItemLootLable[] lootTable)
    {
        int probability = 0;
        foreach(var loot in lootTable) probability += loot.probability;
        return probability;
    }

    void RandomizeFraction() => fraction = (float)_random.Next(splitFractionRange.x, splitFractionRange.y) / 100f;

    void SplitHorizontal(RectInt _origianalRoom, float _fraction, out RectInt _roomA, out RectInt _roomB)
    {
        _roomA = new(_origianalRoom.xMin, 
                                    _origianalRoom.yMin, 
                                    _origianalRoom.width, 
                                    (int)(_origianalRoom.height * _fraction));

        _roomB = new(_origianalRoom.xMin,
                                    _origianalRoom.yMin + (int)(_origianalRoom.height * _fraction),
                                    _origianalRoom.width, 
                                    _origianalRoom.height - _roomA.height);
    }
    void SplitVertical(RectInt _origianalRoom, float _fraction, out RectInt _roomA, out RectInt _roomB)
    {
        _roomA = new(_origianalRoom.xMin, 
                                    _origianalRoom.yMin, 
                                    (int)(_origianalRoom.width * _fraction),
                                    _origianalRoom.height);

        _roomB = new((int)(_origianalRoom.xMin + _origianalRoom.width * _fraction),
                                    _origianalRoom.yMin,
                                    _origianalRoom.width - _roomA.width,
                                    _origianalRoom.height);
    }
    List<RectInt> SortListFromSmallToBig(List<RectInt> originalList)
    {
        List<RectInt> newList = new();
        while (originalList.Count > 0)
        {
            GetSmallestRoom(out int _smallesttIndex);

            newList.Add(originalList[_smallesttIndex]);
            originalList.RemoveAt(_smallesttIndex);
        }

        return newList;
    }

    void Update()
    {
        if (!displayVisualDebugging) return;

        DrawRectangle(initialRoom, height + 1, Color.red);

        // Draws rooms
        foreach (RectInt _room in rooms) { DrawRectangle(new(_room.xMin + splitOffset, _room.yMin + splitOffset, 
            _room.width - splitOffset, _room.height - splitOffset), 
            height, Color.magenta);
        }
        if (originRoom != new RectInt(0,0,0,0)) DrawRectangle(new(originRoom.xMin + splitOffset, originRoom.yMin + splitOffset,
            originRoom.width - splitOffset, originRoom.height - splitOffset), height + 1, Color.cyan);

        // Draws doors
        foreach (RectInt _door in doors)
        {
            DrawRectangle(new(_door.xMin, _door.yMin,
            _door.width, _door.height),
            height, Color.blue);
        }

        // Draws removed doors
        if (showDeletedDoors)
        {
            foreach (RectInt _door in removedDoors)
            {
                DrawRectangle(new(_door.xMin, _door.yMin,
                _door.width, _door.height),
                height, Color.black);
            }
        }

        // Draws graph lines
        foreach (KeyValuePair<Vector2, List<Vector2>> point in dungeonGraph.adjacencyList)
        {
            for(int i = 0; i < point.Value.Count; i++)
                Debug.DrawLine(new(point.Key.x, 1, point.Key.y), new(point.Value[i].x, 1, point.Value[i].y), Color.green);
        }
    }

    public static void DrawRectangle(RectInt _room, float _height, Color _color, float duration = 0.1f)
    {
        DebugExtension.DebugBounds(new Bounds(new Vector3(_room.center.x, 0, _room.center.y), 
            new Vector3(_room.width, _height, _room.height)),
            _color, duration);
    }
}
[System.Serializable]
public struct EnemyProbabilities
{
    public int Quantity;
    [Range(0, 100)] public int probability;
}
