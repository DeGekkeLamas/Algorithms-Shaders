using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using Unity.VisualScripting;
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
    RectInt originRoom;
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
    public bool showDeletedObjects = true;
    public bool disableVisualDebuggingAfterRoomGeneration = true;

    [Header("Generated stuff")]
    public NavMeshSurface navMeshSurface;
    [HideInInspector] public GameObject assetContainer;
    public List<RectInt> rooms = new(1);
    public List<RectInt> doors = new(1);
    public List<RectInt> removedObjects = new(1);
    Graph<Vector2> dungeonGraph = new();
    System.Random _random = new();

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
                        removedObjects.Add(_newDoor);
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
        int _roomsRemoved = 0;
        for (int i = 0; i < rooms.Count; i++)
        {
            if (dungeonGraph.adjacencyList[rooms[i].center].Count == 0)
            {
                RemoveRoom(rooms[i]);
                _roomsRemoved++;
                i--;
                yield return new WaitForSeconds(generationInterval);
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
        Debug.Log($"Removed {_roomsRemoved} inaccessible rooms, from {this}");

        // Reset seed to random after generation
        _random = new System.Random((int)DateTime.Now.Ticks);

        // Removes smallest rooms
        if (shouldRemoveSmallestRooms)
        {
            // Sorts rooms from biggest to smallest
            rooms = rooms.OrderByDescending(x => x.size.magnitude).ToList();
            rooms = InvertList(rooms);

            int roomsRemoved = 0;
            int roomsToRemove = rooms.Count / 10;

            // Checks if all rooms remain accessible with this room removed
            for(int i = 0; i < rooms.Count && roomsRemoved < roomsToRemove; i++)
            {
                RectInt room = rooms[i];
                DrawRectangle(room, 5, Color.white, .5f);
                yield return new WaitForSeconds(.1f);

                // Skip if originroom, remove without graphcheck if 1 door
                if (room == originRoom) continue;
                if (dungeonGraph.adjacencyList[room.center].Count <= 1)
                {
                    //RemoveRoom(room);
                    //roomsRemoved++;
                    //continue;
                }

                List<Vector2> roomsWoThis = new(_accessibleRooms);
                //foreach(Vector2 door in dungeonGraph.GetNeighbours(room.center))
                //{
                //    roomsWoThis.Remove(door);
                //}
                roomsWoThis.Remove(room.center);

                Graph<Vector2> dungeonGraphWoThis = dungeonGraph.CloneGraph();
                dungeonGraphWoThis.RemoveNode(room.center);

                List<Vector2> accessibleRoomsWoThis = dungeonGraphWoThis.BFS(originRoom.center);
                if (accessibleRoomsWoThis.Count == roomsWoThis.Count)
                {
                    RemoveRoom(room);
                    roomsRemoved++;
                    continue;
                }

                foreach(var node in accessibleRoomsWoThis)
                {
                    DrawRectangle(room, 10, new Color(1, .3f, 0,0), 1);
                }
                foreach(var node in roomsWoThis)
                {
                    DrawRectangle(room, 12, new Color(1, 0, 0, .5f), 1);
                }
                Debug.Log($"{accessibleRoomsWoThis.Count}, {roomsWoThis.Count}");
            }
            Debug.Log($"Removed smallest {roomsRemoved} rooms");
        }

        Debug.Log("Dungeon drawing done!");
        if (disableVisualDebuggingAfterRoomGeneration) displayVisualDebugging = false;
        coroutineIsDone = true;
    }

    /// <summary>
    /// Also works if lists contain same values in a different order
    /// </summary>
    static bool ListsAreEqual<T>(List<T> list1, List<T> list2)
    {
        if (list1.Count == list2.Count)
        {
            for (int i = 0; i < list1.Count; i++)
            {
                if (!list1.Contains(list2[i]))
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }
    void RemoveRoom(RectInt roomToRemove)
    {
        if (!dungeonGraph.adjacencyList.ContainsKey(roomToRemove.center))
        {
            Debug.Log($"Node {roomToRemove.center} is alreadu deleted dumbass");
            return;
        }

        removedObjects.Add(roomToRemove);
        rooms.Remove(roomToRemove);
        for(int i = dungeonGraph.adjacencyList[roomToRemove.center].Count; i > 0; i--)
        {
            Vector2 door = dungeonGraph.adjacencyList[roomToRemove.center][i - 1];
            RectInt doorRect = GetDoorByCenter(door);
            removedObjects.Add(doorRect);
            doors.Remove(doorRect);
            dungeonGraph.RemoveNode(door);
        }
            
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
    List<T> InvertList<T>(List<T> originalList)
    {
        List<T> newList = new(originalList);
        for(int i = 0; i < originalList.Count; i++)
        {
            newList[originalList.Count-1-i] = originalList[i];
        }

        return newList;
    }
    public RectInt GetOriginRoom() { return originRoom; }
    public System.Random GetSeed() { return _random; }

    void Update()
    {
        if (!displayVisualDebugging) return;

        DrawRectangle(initialRoom, 4 + 1, Color.red);

        // Draws rooms
        foreach (RectInt _room in rooms) { DrawRectangle(new(_room.xMin, _room.yMin, 
            _room.width, _room.height), 
            4, Color.magenta);
        }
        if (originRoom != new RectInt(0,0,0,0)) DrawRectangle(new(originRoom.xMin, originRoom.yMin,
            originRoom.width, originRoom.height), 4 + 1, Color.cyan);

        // Draws doors
        foreach (RectInt _door in doors)
        {
            DrawRectangle(new(_door.xMin, _door.yMin,
            _door.width, _door.height),
            4, Color.blue);
        }

        // Draws removed doors
        if (showDeletedObjects)
        {
            foreach (RectInt _obj in removedObjects)
            {
                DrawRectangle(new(_obj.xMin, _obj.yMin,
                _obj.width, _obj.height),
                4, Color.black);
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
