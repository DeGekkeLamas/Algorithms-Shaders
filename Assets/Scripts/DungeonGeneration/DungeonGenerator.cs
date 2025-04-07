using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float wallHeight = 5;
    public float wallBoundHeight = 1;
    public GameObject floor;

    [Header("Generated stuff")]
    public List<RectInt> rooms = new(1);
    public List<RectInt> doors = new(1);
    public List<RectInt> removedDoors = new(1);
    List<GameObject> wallsGenerated = new();
    Graph<Vector2> dungeonGraph = new();
    System.Random _random = new System.Random();

    private void OnValidate()
    {
        splitFractionRange = new(
            splitFractionRange.x,
            Mathf.Max(splitFractionRange.x, splitFractionRange.y)
            );
    }
    private void Awake()
    {
        if (seed != 0) _random = new System.Random(seed);

        StartCoroutine(GenerateRooms());
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
        StartCoroutine(GenerateDoors());
    }
    IEnumerator GenerateDoors()
    {
        // Door generation
        int _tolerance = 3; // variable for determining if a door is too close to the edge of its room

        for (int i = 0; i < rooms.Count; i++)
        {
            for (int j = i+1; j < rooms.Count; j++)
            {
                int roomsConnected = dungeonGraph.adjacencyList[rooms[i].center].Count;
                if (AlgorithmsUtils.Intersects(rooms[i], rooms[j]) &&
                    (rooms[i] != originRoom && roomsConnected < maxDoorsPerRoom ||
                    rooms[i] == originRoom && roomsConnected < maxDoorsForOriginRoom) &&
                    (rooms[j] != originRoom && roomsConnected < maxDoorsPerRoom ||
                    rooms[j] == originRoom && roomsConnected < maxDoorsForOriginRoom))
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
        StartCoroutine(RemoveRooms());
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
        StartCoroutine(GenerateInitialWalls());
    }
    IEnumerator GenerateInitialWalls()
    {
        // Generate walls
        GameObject wallContainer = new("WallContainer");

        for (int i = 0; i < rooms.Count; i++)
        {
            GameObject roomWallContainer = new($"Room{i}WallContainer");
            roomWallContainer.transform.parent = wallContainer.transform;

            Vector3 _center = new(rooms[i].center.x, 0, rooms[i].center.y);

            GameObject wallXPlus = Instantiate(wall, _center + new Vector3(rooms[i].width * 0.5f, 0, 0),
                Quaternion.identity, roomWallContainer.transform);
            GameObject wallXMin = Instantiate(wall, _center + new Vector3(-rooms[i].width * 0.5f, 0, 0),
                Quaternion.identity, roomWallContainer.transform);
            GameObject wallYPlus = Instantiate(wall, _center + new Vector3(0, 0, rooms[i].height * 0.5f),
                Quaternion.identity, roomWallContainer.transform);
            GameObject wallYMin = Instantiate(wall, _center + new Vector3(0, 0, -rooms[i].height * 0.5f),
                Quaternion.identity, roomWallContainer.transform);

            wallXPlus.transform.localScale = new(1, wallHeight, rooms[i].height);
            wallXMin.transform.localScale = new(1, wallHeight, rooms[i].height);
            wallYPlus.transform.localScale = new(rooms[i].width, wallHeight, 1);
            wallYMin.transform.localScale = new(rooms[i].width, wallHeight, 1);

            wallsGenerated.Add(wallXPlus);
            wallsGenerated.Add(wallYPlus);
            wallsGenerated.Add(wallXMin);
            wallsGenerated.Add(wallYMin);
            yield return null;
        }
        StartCoroutine(ModifyWalls());
        yield return new();
    }
    IEnumerator ModifyWalls()
    {
        // Check for door intersections
        int wallsQTY = wallsGenerated.Count;
        GameObject doorBoundsContainer = new("DoorBounds");
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
                    yield return null;

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
                    doorBoundA.transform.localScale = new Vector3(1, wallHeight + wallBoundHeight, 1);
                    doorBoundB.transform.localScale = new Vector3(1, wallHeight + wallBoundHeight, 1);
                }
            }
            yield return new();
        }

        // Remove fully overlapping walls
        for (int i = 0; i < wallsGenerated.Count; i++)
        {
            for (int j = i; j < wallsGenerated.Count - 1; j++)
            {
                if (i == j) continue;
                if (wallsGenerated[i].transform.position == wallsGenerated[j].transform.position)
                {
                    Destroy(wallsGenerated[i]);
                    wallsGenerated.RemoveAt(i);
                }
            }
        }
        // Generate bounds on top of and below walls
        GameObject wallBoundContainer = new("WallBoundContainer");
        foreach (GameObject wall in wallsGenerated)
        {
            GameObject wallboundTop = Instantiate(wallBound,
                new Vector3(wall.transform.position.x, 
        wall.transform.position.y + 0.5f * wall.transform.localScale.y, wall.transform.position.z), 
                Quaternion.identity, wallBoundContainer.transform);
            wallboundTop.transform.localScale = new Vector3(wall.transform.localScale.x, 
                wallBoundHeight, wall.transform.localScale.z);
            GameObject wallboundBottom = Instantiate(wallBound,
                new Vector3(wall.transform.position.x, 
        wall.transform.position.y - 0.5f * wall.transform.localScale.y, wall.transform.position.z), 
                Quaternion.identity, wallBoundContainer.transform);
            wallboundBottom.transform.localScale = new Vector3(wall.transform.localScale.x, 
                wallBoundHeight, wall.transform.localScale.z);
        yield return null;
        }
        StartCoroutine(GenerateFloor());
    }
    IEnumerator GenerateFloor()
    {
        // Generate floor
        GameObject floorContainer = new GameObject("FloorContainer");
        foreach (RectInt room in rooms)
        {
            GameObject _floor = Instantiate(floor, new Vector3(room.center.x, -wallHeight * .5f, room.center.y),
                Quaternion.identity, floorContainer.transform);
            _floor.transform.localScale = new Vector3(room.width - 1, 1, room.height - 1) / 10;
            yield return new WaitForSeconds(generationInterval);
            yield return null;
        }
        foreach (RectInt door in doors)
        {
            GameObject doorFloor = Instantiate(floor, new Vector3(door.center.x, -wallHeight * .5f, door.center.y),
                Quaternion.identity, floorContainer.transform);
            doorFloor.transform.localScale = new Vector3(door.width, 1, door.height) / 10;
            yield return new WaitForSeconds(generationInterval);
            yield return null;
        }
        StartCoroutine(Brickify());
        yield return new();

    }
    IEnumerator Brickify()
    {
        // Brickifies generated walls
        foreach (var wall in wallsGenerated)
        {
            wall.AddComponent<WallGenerator>();
            yield return null;
        }
        Debug.Log("Generated all room assets!");
        yield return new();
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
