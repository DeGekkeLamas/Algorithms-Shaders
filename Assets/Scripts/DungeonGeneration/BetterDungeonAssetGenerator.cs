using System.Collections;
using System.Text;
using UnityEngine;

public class BetterDungeonAssetGenerator : MonoBehaviour
{
    DungeonGenerator d;
    int[,] tilemap;
    private void Awake() => d = this.GetComponent<DungeonGenerator>();
    public GameObject[] marchingSquareAssets = new GameObject[16];

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
        //PrintTileMap();
        if (tilemapDebugger != null)
        {
            tilemapDebugger.DrawMap(tilemap);
        }

        yield return new();
        d.coroutineIsDone = true;
    }
    public IEnumerator GenerateWalls()
    {
        GameObject wallContainer = new("WallContainer");
        wallContainer.transform.parent = d.assetContainer.transform;
        int rows = tilemap.GetLength(0);
        int columns = tilemap.GetLength(1);
        for (int y = 0; y < columns-1; y++)
        {
            for (int x = 0; x < rows-1; x++)
            {
                int[] localGroup = new int[4];
                localGroup[0] = tilemap[Mathf.Clamp(x, 0, rows - 1), Mathf.Clamp(y, 0, columns - 1)];
                localGroup[1] = tilemap[Mathf.Clamp(x + 1, 0, rows - 1), Mathf.Clamp(y, 0, columns - 1)];
                localGroup[2] = tilemap[Mathf.Clamp(x, 0, rows - 1), Mathf.Clamp(y + 1, 0, columns - 1)];
                localGroup[3] = tilemap[Mathf.Clamp(x + 1, 0, rows - 1), Mathf.Clamp(y + 1, 0, columns - 1)];

                int tileBinary = localGroup[0] * 1 + localGroup[1] * 2 + localGroup[2] * 4 + localGroup[3] * 8;
                if (marchingSquareAssets[tileBinary] != null)
                {
                    Instantiate(marchingSquareAssets[tileBinary], new(y, 0, x), Quaternion.identity, wallContainer.transform);
                    //yield return new WaitForSeconds(d.generationInterval);
                }

            }
        }
        yield return new();
        d.coroutineIsDone = true;
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
