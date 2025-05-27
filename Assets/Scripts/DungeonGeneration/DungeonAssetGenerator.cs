using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using static DungeonGenerator;

public class DungeonAssetGenerator : MonoBehaviour
{
    public GameObject wall;
    public GameObject wallBound;
    public GameObject floor;
    public GameObject player;
    public GameObject enemy;
    public EnemyProbabilities[] enemiesPerRoom;
    public float wallHeight = 5;
    public float wallBoundHeight = 1;
    public float wallBoundThickness = 1.25f;
    public RoomSpecificAssets rsa;
    RoomType[] roomTypes;
    List<GameObject> wallsGenerated = new();

    DungeonGenerator d;

    private void OnValidate()
    {
        rsa.bakeryTotalItemChance = GetTotalItemProbability(rsa.bakeryItemSpawns);
        rsa.breakTotalItemChance = GetTotalItemProbability(rsa.breakItemSpawns);
        rsa.kitchenTotalItemChance = GetTotalItemProbability(rsa.kitchenItemSpawns);
        rsa.storageTotalItemChance = GetTotalItemProbability(rsa.storageItemSpawns);
        rsa.seatingTotalItemChance = GetTotalItemProbability(rsa.seatingItemSpawns);
    }

    private void Awake() => d = this.GetComponent<DungeonGenerator>();
    public IEnumerator AssignRoomTypes()
    {
        roomTypes = new RoomType[d.rooms.Count];
        int bakeries = 0;
        int kitchens = 0;
        int seatings = 0;
        int storages = 0;
        for (int i = 0; i < d.rooms.Count; i++)
        {
            if (d.rooms[i] == d.GetOriginRoom())
            {
                roomTypes[i] = RoomType.breakRoom;
                continue;
            }
            int lowestQTY = Mathf.Min(bakeries, kitchens, seatings, storages);
            if (lowestQTY == bakeries)
            {
                roomTypes[i] = RoomType.bakery;
                bakeries++;
            }
            else if (lowestQTY == kitchens)
            {
                roomTypes[i] = RoomType.kitchen;
                kitchens++;
            }
            else if (lowestQTY == seatings)
            {
                roomTypes[i] = RoomType.seating;
                seatings++;
            }
            else if (lowestQTY == storages)
            {
                roomTypes[i] = RoomType.storage;
                storages++;
            }
        }
        yield return new();
        d.coroutineIsDone = true;
    }
    public IEnumerator GenerateInitialWalls()
    {
        // Generate walls
        GameObject wallContainer = new("WallContainer");
        wallContainer.transform.parent = d.assetContainer.transform;

        for (int i = 0; i < d.rooms.Count; i++)
        {
            GameObject roomWallContainer = new($"Room{i}WallContainer");
            roomWallContainer.transform.parent = wallContainer.transform;

            RectInt room = d.rooms[i];
            Vector3 _center = new(room.center.x, 0, room.center.y);

            GameObject wallXPlus = Instantiate(wall, _center + new Vector3(room.width * 0.5f, 0, 0),
                Quaternion.identity, roomWallContainer.transform);
            GameObject wallXMin = Instantiate(wall, _center + new Vector3(-room.width * 0.5f, 0, 0),
                Quaternion.identity, roomWallContainer.transform);
            GameObject wallYPlus = Instantiate(wall, _center + new Vector3(0, 0, room.height * 0.5f),
                Quaternion.identity, roomWallContainer.transform);
            GameObject wallYMin = Instantiate(wall, _center + new Vector3(0, 0, -room.height * 0.5f),
                Quaternion.identity, roomWallContainer.transform);

            wallXPlus.name = "wallXPlus";
            wallXMin.name = "wallXMin";
            wallYPlus.name = "wallYPlus";
            wallYMin.name = "wallYMin";

            wallXPlus.transform.localScale = new(1, wallHeight, room.height);
            wallXMin.transform.localScale = new(1, wallHeight, room.height);
            wallYPlus.transform.localScale = new(room.width, wallHeight, 1);
            wallYMin.transform.localScale = new(room.width, wallHeight, 1);

            WallGenerator wallXPlusGen = wallXPlus.AddComponent<WallGenerator>();
            wallXPlusGen.zMin = true;
            WallGenerator wallXMinGen = wallXMin.AddComponent<WallGenerator>();
            wallXMinGen.zPlus = true;
            if (room.xMin == d.initialRoom.xMin) wallXMinGen.zMin = true;
            WallGenerator wallYPlusGen = wallYPlus.AddComponent<WallGenerator>();
            wallYPlusGen.xPlus = true;
            WallGenerator wallYMinGen = wallYMin.AddComponent<WallGenerator>();
            wallYMinGen.xMin = true;
            if (room.yMin == d.initialRoom.yMin) wallYMinGen.xPlus = true;

            wallsGenerated.Add(wallXPlus);
            wallsGenerated.Add(wallYPlus);
            wallsGenerated.Add(wallXMin);
            wallsGenerated.Add(wallYMin);

            Material materialToAssign;
            if (room == d.GetOriginRoom())
                materialToAssign = rsa.breakRoomWall;
            else switch (roomTypes[i])
                {
                    case RoomType.bakery:
                        materialToAssign = rsa.bakeryWall;
                        break;
                    case RoomType.breakRoom:
                        materialToAssign = rsa.breakRoomWall;
                        break;
                    case RoomType.kitchen:
                        materialToAssign = rsa.kitchenWall;
                        break;
                    case RoomType.seating:
                        materialToAssign = rsa.seatingWall;
                        break;
                    case RoomType.storage:
                        materialToAssign = rsa.storageWall;
                        break;
                    default:
                        materialToAssign = rsa.kitchenWall;
                        break;
                }

            wallXPlus.GetComponent<MeshRenderer>().sharedMaterial = materialToAssign;
            wallYPlus.GetComponent<MeshRenderer>().sharedMaterial = materialToAssign;
            wallXMin.GetComponent<MeshRenderer>().sharedMaterial = materialToAssign;
            wallYMin.GetComponent<MeshRenderer>().sharedMaterial = materialToAssign;

            yield return new WaitForSeconds(d.generationInterval);
        }
        d.coroutineIsDone = true;
        yield return new();
    }
    public IEnumerator ModifyWalls()
    {
        // Check for door intersections
        int wallsQTY = wallsGenerated.Count;
        GameObject doorBoundsContainer = new("DoorBounds");
        doorBoundsContainer.transform.parent = d.assetContainer.transform;
        for (int i = 0; i < wallsQTY; i++)
        {
            RectInt wallRect = new(
                (int)(wallsGenerated[i].transform.position.x - .5f * wallsGenerated[i].transform.localScale.x),
                (int)(wallsGenerated[i].transform.position.z - .5f * wallsGenerated[i].transform.localScale.z),
                (int)wallsGenerated[i].transform.localScale.x,
                (int)wallsGenerated[i].transform.localScale.z
                );
            for (int j = 0; j < d.doors.Count; j++)
            {
                if (AlgorithmsUtils.Intersects(d.doors[j], wallRect))
                {
                    GameObject wallDupe = Instantiate(wallsGenerated[i], wallsGenerated[i].transform.position,
                        Quaternion.identity, wallsGenerated[i].transform.parent);
                    wallDupe.name = "WallB";
                    GameObject doorBoundA;
                    GameObject doorBoundB;

                    RectInt newWallA;
                    RectInt newWallB;
                    RectInt intersectDoor = d.doors[j];

                    bool intersectOverHeight = wallRect.width < wallRect.height;
                    if (intersectOverHeight)
                    {
                        newWallA = new(
                            wallRect.xMin,
                            wallRect.yMin,
                            wallRect.width,
                            intersectDoor.yMin - wallRect.yMin
                            );
                        newWallB = new(
                            wallRect.xMin,
                            intersectDoor.yMin + intersectDoor.height,
                            wallRect.width,
                            wallRect.height - newWallA.height - intersectDoor.height
                            );
                    }
                    else
                    {
                        newWallA = new(
                            wallRect.xMin,
                            wallRect.yMin,
                            intersectDoor.xMin - wallRect.xMin,
                            wallRect.height
                            );
                        newWallB = new(
                            intersectDoor.xMin + intersectDoor.width,
                            wallRect.yMin,
                            wallRect.width - newWallA.width - intersectDoor.width,
                            wallRect.height
                            );
                    }
                    yield return new WaitForSeconds(d.generationInterval);

                    wallsGenerated[i].transform.position = new Vector3(newWallA.center.x, 0, newWallA.center.y);
                    wallDupe.transform.position = new Vector3(newWallB.center.x, 0, newWallB.center.y);
                    wallsGenerated[i].transform.localScale = new Vector3(newWallA.width, wallHeight, newWallA.height);
                    wallDupe.transform.localScale = new Vector3(newWallB.width, wallHeight, newWallB.height);
                    wallsGenerated.Add(wallDupe);

                    if (intersectOverHeight)
                    {
                        doorBoundA = Instantiate(wallBound, new Vector3(
                            wallDupe.transform.position.x, wallDupe.transform.position.y, intersectDoor.yMin
                            ), Quaternion.identity, doorBoundsContainer.transform);
                        doorBoundB = Instantiate(wallBound, new Vector3(
                            wallDupe.transform.position.x, wallDupe.transform.position.y, intersectDoor.yMax
                            ), Quaternion.identity, doorBoundsContainer.transform);
                    }
                    else
                    {
                        doorBoundA = Instantiate(wallBound, new Vector3(
                            intersectDoor.xMin, wallDupe.transform.position.y, wallDupe.transform.position.z
                            ), Quaternion.identity, doorBoundsContainer.transform);
                        doorBoundB = Instantiate(wallBound, new Vector3(
                            intersectDoor.xMax, wallDupe.transform.position.y, wallDupe.transform.position.z
                            ), Quaternion.identity, doorBoundsContainer.transform);
                    }
                    doorBoundA.transform.localScale = new Vector3(wallBoundThickness,
                        wallHeight + wallBoundHeight, wallBoundThickness);
                    doorBoundB.transform.localScale = new Vector3(wallBoundThickness,
                        wallHeight + wallBoundHeight, wallBoundThickness);
                }
            }
        }

        // Generate bounds on top of and below walls
        GameObject wallBoundContainer = new("WallBoundContainer");
        wallBoundContainer.transform.parent = d.assetContainer.transform;
        foreach (GameObject wall in wallsGenerated)
        {
            GameObject wallBoundTop = Instantiate(wallBound,
                new Vector3(wall.transform.position.x,
        wall.transform.position.y + 0.5f * wall.transform.localScale.y, wall.transform.position.z),
                Quaternion.identity, wallBoundContainer.transform);

            Vector3 wallScale = wall.transform.localScale;
            wallBoundTop.transform.localScale = new Vector3(
                wallScale.x + (wallScale.x < wallScale.z ? wallBoundThickness - 1 : 0),
                wallBoundHeight,
                wallScale.z + (wallScale.x > wallScale.z ? wallBoundThickness - 1 : 0));

            GameObject wallBoundBottom = Instantiate(wallBound,
                new Vector3(wall.transform.position.x,
        wall.transform.position.y - 0.5f * wall.transform.localScale.y, wall.transform.position.z),
                Quaternion.identity, wallBoundContainer.transform);

            wallBoundBottom.transform.localScale = wallBoundTop.transform.localScale;
            yield return new WaitForSeconds(d.generationInterval);
        }
        // Generate bounds in map corners
        GameObject cornerOrigin = Instantiate(wallBound, new(d.initialRoom.xMin, 0, d.initialRoom.yMin), Quaternion.identity, wallBoundContainer.transform);
        GameObject cornerOriginPlusX = Instantiate(wallBound, new(d.initialRoom.xMax, 0, d.initialRoom.yMin), Quaternion.identity, wallBoundContainer.transform);
        GameObject cornerOriginPlusY = Instantiate(wallBound, new(d.initialRoom.xMin, 0, d.initialRoom.yMax), Quaternion.identity, wallBoundContainer.transform);
        GameObject cornerOriginPlusXY = Instantiate(wallBound, new(d.initialRoom.xMax, 0, d.initialRoom.yMax), Quaternion.identity, wallBoundContainer.transform);
        cornerOrigin.transform.localScale = new(wallBoundThickness, wallHeight + wallBoundHeight, wallBoundThickness);
        cornerOriginPlusX.transform.localScale = new(wallBoundThickness, wallHeight + wallBoundHeight, wallBoundThickness);
        cornerOriginPlusY.transform.localScale = new(wallBoundThickness, wallHeight + wallBoundHeight, wallBoundThickness);
        cornerOriginPlusXY.transform.localScale = new(wallBoundThickness, wallHeight + wallBoundHeight, wallBoundThickness);

        d.coroutineIsDone = true;
    }
    public IEnumerator GenerateFloor()
    {
        // Generate floor
        GameObject floorContainer = new GameObject("FloorContainer");
        floorContainer.transform.parent = d.assetContainer.transform;
        for (int i = 0; i < d.rooms.Count; i++)
        {
            RectInt room = d.rooms[i];
            GameObject _floor = Instantiate(floor, new Vector3(room.center.x, -wallHeight * .5f, room.center.y),
                Quaternion.identity, floorContainer.transform);
            _floor.transform.localScale = new Vector3(room.width, 1, room.height) / 10;
            yield return new WaitForSeconds(d.generationInterval);

            Material materialToAssign;
            if (room == d.GetOriginRoom())
                materialToAssign = rsa.breakRoomFloor;
            else switch (roomTypes[i])
                {
                    case RoomType.bakery:
                        materialToAssign = rsa.bakeryFloor;
                        break;
                    case RoomType.breakRoom:
                        materialToAssign = rsa.breakRoomFloor;
                        break;
                    case RoomType.kitchen:
                        materialToAssign = rsa.kitchenFloor;
                        break;
                    case RoomType.seating:
                        materialToAssign = rsa.seatingFloor;
                        break;
                    case RoomType.storage:
                        materialToAssign = rsa.storageFloor;
                        break;
                    default:
                        materialToAssign = rsa.kitchenFloor;
                        break;
                }
            _floor.GetComponent<MeshRenderer>().sharedMaterial = materialToAssign;
        }

        d.coroutineIsDone = true;
        yield return new();

    }
    public IEnumerator Brickify()
    {
        // Brickifies generated walls
        WallGenerator[] walls = GameObject.FindObjectsByType<WallGenerator>(FindObjectsSortMode.None);
        foreach (WallGenerator wall in walls)
        {
            wall.GenerateWalls();
        }

        d.coroutineIsDone = true;
        yield return new();
    }
    public IEnumerator SpawnObjects()
    {

        // Place objects in d.rooms
        GameObject roomAssetContainer = new("RoomAssetContainer");
        roomAssetContainer.transform.parent = d.assetContainer.transform;
        GameObject itemSpawnsContainer = new("ItemSpawnsContainer");
        itemSpawnsContainer.transform.parent = roomAssetContainer.transform;

        GameObject counterContainer = new("CounterContainer");
        counterContainer.transform.parent = roomAssetContainer.transform;
        for (int i = 0; i < d.rooms.Count; i++)
        {
            // Place assets based on room type
            switch (roomTypes[i])
            {
                case RoomType.bakery:
                    break;
                case RoomType.kitchen:
                    RectInt room = d.rooms[i];
                    // Kitchen island
                    Vector2 offset = new(-.5f * rsa.counterLength, -0.5f);
                    bool roomvertical = room.height > room.width;
                    float counterSize = rsa.counter.transform.lossyScale.x;

                    if (roomvertical) offset = new(offset.y, offset.x);
                    for (int j = 0; j < 2; j++)
                    {
                        for (int k = 0; k < rsa.counterLength; k++)
                        {
                            Transform counter = Instantiate(rsa.counter, new(
                                room.center.x + offset.x,
                                -wallHeight * .5f + counterSize * .5f,
                                room.center.y + offset.y
                                ),
                                j == 1 ? Quaternion.Euler(0, !roomvertical ? 0 : 90, 0) : Quaternion.Euler(0, !roomvertical ? 180 : 270, 0),
                                counterContainer.transform).transform;
                            if (roomvertical) offset.y += counterSize;
                            else offset.x += counterSize;

                            string itemToSpawn = GetItemFromLoottable(rsa.kitchenItemSpawns);
                            if (itemToSpawn != string.Empty)
                            {
                                PickupItem itemSpawned = Instantiate(rsa.itemPickup, new(
                                    counter.position.x, wallHeight, counter.position.z
                                    ), Quaternion.identity, itemSpawnsContainer.transform);
                                itemSpawned.itemToGive = ItemPresets.presets[itemToSpawn];
                            }
                        }
                        if (roomvertical) offset = new(offset.x + counterSize, -.5f * rsa.counterLength);
                        else offset = new(-.5f * rsa.counterLength, offset.y + counterSize);
                    }

                    break;
                case RoomType.seating:
                    break;
                case RoomType.storage:
                    break;
            }
            yield return new WaitForSeconds(d.generationInterval);
        }

        // Spawn enemies
        GameObject enemyContainer = new("EnemyContainer");
        enemyContainer.transform.parent = d.assetContainer.transform;
        d.navMeshSurface.BuildNavMesh();
        foreach (RectInt room in d.rooms)
        {
            if (room == d.GetOriginRoom()) continue;
            int enemiesToSpawn = 0;
            int enemyroll = d.GetSeed().Next(0, 100);
            for (int i = enemiesPerRoom.Length - 1; i >= 0; i--)
            {
                if (enemyroll > 100 - enemiesPerRoom[i].probability)
                {
                    enemiesToSpawn = enemiesPerRoom[i].Quantity;
                    break;
                }
            }
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                Instantiate(enemy, new Vector3(room.center.x + i, -wallHeight * .5f, room.center.y + i + 3),
                    Quaternion.identity, enemyContainer.transform);
                yield return new WaitForSeconds(d.generationInterval);
            }
            yield return new();
        }

