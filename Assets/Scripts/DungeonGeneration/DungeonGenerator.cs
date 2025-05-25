using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    DungeonAssetGenerator da;
    BetterDungeonAssetGenerator bda;
    LameDungeonAssetGenerator lda;

    [Header("Generation properties")]
    [Tooltip("Leave as 0 to use a random seed")]
    public int seed;
    public bool shouldRemoveSmallestRooms;
    public RectInt initialRoom = new(0, 0, 100, 100);
    [HideInInspector] public RectInt originRoom;
    public float height = 5;
    public enum GenerationType { Old, Cool, Lame} // Cool is for good requirements, lame is for sufficient requirements
    public GenerationType generationType = GenerationType.Cool;

    float fraction = 0.5f;
    public Vector2Int splitFractionRange = new(35, 66);
    public float roomMaxSize = 45;
    public int maxDoorsForOriginRoom = 1;
    public int maxDoorsPerRoom = 3;

    [Header("Display properties")]
    public bool displayVisualDebugging = true;
    public float generationInterval = .1f;
    public int doorWidth = 1;
    public bool showDeletedDoors = true;
    public bool disableVisualDebuggingAfterRoomGeneration = true;

    [Header("Generated stuff")]
    public NavMeshSurface navMeshSurface;
    [HideInInspector] public GameObject assetContainer;
    public List<RectInt> rooms = new(1);
    public List<RectInt> doors = new(1);
    public List<RectInt> removedDoors = new(1);
    [HideInInspector]  public List<GameObject> wallsGenerated = new();
    [HideInInspector] public Graph<Vector2> dungeonGraph = new();
    [HideInInspector] public System.Random _random = new();

    private void OnValidate()
    {
        splitFractionRange = new(
            splitFractionRange.x,
            Mathf.Max(splitFractionRange.x, splitFractionRange.y)
            );
        initialRoom.xMin = Mathf.Max(initialRoom.xMin, 0);
        initialRoom.yMin = Mathf.Max(initialRoom.yMin, 0);

        if (generationType == GenerationType.Lame && doorWidth%2 == 0)
        {
            doorWidth += 1;
        }
    }
    private void Awake()
    {
        da = this.GetComponent<DungeonAssetGenerator>();
        bda = this.GetComponent<BetterDungeonAssetGenerator>();
        lda = this.GetComponent<LameDungeonAssetGenerator>();
        if (seed == 0)
        {
            seed = _random.Next(int.MinValue, int.MaxValue);
            Debug.Log($"Random chosen seed is {seed}");
        }
        _random = new System.Random(seed);

        StartCoroutine(GenerateDungeon());
    }
    [HideInInspector] public bool coroutineIsDone;
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
        StartCoroutine(da.AssignRoomTypes());
        yield return new WaitUntil(() => coroutineIsDone);
        coroutineIsDone = false;
        switch(generationType)
        {
            case GenerationType.Old:
                StartCoroutine(da.GenerateInitialWalls());
                yield return new WaitUntil(() => coroutineIsDone);
                coroutineIsDone = false;
                StartCoroutine(da.ModifyWalls());
                yield return new WaitUntil(() => coroutineIsDone);
                coroutineIsDone = false;
                StartCoroutine(da.GenerateFloor());
                yield return new WaitUntil(() => coroutineIsDone);
                coroutineIsDone = false;
                StartCoroutine(da.Brickify());
                yield return new WaitUntil(() => coroutineIsDone);
                coroutineIsDone = false;
                StartCoroutine(da.SpawnObjects());
                yield return new WaitUntil(() => coroutineIsDone);
                break;
            case GenerationType.Cool:
                StartCoroutine(bda.GenerateTileMap());
                yield return new WaitUntil(() => coroutineIsDone);
                coroutineIsDone = false;
                StartCoroutine(bda.GenerateWalls());
                yield return new WaitUntil(() => coroutineIsDone);
                coroutineIsDone = false;
                StartCoroutine(bda.GenerateFloor(Vector2Int.RoundToInt(originRoom.center)));
                yield return new WaitUntil(() => coroutineIsDone);
                coroutineIsDone = false;
                StartCoroutine(da.Brickify());
                yield return new WaitUntil(() => coroutineIsDone);
                coroutineIsDone = false;
                StartCoroutine(bda.SpawnPlayer());
                yield return new WaitUntil(() => coroutineIsDone);
                coroutineIsDone = false;
                break;
            case GenerationType.Lame:
                StartCoroutine(lda.GenerateWalls());
                yield return new WaitUntil(() => coroutineIsDone);
                coroutineIsDone = false;
                StartCoroutine(lda.GenerateFloor());
                yield return new WaitUntil(() => coroutineIsDone);
                coroutineIsDone = false;
                StartCoroutine(lda.SpawnPlayer());
                yield return new WaitUntil(() => coroutineIsDone);
                coroutineIsDone = false;
                break;
        }
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

                    if (_newDoor.width > _newDoor.height)
                    {
                        _newDoor.xMin += (int)(_newDoor.width * .5f - doorWidth * .5f);
                        _newDoor.width = doorWidth;
                    }
                    else
                    {
                        _newDoor.yMin += (int)(_newDoor.height * .5f - doorWidth * .5f);
                        _newDoor.height = doorWidth;
                    }


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
            RemoveRoom(GetRoomByCenter(room));
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

            int roomsRemoved = 0;
            int roomsToRemove = (int)(rooms.Count * 0.1f);

            // Checks if all rooms remain accessible with this room removed
            for(int i = 0; i < rooms.Count && roomsRemoved < roomsToRemove; i++)
            {
                RectInt room = rooms[i];
                // Skip if originroom, remove without graphcheck if 1 door
                if (room == originRoom) continue;
                if (dungeonGraph.adjacencyList[room.center].Count <= 1)
                {
                    RemoveRoom(room);
                    roomsRemoved++;
                    continue;
                }

                List<Vector2> roomsWoThis = new(_accessibleRooms);
                roomsWoThis.Remove(room.center);

                Graph<Vector2> dungeonGraphWoThis = dungeonGraph;

                List<Vector2> accessibleRoomsWoThis = dungeonGraphWoThis.BFS(originRoom.center);
                if (accessibleRoomsWoThis == _accessibleRooms)
                {

                }
            }
            Debug.Log($"Removed smallest {roomsRemoved} rooms");
        }

        Debug.Log("Dungeon drawing done!");
        if (disableVisualDebuggingAfterRoomGeneration) displayVisualDebugging = false;
        coroutineIsDone = true;
    }
    
    void RemoveRoom(RectInt roomToRemove)
    {
        if (!dungeonGraph.adjacencyList.ContainsKey(roomToRemove.center))
        {
            Debug.Log($"Node {roomToRemove.center} is alreadu deleted dumbass");
            return;
        }

        rooms.Remove(roomToRemove);
        foreach (Vector2 door in dungeonGraph.adjacencyList[roomToRemove.center])
            doors.Remove(GetDoorByCenter(door));
        dungeonGraph.RemoveNode(roomToRemove.center);
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
        Vector2Int offset = new(initialRoom.xMin, initialRoom.yMin);

        float latestDistance = float.MaxValue;
        for (int i = 0; i < rooms.Count; i++)
        {
            float distanceFromOrigin = (rooms[i].center - offset).magnitude;
            if (distanceFromOrigin < latestDistance)
            {
                _origin = i;
                latestDistance = distanceFromOrigin;
            }
        }
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
                                    (int)(_origianalRoom.height * _fraction) + 1);

        _roomB = new(_origianalRoom.xMin,
                                    _origianalRoom.yMin + (int)(_origianalRoom.height * _fraction),
                                    _origianalRoom.width, 
                                    _origianalRoom.height - _roomA.height + 1);
    }
    void SplitVertical(RectInt _origianalRoom, float _fraction, out RectInt _roomA, out RectInt _roomB)
    {
        _roomA = new(_origianalRoom.xMin, 
                                    _origianalRoom.yMin, 
                                    (int)(_origianalRoom.width * _fraction) + 1,
                                    _origianalRoom.height);

        _roomB = new((int)(_origianalRoom.xMin + _origianalRoom.width * _fraction),
                                    _origianalRoom.yMin,
                                    _origianalRoom.width - _roomA.width + 1,
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
        foreach (RectInt _room in rooms) { DrawRectangle(new(_room.xMin, _room.yMin, 
            _room.width, _room.height), 
            height, Color.magenta);
        }
        if (originRoom != new RectInt(0,0,0,0)) DrawRectangle(new(originRoom.xMin, originRoom.yMin,
            originRoom.width, originRoom.height), height + 1, Color.cyan);

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
    [ContextMenu("Cancel generation")]
    void CancelGeneration()
    {
        StopAllCoroutines();
        Debug.Log("Cancelled dungeon generation");
    }
}
