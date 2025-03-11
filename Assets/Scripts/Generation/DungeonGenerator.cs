using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public RectInt initialRoom = new(0, 0, 100, 100);
    //RectInt initialRoom = new(30, 30, 30, 30);
    public float height = 5;

    public List<RectInt> rooms = new(1);
    public List<RectInt> doors = new(1);

    public List<RectInt> removedDoors = new(1);

    float fraction = 0.5f;
    public int splitDepth;
    public int splitOffset = 1;

    public int doorWidth = 1;
    public int maxDoorsPerRoom = 3;

    Graph<Vector2> dungeonGraph = new();

    private void Awake() => StartCoroutine(GenerateRooms());
    IEnumerator GenerateRooms()
    {
        rooms.Add(initialRoom);

        yield return new WaitForSeconds(0.1f);

        // Room generation
        for (int i = 0; i < splitDepth; i++)
        {
            int _biggestRoom = GetBiggestRoom();

            RandomizeFraction();
            if (rooms[_biggestRoom].width > rooms[_biggestRoom].height)
            {
                rooms.Add(SplitVertical(rooms[_biggestRoom], fraction).Item1);
                rooms[_biggestRoom] = SplitVertical(rooms[_biggestRoom], fraction).Item2;
            }
            else
            {
                rooms.Add(SplitHorizontal(rooms[_biggestRoom], fraction).Item1);
                rooms[_biggestRoom] = SplitHorizontal(rooms[_biggestRoom], fraction).Item2;
            }
            yield return new WaitForSeconds(0.1f);
        }
        foreach (var room in rooms) { dungeonGraph.AddNode(room.center); }

        Debug.Log($"Generated all rooms, from {this}");

        // Door generation
        int _tolerance = 3;
        for (int i = 0;i < rooms.Count; i++)
        {
            for (int j = i; j < rooms.Count; j++)
            {
                if (i != j && AlgorithmsUtils.Intersects(rooms[i], rooms[j]) && 
                    dungeonGraph.adjacencyList[rooms[i].center].Count < maxDoorsPerRoom &&
                    dungeonGraph.adjacencyList[rooms[j].center].Count < maxDoorsPerRoom)
                {
                    var _newDoor = AlgorithmsUtils.Intersect(rooms[i], rooms[j]);

                    _newDoor = new(
                        (_newDoor.width <= 0) ? _newDoor.xMin - doorWidth/2 : (_newDoor.xMin) + _newDoor.width/2,
                        (_newDoor.height <= 0) ? _newDoor.yMin - doorWidth/2 : (_newDoor.yMin) + _newDoor.height/2,
                        (_newDoor.width <= 0) ? _newDoor.width + doorWidth : doorWidth,
                        (_newDoor.height <= 0) ? _newDoor.height + doorWidth : doorWidth);

                    if (Mathf.Abs(rooms[i].center.x - _newDoor.center.x) > rooms[i].width / _tolerance &&
                        Mathf.Abs(rooms[i].center.y - _newDoor.center.y) > rooms[i].height / _tolerance ||
                        Mathf.Abs(rooms[j].center.x - _newDoor.center.x) > rooms[j].width / _tolerance &&
                        Mathf.Abs(rooms[j].center.y - _newDoor.center.y) > rooms[j].height / _tolerance)
                    {
                        Debug.Log($"Removed corner door, from {this}");
                        removedDoors.Add(_newDoor);
                        continue;
                    }

                    doors.Add(_newDoor);

                    dungeonGraph.AddNode(_newDoor.center);
                    dungeonGraph.AddEdge(_newDoor.center, rooms[i].center);
                    dungeonGraph.AddEdge(_newDoor.center, rooms[j].center);

                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        Debug.Log($"Generated all doors, from {this}");

        // Removed inaccessible rooms
        List<Vector2> _accessibleRooms = dungeonGraph.BFS(rooms[0].center);
        foreach (Vector2 room in _accessibleRooms)
        {
            if (!dungeonGraph.adjacencyList.ContainsKey(room))
            {

            }
        }

        int _doorsRemoved = 0;
        for(int i = 0; i < rooms.Count; i++)
        {
            if (dungeonGraph.adjacencyList[ rooms[i].center ].Count == 0)
            {
                dungeonGraph.adjacencyList.Remove(rooms[i].center);
                rooms.RemoveAt(i);
                _doorsRemoved++;
                i--;
                yield return new WaitForSeconds(0.1f);
            }
        }

        Debug.Log($"Removed {_doorsRemoved} inaccessible rooms, from {this}");
    }
    int GetBiggestRoom()
    {
        int biggestIndex = 0;
        RectInt biggestRoom = new(0,0,0,0);

        for (int i = 0;i < rooms.Count; i++)
        {
            if (rooms[i].width * rooms[i].height > biggestRoom.width * biggestRoom.height)
            {
                biggestIndex = i;
                biggestRoom = rooms[i];
            }
        }
        return biggestIndex;
    }
    void RandomizeFraction() => fraction = (float)Random.Range(35, 66) / 100f;

    (RectInt, RectInt) SplitHorizontal(RectInt _origianalRoom, float _fraction)
    {
        RectInt _roomA = new(_origianalRoom.xMin, 
                                    _origianalRoom.yMin, 
                                    _origianalRoom.width, 
                                    (int)(_origianalRoom.height * _fraction));

        RectInt _roomB = new(_origianalRoom.xMin,
                                    _origianalRoom.yMin + (int)(_origianalRoom.height * _fraction),
                                    _origianalRoom.width, 
                                    _origianalRoom.height - _roomA.height);
        return (_roomA, _roomB);
    }
    (RectInt, RectInt) SplitVertical(RectInt _origianalRoom, float _fraction)
    {
        RectInt _roomA = new(_origianalRoom.xMin, 
                                    _origianalRoom.yMin, 
                                    (int)(_origianalRoom.width * _fraction),
                                    _origianalRoom.height);

        RectInt _roomB = new((int)(_origianalRoom.xMin + _origianalRoom.width * _fraction),
                                    _origianalRoom.yMin,
                                    _origianalRoom.width - _roomA.width,
                                    _origianalRoom.height);
        return (_roomA, _roomB);
    }

    void Update()
    {
        DrawRectangle(initialRoom, height + 1, Color.red);

        // Draws rooms
        foreach (RectInt _room in rooms) { DrawRectangle(new(_room.xMin + splitOffset, _room.yMin + splitOffset, 
            _room.width - splitOffset, _room.height - splitOffset), 
            height, Color.magenta);
        }

        // Draws doors
        foreach (RectInt _door in doors)
        {
            DrawRectangle(new(_door.xMin, _door.yMin,
            _door.width, _door.height),
            height, Color.blue);
        }

        // Draws removed doors
        foreach (RectInt _door in removedDoors)
        {
            DrawRectangle(new(_door.xMin, _door.yMin,
            _door.width, _door.height),
            height, Color.black);
        }

        // Draws graph lines
        foreach (KeyValuePair<Vector2, List<Vector2>> point in dungeonGraph.adjacencyList)
        {
            for(int i = 0; i < point.Value.Count; i++)
                Debug.DrawLine(new(point.Key.x, 1, point.Key.y), new(point.Value[i].x, 1, point.Value[i].y), Color.green);
        }
    }

    public static void DrawRectangle(RectInt _room, float _height, Color _color)
    {
        DebugExtension.DebugBounds(new Bounds(new Vector3(_room.center.x, 0, _room.center.y), 
            new Vector3(_room.width, _height, _room.height)),
            _color, 0.1f);
    }
}