        // Spawn Player
        Destroy(Camera.main.gameObject);
        Instantiate(player, new Vector3(d.GetOriginRoom().center.x, -wallHeight * .5f, d.GetOriginRoom().center.y), Quaternion.identity);
        d.coroutineIsDone = true;
        yield return new WaitForSeconds(d.generationInterval);
    }
    string GetItemFromLoottable(ItemLootLable[] lootTable)
    {
        int probabilityPassed = lootTable[0].probability;
        int lootRoll = d.GetSeed().Next(0, 100);
        for (int i = 0; i < lootTable.Length; i++)
        {
            if (lootRoll < probabilityPassed) return lootTable[i].itemName;
            else probabilityPassed += lootTable[i].probability;
        }
        return string.Empty;
    }
    public static int GetTotalItemProbability(ItemLootLable[] lootTable)
    {
        int probability = 0;
        foreach (var loot in lootTable) probability += loot.probability;
        return probability;
    }
}

[System.Serializable]
public struct ItemLootLable
{
    public string itemName;
    public int probability;
}

[Serializable]
public struct RoomSpecificAssets
{
    public PickupItem itemPickup;
    [Header("Bakery")]
    public Material bakeryWall;
    public Material bakeryFloor;
    [Space]
    public int bakeryTotalItemChance;
    public ItemLootLable[] bakeryItemSpawns;
    [Header("Break room")]
    public Material breakRoomWall;
    public Material breakRoomFloor;
    [Space]
    public int breakTotalItemChance;
    public ItemLootLable[] breakItemSpawns;
    [Header("Kitchen")]
    public Material kitchenWall;
    public Material kitchenFloor;
    public int counterLength;
    public GameObject counter;
    public GameObject counterCornerL;
    public GameObject counterCornerR;
    [Space]
    public int kitchenTotalItemChance;
    public ItemLootLable[] kitchenItemSpawns;
    [Header("Storage")]
    public Material storageWall;
    public Material storageFloor;
    [Space]
    public int storageTotalItemChance;
    public ItemLootLable[] storageItemSpawns;
    [Header("Seating")]
    public Material seatingWall;
    public Material seatingFloor;
    [Space]
    public int seatingTotalItemChance;
    public ItemLootLable[] seatingItemSpawns;
}

[System.Serializable]
public struct EnemyProbabilities
{
    public int Quantity;
    [Range(0, 100)] public int probability;
}
public enum RoomType { bakery, breakRoom, kitchen, seating, storage };

