using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeonGeneration
{
    /// <summary>
    /// Script used for cool dungeon generation
    /// </summary>
    public class BetterDungeonAssetGenerator : MonoBehaviour
    {
        DungeonGenerator d;
        [Header("Assets")]
        public GameObject[] marchingSquareAssets = new GameObject[16];
        public MeshRenderer floor;
        public GameObject player;
        public GameObject floorCollider;
        [Header("Coroutine speed")]
        public int assetsPerDelayWalls = 20;
        public int assetsPerDelayFloor = 50;
        private int _assetsDone;

        private void OnValidate()
        {
            if (marchingSquareAssets.Length != 16)
            {
                Debug.LogWarning("Don't change the 'marchingSquareAssets' field's array size!");
                Array.Resize(ref marchingSquareAssets, 16);
            }
        }
        private void Awake() => d = this.GetComponent<DungeonGenerator>();

        /// <summary>
        /// Creates tilemap
        /// </summary>
        public IEnumerator GenerateTileMap()
        {
            RectInt bounds = d.initialRoom;
            RectInt offset = new(-d.initialRoom.xMin, -d.initialRoom.yMin, 0, 0);
            d.tilemap = new int[bounds.height, bounds.width];
            // fill tilemap
            foreach (RectInt room in d.rooms)
            {
                AlgorithmsUtils.FillRectangleOutline(d.tilemap, RectIntAddition(room, offset), 1);
            }
            foreach (RectInt door in d.doors)
            {
                AlgorithmsUtils.FillRectangle(d.tilemap, RectIntAddition(door, offset), 0);
            }

            // Add empty edges at the outside so walls generate on the dungeon exterior
            int[,] newTilemap = new int[bounds.height + 2, bounds.width + 2];
            for (int y = 1; y < newTilemap.GetLength(0); y++)
            {
                for (int x = 1; x < newTilemap.GetLength(1); x++)
                {
                    if (y == newTilemap.GetLength(0) - 1 || x == newTilemap.GetLength(1) - 1)
                    {
                        newTilemap[y, x] = 0;
                    }
                    else
                    {
                        newTilemap[y, x] = d.tilemap[y - 1, x - 1];
                    }
                }
            }
            d.tilemap = newTilemap;

            // Draw tilemap onto plane for debugging
            //PrintTileMap();
            d.DrawTilemap();

            Debug.Log("Generated tilemap");
            yield return new();
        }

        /// <summary>
        /// Generate walls with marching squares
        /// </summary>
        public IEnumerator GenerateWalls()
        {
            GameObject wallContainer = new("WallContainer");
            wallContainer.transform.parent = d.assetContainer.transform;
            int rows = d.tilemap.GetLength(0);
            int columns = d.tilemap.GetLength(1);
            for (int y = 0; y < columns - 1; y++)
            {
                for (int x = 0; x < rows - 1; x++)
                {
                    int[] localGroup = new int[4];
                    localGroup[0] = d.tilemap[Mathf.Clamp(x, 0, rows - 1), Mathf.Clamp(y, 0, columns - 1)];
                    localGroup[1] = d.tilemap[Mathf.Clamp(x + 1, 0, rows - 1), Mathf.Clamp(y, 0, columns - 1)];
                    localGroup[2] = d.tilemap[Mathf.Clamp(x, 0, rows - 1), Mathf.Clamp(y + 1, 0, columns - 1)];
                    localGroup[3] = d.tilemap[Mathf.Clamp(x + 1, 0, rows - 1), Mathf.Clamp(y + 1, 0, columns - 1)];

                    int tileBinary = localGroup[0] * 1 + localGroup[1] * 2 + localGroup[2] * 4 + localGroup[3] * 8;
                    if (marchingSquareAssets[tileBinary] != null)
                    {
                        Instantiate(marchingSquareAssets[tileBinary], new(y, 0, x), Quaternion.identity, wallContainer.transform);
                        _assetsDone++;
                        if (_assetsDone >= assetsPerDelayWalls)
                        {
                            yield return new WaitForSeconds(d.generationInterval);
                            _assetsDone = 0;
                        }
                    }

                }
            }
            Debug.Log("Placed all walls");
            yield return new();
        }
        /// <summary>
        /// Generates floor with floodfill
        /// </summary>
        public IEnumerator GenerateFloor(Vector2Int start)
        {
            GameObject floorContainer = new("FloorContainer");
            floorContainer.transform.parent = d.assetContainer.transform;
            List<Vector2Int> visitedList = new();
            HashQueue<Vector2Int> queue = new();
            queue.Initialize();
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
                        (d.tilemap[
                            Mathf.Clamp(realPoint.y, 1, d.tilemap.GetLength(0) - 2),
                            Mathf.Clamp(realPoint.x, 1, d.tilemap.GetLength(1) - 2)] == 0))
                    {
                        queue.Enqueue(realPoint);
                    }
                }

                MeshRenderer floorpiece = Instantiate(floor, new(point.x - .5f, 0, point.y - .5f), Quaternion.identity, floorContainer.transform);
                floorpiece.material = d.Rda.floorMat;
                _assetsDone++;
                if (_assetsDone >= assetsPerDelayFloor)
                {
                    yield return new WaitForSeconds(d.generationInterval);
                    _assetsDone = 0;
                }
            }
            Debug.Log("Generated floor");
            yield return new();
        }
        /// <summary>
        /// Spawns player and bakes navmesh
        /// </summary>
        public IEnumerator SpawnPlayer()
        {
            d.navMeshSurface.BuildNavMesh();
            Destroy(Camera.main.gameObject);
            Instantiate(player, new(d.originRoom.center.x, 0, d.originRoom.center.y), Quaternion.identity);

            Debug.Log("Spawned player and generated navmesh");
            yield return new();
        }
        static RectInt RectIntAddition(RectInt A, RectInt B)
        {
            return new(A.xMin + B.xMin, A.yMin + B.yMin, A.width + B.width, A.height + B.height);
        }

        public string ToString(bool flip)
        {
            if (d.tilemap == null) return "Tile map not generated yet.";

            int rows = d.tilemap.GetLength(0);
            int cols = d.tilemap.GetLength(1);

            var sb = new StringBuilder();

            int start = flip ? rows - 1 : 0;
            int end = flip ? -1 : rows;
            int step = flip ? -1 : 1;

            for (int i = start; i != end; i += step)
            {
                for (int j = 0; j < cols; j++)
                {
                    sb.Append((d.tilemap[i, j] == 0 ? '0' : '#')); //Replaces 1 with '#' making it easier to visualize
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
}