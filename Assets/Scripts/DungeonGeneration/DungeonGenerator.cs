using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;

namespace DungeonGeneration
{

    [RequireComponent(typeof(BetterDungeonAssetGenerator), typeof(LameDungeonAssetGenerator), typeof(RoomAssetGenerator))]
    public class DungeonGenerator : MonoBehaviour
    {
        RoomAssetGenerator rda;
        public RoomAssetGenerator Rda => rda;
        BetterDungeonAssetGenerator bda; // Cool generation
        LameDungeonAssetGenerator lda; // Lame generation

        [Header("Generation properties")]
        [Tooltip("Leave as 0 to use a random seed")]
        public int seed;
        public bool shouldRemoveSmallestRooms;
        [Range(0, 100)] public int percentSmallestRoomsToRemove = 10;
        public RectInt initialRoom = new(0, 0, 100, 100);
        RectInt _originRoom;
        public RectInt originRoom => _originRoom;
        public enum GenerationType { Cool, Lame } // Cool is for good requirements, lame is for sufficient requirements
        public GenerationType generationType = GenerationType.Cool;

        float fraction = 0.5f;
        public Vector2Int splitFractionRange = new(35, 66);
        public float roomMaxSize = 45;
        public int maxDoorsForOriginRoom = 1;
        public int maxDoorsPerRoom = 3;

        [Header("Display properties")]
        public bool displayVisualDebugging = true;
        public float generationInterval = .1f;
        public YieldInstruction interval;
        public int doorWidth = 1;
        public bool showDeletedObjects = true;
        public bool disableVisualDebuggingAfterRoomGeneration = true;

        [Header("Generated stuff")]
        public NavMeshSurface navMeshSurface;
        [HideInInspector] public GameObject assetContainer;
        public List<RectInt> rooms = new(1);
        public List<RectInt> doors = new(1);
        public List<RectInt> removedObjects = new(1);
        Graph<Vector2> _dungeonGraph = new();
        System.Random _random = new();
        public System.Random random => _random;
        List<Vector2> _accessibleRooms = new();
        public int[,] tilemap;

        [Header("Debug")]
        public DrawTilemap tilemapDebugger;

