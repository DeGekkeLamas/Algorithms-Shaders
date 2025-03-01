using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    RectInt initialRoom = new(0, 0, 100, 100);
    //RectInt initialRoom = new(30, 30, 30, 30);
    public float height = 5;
    public List<RectInt> rooms = new List<RectInt>(1);
    float fraction = 0.5f;
    public int splitDepth = 1;
    public int splitOffset = 1;

    private void Awake() => StartCoroutine(GenerateRooms());
    IEnumerator GenerateRooms()
    {
        RandomizeFraction();
        rooms.Add(initialRoom);
        //rooms.Add(SplitHorizontal(initialRoom, fraction));

        yield return new WaitForSeconds(0.5f);
        //bool canSplit = false;
        for (int i = 0; i < splitDepth; i++)
        {
            int _length = rooms.Count;
            for (int j = 0; j < _length; j++)
            {
                RandomizeFraction();
                if (rooms[j].width > rooms[j].height)
                {
                    rooms.Add(SplitVertical(rooms[j], fraction).Item1);
                    rooms[j] = SplitVertical(rooms[j], fraction).Item2;
                }
                else
                {
                    rooms.Add(SplitHorizontal(rooms[j], fraction).Item1);
                    rooms[j] = SplitHorizontal(rooms[j], fraction).Item2;
                }
                yield return new WaitForSeconds(0.5f);
            }
            Debug.Log($"{i} cycles done, from {this}");
        }
    }
    void RandomizeFraction() => fraction = (float)Random.Range(25, 76) / 100f;

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
        foreach (RectInt room in rooms) { DrawRectangle(new(room.xMin + splitOffset, room.yMin + splitOffset, 
            room.width - splitOffset, room.height - splitOffset), 
            height, Color.magenta); }
    }

    public static void DrawRectangle(RectInt _room, float _height, Color _color)
    {
        DebugExtension.DebugBounds(new Bounds(new Vector3(_room.center.x, 0, _room.center.y), 
            new Vector3(_room.width, _height, _room.height)),
            _color, 1);
    }
}
