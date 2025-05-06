using System.Collections;
using System.Text;
using UnityEngine;

public class BetterDungeonAssetGenerator : MonoBehaviour
{
    DungeonGenerator d;
    private void Awake() => d = this.GetComponent<DungeonGenerator>();
    int[,] tilemap;

    public IEnumerator GenerateTileMap()
    {
        RectInt bounds = d.initialRoom;
        RectInt offset = new(-d.initialRoom.xMin, -d.initialRoom.yMin, 0, 0);
        tilemap = new int[bounds.height, bounds.width];
        // fill tilemap
        foreach (RectInt room in d.rooms)
        {
            AlgorithmsUtils.FillRectangleOutline(tilemap, RectIntAddition(room, offset), 1);
        }
        foreach (RectInt door in d.doors)
        {
            AlgorithmsUtils.FillRectangle(tilemap, RectIntAddition(door, offset), 0);
        }

        // Draw tilemap onto plane for debugging
        DrawTilemap tilemapDebugger = FindAnyObjectByType<DrawTilemap>();
        PrintTileMap();
        if (tilemapDebugger != null)
        {
            tilemapDebugger.DrawMap(tilemap);
        }

        yield return new();
    }

    static RectInt RectIntAddition(RectInt A, RectInt B)
    {
        return new(A.xMin + B.xMin, A.yMin + B.yMin, A.width + B.width, A.height + B.height);
    }


    public string ToString(bool flip)
    {
        if (tilemap == null) return "Tile map not generated yet.";

        int rows = tilemap.GetLength(0);
        int cols = tilemap.GetLength(1);

        var sb = new StringBuilder();

        int start = flip ? rows - 1 : 0;
        int end = flip ? -1 : rows;
        int step = flip ? -1 : 1;

        for (int i = start; i != end; i += step)
        {
            for (int j = 0; j < cols; j++)
            {
                sb.Append((tilemap[i, j] == 0 ? '0' : '#')); //Replaces 1 with '#' making it easier to visualize
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }
    public void PrintTileMap()
    {
        Debug.Log(ToString(true));
    }
}