        private void OnValidate()
        {
            splitFractionRange = new(
                splitFractionRange.x,
                Mathf.Max(splitFractionRange.x, splitFractionRange.y)
                );
            initialRoom.xMin = Mathf.Max(initialRoom.xMin, 0);
            initialRoom.yMin = Mathf.Max(initialRoom.yMin, 0);

            if (generationType == GenerationType.Lame && doorWidth % 2 == 0)
            {
                doorWidth += 1;
            }
        }
        private void Awake()
        {
            interval = new WaitForSeconds(generationInterval);
            rda = this.GetComponent<RoomAssetGenerator>();
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
        IEnumerator GenerateDungeon()
        {
            assetContainer = new GameObject("AssetContainer");

            yield return StartCoroutine(GenerateRooms()); // Rooms
            yield return StartCoroutine(GenerateDoors()); // Doors
            yield return StartCoroutine(RemoveUnreachableRooms()); // Check for room accessibility and delete unreachable rooms
            yield return StartCoroutine(RemoveSmallestRooms()); // Remove smallest rooms if enabled
            rda.AssignRoomTypes(); // Assign different types of rooms
            switch (generationType)
            {
                case GenerationType.Cool:
                    yield return StartCoroutine(bda.GenerateTileMap()); // Tilemap
                    yield return StartCoroutine(bda.GenerateWalls()); // Walls
                    yield return StartCoroutine(bda.GenerateFloor(Vector2Int.RoundToInt(_originRoom.center))); // Floor
                    yield return StartCoroutine(lda.GenerateFloor(bda.floorCollider)); // Add colliders
                    yield return StartCoroutine(rda.Brickify()); // Carve walls into bricks
                    break;
                case GenerationType.Lame:
                    yield return StartCoroutine(lda.GenerateWalls()); // Walls
                    yield return StartCoroutine(lda.GenerateFloor(lda.floor)); // Floor
                    break;
            }
            yield return StartCoroutine(rda.SpawnObjects());
            yield return StartCoroutine(bda.SpawnPlayer()); // Player
            FillInaccessibleSpaces();
            yield return StartCoroutine(rda.SpawnEnemies());
            Debug.Log("Generated all room assets!");
        }
        /// <summary>
        /// Draws out initial rooms
        /// </summary>
        IEnumerator GenerateRooms()
        {
            rooms.Add(initialRoom);

            yield return new WaitForSeconds(0.1f);

            // Room generation
            while (rooms[GetBiggestRoom(out int biggestIndex)].size.magnitude > roomMaxSize)
            {
                RectInt biggestRoom = rooms[biggestIndex];
                RandomizeFraction();
                if (biggestRoom.width > biggestRoom.height)
                {
                    SplitVertical(biggestRoom, fraction, out RectInt newRoomA, out RectInt newRoomB);
                    rooms.Add(newRoomA);
                    rooms[biggestIndex] = newRoomB;
                }
                else
                {
                    SplitHorizontal(biggestRoom, fraction, out RectInt newRoomA, out RectInt newRoomB);
                    rooms.Add(newRoomA);
                    rooms[biggestIndex] = newRoomB;
                }
                yield return interval;
            }
            foreach (var room in rooms) { _dungeonGraph.AddNode(room.center); }
            _originRoom = rooms[GetNearestToOrigin()];

            Debug.Log($"Generated all rooms, from {this}");
        }
        /// <summary>
        /// Draws out doors
        /// </summary>
        IEnumerator GenerateDoors()
        {
            // Door generation
            const int _tolerance = 3; // variable for determining if a door is too close to the edge of its room

            for (int i = 0; i < rooms.Count; i++)
            {
                for (int j = i + 1; j < rooms.Count; j++)
                {
                    int roomsConnectedI = _dungeonGraph.adjacencyList[rooms[i].center].Count;
                    int roomsConnectedJ = _dungeonGraph.adjacencyList[rooms[j].center].Count;
                    if (AlgorithmsUtils.Intersects(rooms[i], rooms[j]) &&
                        (rooms[i] != _originRoom && roomsConnectedI < maxDoorsPerRoom ||
                        rooms[i] == _originRoom && roomsConnectedI < maxDoorsForOriginRoom) &&
                        (rooms[j] != _originRoom && roomsConnectedJ < maxDoorsPerRoom ||
                        rooms[j] == _originRoom && roomsConnectedJ < maxDoorsForOriginRoom))
                    {
                        var newDoor = AlgorithmsUtils.Intersect(rooms[i], rooms[j]);

                        if (newDoor.width > newDoor.height)
                        {
                            newDoor.xMin += (int)(newDoor.width * .5f - doorWidth * .5f);
                            newDoor.width = doorWidth;
                        }
                        else
                        {
                            newDoor.yMin += (int)(newDoor.height * .5f - doorWidth * .5f);
                            newDoor.height = doorWidth;
                        }

                        // Remove corner doors
                        if (Mathf.Abs(rooms[i].center.x - newDoor.center.x) > rooms[i].width / _tolerance &&
                            Mathf.Abs(rooms[i].center.y - newDoor.center.y) > rooms[i].height / _tolerance ||
                            Mathf.Abs(rooms[j].center.x - newDoor.center.x) > rooms[j].width / _tolerance &&
                            Mathf.Abs(rooms[j].center.y - newDoor.center.y) > rooms[j].height / _tolerance)
                        {
                            Debug.Log($"Removed corner door, from {this}");
                            removedObjects.Add(newDoor);
                            yield return interval;
                            continue;
                        }

                        doors.Add(newDoor);

                        _dungeonGraph.AddNode(newDoor.center);
                        _dungeonGraph.AddEdge(newDoor.center, rooms[i].center);
                        _dungeonGraph.AddEdge(newDoor.center, rooms[j].center);

                        yield return interval;
                    }
                }
            }
            Debug.Log($"Generated all doors, from {this}");
        }
        /// <summary>
        /// Check for room accessibility and delete unreachable rooms
        /// </summary>
        IEnumerator RemoveUnreachableRooms()
        {
            // Removes rooms with 0 doors
            int roomsRemovedAccessibility = 0;
            for (int i = 0; i < rooms.Count; i++)
            {
                if (_dungeonGraph.adjacencyList[rooms[i].center].Count == 0)
                {
                    RemoveRoom(rooms[i]);
                    roomsRemovedAccessibility++;
                    i--;
                    yield return interval;
                }
            }

            // removes rooms with doors but no way to reach origin
            _accessibleRooms = _dungeonGraph.BFS(_originRoom.center);

            List<Vector2> roomsToRemove = new();
            foreach (KeyValuePair<Vector2, List<Vector2>> point in _dungeonGraph.adjacencyList)
            {
                if (!_accessibleRooms.Contains(point.Key)) roomsToRemove.Add(point.Key);
            }

            foreach (Vector2 room in roomsToRemove)
            {
                RemoveRoom(GetRoomByCenter(room));
                roomsRemovedAccessibility++;
                yield return interval;
            }
            if (roomsRemovedAccessibility != 0) Debug.Log($"Removed {roomsRemovedAccessibility} inaccessible rooms");
            else Debug.Log("All rooms were accessible, no rooms had to be removed");

        }
        /// <summary>
        /// Removes smallest rooms if enabled
        /// </summary>
        IEnumerator RemoveSmallestRooms()
        {
            // Removes smallest rooms
            if (shouldRemoveSmallestRooms)
            {
                // Sorts rooms from biggest to smallest
                rooms = rooms.OrderByDescending(x => x.size.magnitude).ToList();
                rooms = InvertList(rooms);

                int roomsRemovedSmallest = 0;
                int roomsToRemoveQTY = rooms.Count * percentSmallestRoomsToRemove / 100;

                // Checks if all rooms remain accessible with this room removed
                for (int i = 0; i < rooms.Count && roomsRemovedSmallest < roomsToRemoveQTY; i++)
                {
                    RectInt room = rooms[i];
                    DrawRectangle(room, 5, Color.white, .5f);
                    yield return interval;

                    // Skip if originroom, remove without graphcheck if 1 door
                    if (room == _originRoom) continue;
                    if (_dungeonGraph.adjacencyList[room.center].Count <= 1)
                    {
                        RemoveRoom(room);
                        roomsRemovedSmallest++;
                        continue;
                    }

                    List<Vector2> roomsWoThis = new(_accessibleRooms);
                    roomsWoThis.Remove(room.center);
                    List<Vector2> accessibleRoomsWoThis = _dungeonGraph.BFSWithout(_originRoom.center, room.center);

                    if (accessibleRoomsWoThis.Count == roomsWoThis.Count)
                    {
                        foreach (var door in _dungeonGraph.GetNeighbours(room.center))
                        {
                            _accessibleRooms.Remove(door);
                        }
                        _accessibleRooms.Remove(room.center);
                        RemoveRoom(room);
                        roomsRemovedSmallest++;
                        continue;
                    }
                }
                Debug.Log($"Removed smallest {roomsRemovedSmallest} rooms");
            }

            Debug.Log("Dungeon drawing done!");
            yield return null;
        }

        [Button]
        public void DrawTilemap()
        {
            if (tilemapDebugger != null)
            {
                tilemapDebugger.DrawMap(tilemap);
            }
        }

        /// <summary>
        /// Remove a room, also removes it and its attached doors from the dungeongraph
        /// </summary>
        void RemoveRoom(RectInt roomToRemove)
        {
            if (!_dungeonGraph.adjacencyList.ContainsKey(roomToRemove.center))
            {
                Debug.Log($"Node {roomToRemove.center} is alreadu deleted dumbass");
                return;
            }

            removedObjects.Add(roomToRemove);
            rooms.Remove(roomToRemove);
            for (int i = _dungeonGraph.adjacencyList[roomToRemove.center].Count; i > 0; i--)
            {
                Vector2 door = _dungeonGraph.adjacencyList[roomToRemove.center][i - 1];
                RectInt doorRect = GetDoorByCenter(door);
                removedObjects.Add(doorRect);
                doors.Remove(doorRect);
                _dungeonGraph.RemoveNode(door);
            }

            _dungeonGraph.RemoveNode(roomToRemove.center);
        }

        public void FillInaccessibleSpaces()
        {
            AlgorithmsUtils.FillRectangleOutline(tilemap, new(initialRoom.xMin, initialRoom.yMin, initialRoom.width+2, initialRoom.height+2), 1);
            foreach(RectInt obj in removedObjects)
            {
                AlgorithmsUtils.FillRectangle(tilemap, obj, 1);
            }
        }

        /// <summary>
        /// Gets biggest room
        /// </summary>
        int GetBiggestRoom(out int biggestIndex)
        {
            biggestIndex = 0;
            RectInt biggestRoom = new();

            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].size.magnitude > biggestRoom.size.magnitude)
                {
                    biggestIndex = i;
                    biggestRoom = rooms[i];
                }
            }
            return biggestIndex;
        }
        /// <summary>
        /// Gets room nearest (0,0,0)
        /// </summary>
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
        /// <summary>
        /// Find a room by its center
        /// </summary>
        RectInt GetRoomByCenter(Vector2 center)
        {
            foreach (RectInt room in rooms) if (room.center == center) return room;
            return new();
        }
        /// <summary>
        /// Find a door by its center
        /// </summary>
        RectInt GetDoorByCenter(Vector2 center)
        {
            foreach (RectInt door in doors) if (door.center == center) return door;
            return new();
        }

        /// <summary>
        /// Randomize the fraction at which rooms are split
        /// </summary>
        void RandomizeFraction() => fraction = (float)_random.Next(splitFractionRange.x, splitFractionRange.y) / 100f;

        /// <summary>
        /// Splits a room into 2 over horizontal axis, new rooms are returned as out variables
        /// </summary>
        void SplitHorizontal(RectInt origianalRoom, float fraction, out RectInt roomA, out RectInt roomB)
        {
            roomA = new(origianalRoom.xMin,
                                        origianalRoom.yMin,
                                        origianalRoom.width,
                                        (int)(origianalRoom.height * fraction) + 1);

            roomB = new(origianalRoom.xMin,
                                        origianalRoom.yMin + (int)(origianalRoom.height * fraction),
                                        origianalRoom.width,
                                        origianalRoom.height - roomA.height + 1);
        }
        /// <summary>
        /// Splits a room into 2 over vertical axis, new rooms are returned as out variables
        /// </summary>
        void SplitVertical(RectInt origianalRoom, float fraction, out RectInt roomA, out RectInt roomB)
        {
            roomA = new(origianalRoom.xMin,
                                        origianalRoom.yMin,
                                        (int)(origianalRoom.width * fraction) + 1,
                                        origianalRoom.height);

            roomB = new((int)(origianalRoom.xMin + origianalRoom.width * fraction),
                                        origianalRoom.yMin,
                                        origianalRoom.width - roomA.width + 1,
                                        origianalRoom.height);
        }
        /// <summary>
        /// reverse the order of a list
        /// </summary>
        List<T> InvertList<T>(List<T> originalList)
        {
            List<T> newList = new(originalList);
            for (int i = 0; i < originalList.Count; i++)
            {
                newList[originalList.Count - 1 - i] = originalList[i];
            }

            return newList;
        }

        /// <summary>
        /// Get the sides at which this room is connected to a door
        /// </summary>
        public Vector2[] GetDoorDirections(RectInt room)
        {
            List<Vector2> doors = new(_dungeonGraph.GetNeighbours(room.center));
            for (int i = 0; i < doors.Count;i++)
            {
                doors[i] -= room.center;
                doors[i] = doors[i].normalized;
            }
            return doors.ToArray();
        }

        void Update()
        {
            if (!displayVisualDebugging) return;

            DrawRectangle(initialRoom, 4 + 1, Color.red);

            // Draws rooms
            foreach (RectInt room in rooms)
            {
                DrawRectangle(new(room.xMin, room.yMin,
                room.width, room.height),
                4, Color.magenta);
            }
            if (_originRoom != new RectInt(0, 0, 0, 0)) DrawRectangle(new(_originRoom.xMin, _originRoom.yMin,
                _originRoom.width, _originRoom.height), 4 + 1, Color.cyan);

            // Draws doors
            foreach (RectInt door in doors)
            {
                DrawRectangle(new(door.xMin, door.yMin,
                door.width, door.height),
                4, Color.blue);
            }

            // Draws removed doors
            if (showDeletedObjects)
            {
                foreach (RectInt obj in removedObjects)
                {
                    DrawRectangle(new(obj.xMin, obj.yMin,
                    obj.width, obj.height),
                    4, Color.black);
                }
            }

            // Draws graph lines
            foreach (KeyValuePair<Vector2, List<Vector2>> point in _dungeonGraph.adjacencyList)
            {
                for (int i = 0; i < point.Value.Count; i++)
                    Debug.DrawLine(new(point.Key.x, 1, point.Key.y), new(point.Value[i].x, 1, point.Value[i].y), Color.green);
            }
        }
        /// <summary>
        /// Draw debug rectangle
        /// </summary>
        public static void DrawRectangle(RectInt room, float height, Color color, float duration = 0.1f)
        {
            DebugExtension.DebugBounds(new Bounds(new Vector3(room.center.x, 0, room.center.y),
                new Vector3(room.width, height, room.height)),
                color, duration);
        }
        /// <summary>
        /// Cancels generation for debug purposes
        /// </summary>
        [ContextMenu("Cancel generation")]
        void CancelGeneration()
        {
            StopAllCoroutines();
            Debug.Log("Cancelled dungeon generation");
        }
    }

}