using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    RectInt initialRoom = new(0, 0, 100, 100);
    //RectInt initialRoom = new(30, 30, 30, 30);
    public float height = 5;

    public List<RectInt> rooms = new(1);
    public List<RectInt> doors = new(1);

    float fraction = 0.5f;
    public int splitDepth = 1;
    public int splitOffset = 1;

    private void Awake() => StartCoroutine(GenerateRooms());
    IEnumerator GenerateRooms()
    {
        rooms.Add(initialRoom);

        yield return new WaitForSeconds(0.1f);

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

        Debug.Log($"Generated all rooms, from {this}");

        for (int i = 0;i < rooms.Count; i++)
        {
            for (int j = i; j < rooms.Count; j++)
            {
                if (i != j && AlgorithmsUtils.Intersects(rooms[i], rooms[j]))
                {
                    //Debug.Log($"Intersection between rooms {i} and {j}, from {this}");

                    var _newDoor = AlgorithmsUtils.Intersect(rooms[i], rooms[j]);
                    doors.Add(_newDoor);
                    if (_newDoor.width == 0 && _newDoor.height == 0) doors.Remove(_newDoor);

                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        Debug.Log($"Generated all doors, from {this}");
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
        //rooms.Remove(_origianalRoom);
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
        //rooms.Remove(_origianalRoom);
        return (_roomA, _roomB);
    }

    void Update()
    {
        DrawRectangle(initialRoom, height + 1, Color.red);

        foreach (RectInt _room in rooms) { DrawRectangle(new(_room.xMin + splitOffset, _room.yMin + splitOffset, 
            _room.width - splitOffset, _room.height - splitOffset), 
            height, Color.magenta);
        }

        foreach (RectInt _door in doors)
        {
            DrawRectangle(new(_door.xMin + splitOffset, _door.yMin + splitOffset,
            _door.width - splitOffset, _door.height - splitOffset),
            height, Color.blue);
        }
    }

    public static void DrawRectangle(RectInt _room, float _height, Color _color)
    {
        DebugExtension.DebugBounds(new Bounds(new Vector3(_room.center.x, 0, _room.center.y), 
            new Vector3(_room.width, _height, _room.height)),
            _color, 0.1f);
    }
}
