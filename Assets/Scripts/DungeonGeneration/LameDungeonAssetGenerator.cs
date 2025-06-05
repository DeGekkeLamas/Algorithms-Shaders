using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LameDungeonAssetGenerator : MonoBehaviour
{
    DungeonGenerator d;
    [Header("Assets")]
    public GameObject wall;
    public GameObject floor;
    public GameObject player;
    [Header("Coroutine speed")]
    public int assetsPerDelayWalls = 40;
    public int assetsPerDelayFloors = 5;
    private int assetsDone;

    private void Awake() => d = this.GetComponent<DungeonGenerator>();

    public IEnumerator GenerateWalls()
    {
        GameObject wallContainer = new GameObject("WallContainer");

        // Generate walls
        Dictionary<Vector3, GameObject> spawnedWalls = new();
        float wallHeight = .5f * wall.transform.localScale.y;
        for (int i = 0; i < d.rooms.Count; i++)
        {
            GameObject roomWallContainer = new($"Room{i}WallContainer");
            roomWallContainer.transform.parent = wallContainer.transform;

            RectInt room = d.rooms[i];
            // Over width
            for (int j = 0; j < room.width; j++)
            {
                GameObject wallA = Instantiate(wall, new Vector3(room.xMin + j, wallHeight, room.yMin)
                    + new Vector3(.5f, 0, .5f), Quaternion.identity, roomWallContainer.transform);
                GameObject wallB = Instantiate(wall, new Vector3(room.xMin + j, wallHeight, room.yMax)
                    + new Vector3(.5f, 0, -.5f), Quaternion.identity, roomWallContainer.transform);
                // Destroy duplicates
                if (spawnedWalls.ContainsKey(wallA.transform.position))
                    Destroy(wallA);
                else spawnedWalls[wallA.transform.position] = wallA;
                if (spawnedWalls.ContainsKey(wallB.transform.position))
                    Destroy(wallB);
                else spawnedWalls[wallB.transform.position] = wallB;

                assetsDone++;
                if (assetsDone >= assetsPerDelayWalls)
                {
                    yield return new WaitForSeconds(d.generationInterval);
                    assetsDone = 0;
                }
            }
            // Over height
            for (int j = 0; j < room.height; j++)
            {
                GameObject wallA = Instantiate(wall, new Vector3(room.xMin, wallHeight, room.yMin + j)
                    + new Vector3(.5f, 0, .5f), Quaternion.identity, roomWallContainer.transform);
                GameObject wallB = Instantiate(wall, new Vector3(room.xMax, wallHeight, room.yMin + j)
                    - new Vector3(.5f, 0, -.5f), Quaternion.identity, roomWallContainer.transform);
                // Destroy duplicates
                if (spawnedWalls.ContainsKey(wallA.transform.position))
                    Destroy(wallA);
                else spawnedWalls[wallA.transform.position] = wallA;
                if (spawnedWalls.ContainsKey(wallB.transform.position))
                    Destroy(wallB);
                else spawnedWalls[wallB.transform.position] = wallB;

                assetsDone++;
                if (assetsDone >= assetsPerDelayWalls)
                {
                    yield return new WaitForSeconds(d.generationInterval);
                    assetsDone = 0;
                }
            }
        }
        // Carve out doors
        foreach (RectInt door in d.doors)
        {
            Vector3 doorPos = new(door.center.x, wallHeight, door.center.y);
            Destroy(spawnedWalls[doorPos]);
            yield return new WaitForSeconds(d.generationInterval);
        }

        yield return new();
        d.coroutineIsDone = true;
    }

    public IEnumerator GenerateFloor(GameObject floor)
    { 
        assetsDone = 0;
        // room floors
        GameObject floorContainer = new GameObject("FloorContainer");
        foreach (RectInt room in d.rooms)
        {
            GameObject _floor = Instantiate(floor, new Vector3(room.center.x, 0, room.center.y),
                Quaternion.identity, floorContainer.transform);
            _floor.transform.localScale = new Vector3(room.width - 2, 1, room.height - 2);
            DungeonGenerator.DrawRectangle(room, .2f, Color.white, .5f);
            yield return new WaitForSeconds(d.generationInterval);
        }
        // door flooors
        foreach (RectInt door in d.doors)
        {
            GameObject doorFloor = Instantiate(floor, new Vector3(door.center.x, 0, door.center.y),
            Quaternion.identity, floorContainer.transform);
            doorFloor.transform.localScale = new Vector3(door.width, 1, door.height);
            DungeonGenerator.DrawRectangle(door, .2f, Color.white, .5f);
            
            assetsDone++;
            if (assetsDone >= assetsPerDelayWalls)
            {
                yield return new WaitForSeconds(d.generationInterval);
                assetsDone = 0;
            }
        }

        yield return new();
        d.coroutineIsDone = true;
    }
}
