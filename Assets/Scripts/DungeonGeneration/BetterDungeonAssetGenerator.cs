using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.AI.Navigation;
using UnityEngine;

public class BetterDungeonAssetGenerator : MonoBehaviour
{
    DungeonGenerator d;
    int[,] tilemap;
    [Header("Assets")]
    public GameObject[] marchingSquareAssets = new GameObject[16];
    public GameObject floor;
    public GameObject player;
    [Header("Coroutine speed")]
    public int assetsPerDelayWalls = 20;
    public int assetsPerDelayFloor = 50;
    private int assetsDone;

    private void Awake() => d = this.GetComponent<DungeonGenerator>();

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

        // Add empty edges at the outside so walls generate on the dungeon exterior
        int[,] newTilemap = new int[bounds.height + 2, bounds.width + 2];
        for (int y = 0; y < newTilemap.GetLength(0); y++)
        {
            for (int x = 0; x < newTilemap.GetLength(1); x++)
            {
                if (y == 0 || x == 0 || y == newTilemap.GetLength(0)-1 || x == newTilemap.GetLength(1)-1)
                {
                    newTilemap[y,x] = 0;
                }
                else
                {
                    newTilemap[y, x] = tilemap[y-1,x-1];
                }
            }
        }
        tilemap = newTilemap;

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
                    assetsDone++;
                    if (assetsDone >= assetsPerDelayWalls)
                    {
                        yield return new WaitForSeconds(d.generationInterval);
                        assetsDone = 0;
                    }
                }

            }
        }
        yield return new();
        d.coroutineIsDone = true;
    }
    public IEnumerator GenerateFloor(Vector2Int start)
    {
        GameObject floorContainer = new("FloorContainer");
        floorContainer.transform.parent = d.assetContainer.transform;
        List<Vector2Int> visitedList = new();
        Queue<Vector2Int> queue = new();
        //queue.Initialize();
        queue.Enqueue(start);
        int floorSize = (int)floor.transform.lossyScale.x;

        while (queue.Count > 0)
        {
            Vector2Int point = queue.Dequeue();
            visitedList.Add(point);
            Vector2Int[] pointsToAdd = new Vector2Int[4]
            {
                point + new Vector2Int(floorSize, 0),
                point + new Vector2Int(-floorSize, 0),
                point + new Vector2Int(0, floorSize),
                point + new Vector2Int(0, -floorSize)
            };
            foreach (Vector2Int pointToFill in pointsToAdd)
            {
                Vector2Int realPoint = pointToFill / floorSize * floorSize;
                if (!visitedList.Contains(realPoint) && 
                    !queue.Contains(realPoint) && 
                    (tilemap[
                        Mathf.Clamp(realPoint.y, 1, tilemap.GetLength(0)-2), 
                        Mathf.Clamp(realPoint.x, 1, tilemap.GetLength(1)-2)] == 0))
                {
                    queue.Enqueue(realPoint);
                }
            }

            Instantiate(floor, new(point.x - .5f, 0, point.y - .5f), Quaternion.identity, floorContainer.transform);
            assetsDone++;
            if (assetsDone >= assetsPerDelayFloor)
            {
                yield return new WaitForSeconds(d.generationInterval);
                assetsDone = 0;
            }
        }
        yield return new();
        d.coroutineIsDone = true;
    }
    public IEnumerator SpawnPlayer()
    {
        d.navMeshSurface.BuildNavMesh();
        Destroy(Camera.main.gameObject);
        Instantiate(player, new(d.GetOriginRoom().center.x, 0, d.GetOriginRoom().center.y), Quaternion.identity);

        yield return new();
        d.coroutineIsDone = true;
    }
    public struct HashQueue<T>
    {
        HashSet<T> hashset;
        public int Count;
        public void Initialize()
        {
            hashset = new();
            Count = hashset.Count;
        }
        public void Enqueue(T toAdd) 
        { 
            hashset.Add(toAdd);
            Count = hashset.Count;
        }
        public T Dequeue()
        {
            T removed = hashset.ElementAt(0);
            hashset.Remove(removed);
            Count = hashset.Count;
            return removed;
        }
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
