using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using static DungeonGenerator;

public class DungeonAssetGenerator : MonoBehaviour
{
    DungeonGenerator d;
    private void Awake() => d = this.GetComponent<DungeonGenerator>();
    public IEnumerator AssignRoomTypes()
    {
        d.roomTypes = new RoomType[d.rooms.Count];
        int bakeries = 0;
        int kitchens = 0;
        int seatings = 0;
        int storages = 0;
        for (int i = 0; i < d.rooms.Count; i++)
        {
            if (d.rooms[i] == d.originRoom)
            {
                d.roomTypes[i] = RoomType.breakRoom;
                continue;
            }
            int lowestQTY = Mathf.Min(bakeries, kitchens, seatings, storages);
            if (lowestQTY == bakeries)
            {
                d.roomTypes[i] = RoomType.bakery;
                bakeries++;
            }
            else if (lowestQTY == kitchens)
            {
                d.roomTypes[i] = RoomType.kitchen;
                kitchens++;
            }
            else if (lowestQTY == seatings)
            {
                d.roomTypes[i] = RoomType.seating;
                seatings++;
            }
            else if (lowestQTY == storages)
            {
                d.roomTypes[i] = RoomType.storage;
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

            GameObject wallXPlus = Instantiate(d.wall, _center + new Vector3(room.width * 0.5f, 0, 0),
                Quaternion.identity, roomWallContainer.transform);
            GameObject wallXMin = Instantiate(d.wall, _center + new Vector3(-room.width * 0.5f, 0, 0),
                Quaternion.identity, roomWallContainer.transform);
            GameObject wallYPlus = Instantiate(d.wall, _center + new Vector3(0, 0, room.height * 0.5f),
                Quaternion.identity, roomWallContainer.transform);
            GameObject wallYMin = Instantiate(d.wall, _center + new Vector3(0, 0, -room.height * 0.5f),
                Quaternion.identity, roomWallContainer.transform);

            wallXPlus.name = "wallXPlus";
            wallXMin.name = "wallXMin";
            wallYPlus.name = "wallYPlus";
            wallYMin.name = "wallYMin";

            wallXPlus.transform.localScale = new(1, d.wallHeight, room.height);
            wallXMin.transform.localScale = new(1, d.wallHeight, room.height);
            wallYPlus.transform.localScale = new(room.width, d.wallHeight, 1);
            wallYMin.transform.localScale = new(room.width, d.wallHeight, 1);

            WallGenerator wallXPlusGen = wallXPlus.AddComponent<WallGenerator>();
            wallXPlusGen.zMin = true;
            WallGenerator wallXMinGen = wallXMin.AddComponent<WallGenerator>();
            wallXMinGen.zPlus = true;
            if (room.xMin == 0) wallXMinGen.zMin = true;
            WallGenerator wallYPlusGen = wallYPlus.AddComponent<WallGenerator>();
            wallYPlusGen.xPlus = true;
            WallGenerator wallYMinGen = wallYMin.AddComponent<WallGenerator>();
            wallYMinGen.xMin = true;
            if (room.yMin == 0) wallYMinGen.xPlus = true;

            d.wallsGenerated.Add(wallXPlus);
            d.wallsGenerated.Add(wallYPlus);
            d.wallsGenerated.Add(wallXMin);
            d.wallsGenerated.Add(wallYMin);

            Material materialToAssign;
            if (room == d.originRoom)
                materialToAssign = d.roomSpecificAssets.breakRoomWall;
            else switch (d.roomTypes[i])
                {
                    case RoomType.bakery:
                        materialToAssign = d.roomSpecificAssets.bakeryWall;
                        break;
                    case RoomType.breakRoom:
                        materialToAssign = d.roomSpecificAssets.breakRoomWall;
                        break;
                    case RoomType.kitchen:
                        materialToAssign = d.roomSpecificAssets.kitchenWall;
                        break;
                    case RoomType.seating:
                        materialToAssign = d.roomSpecificAssets.seatingWall;
                        break;
                    case RoomType.storage:
                        materialToAssign = d.roomSpecificAssets.storageWall;
                        break;
                    default:
                        materialToAssign = d.roomSpecificAssets.kitchenWall;
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
        int wallsQTY = d.wallsGenerated.Count;
        GameObject doorBoundsContainer = new("DoorBounds");
        doorBoundsContainer.transform.parent = d.assetContainer.transform;
        for (int i = 0; i < wallsQTY; i++)
        {
            RectInt wallRect = new(
                (int)(d.wallsGenerated[i].transform.position.x - .5f * d.wallsGenerated[i].transform.localScale.x),
                (int)(d.wallsGenerated[i].transform.position.z - .5f * d.wallsGenerated[i].transform.localScale.z),
                (int)d.wallsGenerated[i].transform.localScale.x,
                (int)d.wallsGenerated[i].transform.localScale.z
                );
            for (int j = 0; j < d.doors.Count; j++)
            {
                if (AlgorithmsUtils.Intersects(d.doors[j], wallRect))
                {
                    GameObject wallDupe = Instantiate(d.wallsGenerated[i], d.wallsGenerated[i].transform.position,
                        Quaternion.identity, d.wallsGenerated[i].transform.parent);
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

                    d.wallsGenerated[i].transform.position = new Vector3(newWallA.center.x, 0, newWallA.center.y);
                    wallDupe.transform.position = new Vector3(newWallB.center.x, 0, newWallB.center.y);
                    d.wallsGenerated[i].transform.localScale = new Vector3(newWallA.width, d.wallHeight, newWallA.height);
                    wallDupe.transform.localScale = new Vector3(newWallB.width, d.wallHeight, newWallB.height);
                    d.wallsGenerated.Add(wallDupe);

                    if (intersectOverHeight)
                    {
                        doorBoundA = Instantiate(d.wallBound, new Vector3(
                            wallDupe.transform.position.x, wallDupe.transform.position.y, intersectDoor.yMin
                            ), Quaternion.identity, doorBoundsContainer.transform);
                        doorBoundB = Instantiate(d.wallBound, new Vector3(
                            wallDupe.transform.position.x, wallDupe.transform.position.y, intersectDoor.yMax
                            ), Quaternion.identity, doorBoundsContainer.transform);
                    }
                    else
                    {
                        doorBoundA = Instantiate(d.wallBound, new Vector3(
                            intersectDoor.xMin, wallDupe.transform.position.y, wallDupe.transform.position.z
                            ), Quaternion.identity, doorBoundsContainer.transform);
                        doorBoundB = Instantiate(d.wallBound, new Vector3(
                            intersectDoor.xMax, wallDupe.transform.position.y, wallDupe.transform.position.z
                            ), Quaternion.identity, doorBoundsContainer.transform);
                    }
                    doorBoundA.transform.localScale = new Vector3(d.wallBoundThickness,
                        d.wallHeight + d.wallBoundHeight, d.wallBoundThickness);
                    doorBoundB.transform.localScale = new Vector3(d.wallBoundThickness,
                        d.wallHeight + d.wallBoundHeight, d.wallBoundThickness);
                }
            }
        }

        // Generate bounds on top of and below walls
        GameObject wallBoundContainer = new("d.wallBoundContainer");
        wallBoundContainer.transform.parent = d.assetContainer.transform;
        foreach (GameObject wall in d.wallsGenerated)
        {
            GameObject wallBoundTop = Instantiate(d.wallBound,
                new Vector3(wall.transform.position.x,
        wall.transform.position.y + 0.5f * wall.transform.localScale.y, wall.transform.position.z),
                Quaternion.identity, wallBoundContainer.transform);

            Vector3 wallScale = wall.transform.localScale;
            wallBoundTop.transform.localScale = new Vector3(
                wallScale.x + (wallScale.x < wallScale.z ? d.wallBoundThickness - 1 : 0),
                d.wallBoundHeight,
                wallScale.z + (wallScale.x > wallScale.z ? d.wallBoundThickness - 1 : 0));

            GameObject wallBoundBottom = Instantiate(d.wallBound,
                new Vector3(wall.transform.position.x,
        wall.transform.position.y - 0.5f * wall.transform.localScale.y, wall.transform.position.z),
                Quaternion.identity, wallBoundContainer.transform);

            wallBoundBottom.transform.localScale = wallBoundTop.transform.localScale;
            yield return new WaitForSeconds(d.generationInterval);
        }
        // Generate bounds in map corners
        GameObject cornerOrigin = Instantiate(d.wallBound, new(d.initialRoom.xMin, 0, d.initialRoom.yMin), Quaternion.identity, wallBoundContainer.transform);
        GameObject cornerOriginPlusX = Instantiate(d.wallBound, new(d.initialRoom.xMax, 0, d.initialRoom.yMin), Quaternion.identity, wallBoundContainer.transform);
        GameObject cornerOriginPlusY = Instantiate(d.wallBound, new(d.initialRoom.xMin, 0, d.initialRoom.yMax), Quaternion.identity, wallBoundContainer.transform);
        GameObject cornerOriginPlusXY = Instantiate(d.wallBound, new(d.initialRoom.xMax, 0, d.initialRoom.yMax), Quaternion.identity, wallBoundContainer.transform);
        cornerOrigin.transform.localScale = new(d.wallBoundThickness, d.wallHeight + d.wallBoundHeight, d.wallBoundThickness);
        cornerOriginPlusX.transform.localScale = new(d.wallBoundThickness, d.wallHeight + d.wallBoundHeight, d.wallBoundThickness);
        cornerOriginPlusY.transform.localScale = new(d.wallBoundThickness, d.wallHeight + d.wallBoundHeight, d.wallBoundThickness);
        cornerOriginPlusXY.transform.localScale = new(d.wallBoundThickness, d.wallHeight + d.wallBoundHeight, d.wallBoundThickness);

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
            GameObject _floor = Instantiate(d.floor, new Vector3(room.center.x, -d.wallHeight * .5f, room.center.y),
                Quaternion.identity, floorContainer.transform);
            _floor.transform.localScale = new Vector3(room.width, 1, room.height) / 10;
            yield return new WaitForSeconds(d.generationInterval);

            Material materialToAssign;
            if (room == d.originRoom)
                materialToAssign = d.roomSpecificAssets.breakRoomFloor;
            else switch (d.roomTypes[i])
                {
                    case RoomType.bakery:
                        materialToAssign = d.roomSpecificAssets.bakeryFloor;
                        break;
                    case RoomType.breakRoom:
                        materialToAssign = d.roomSpecificAssets.breakRoomFloor;
                        break;
                    case RoomType.kitchen:
                        materialToAssign = d.roomSpecificAssets.kitchenFloor;
                        break;
                    case RoomType.seating:
                        materialToAssign = d.roomSpecificAssets.seatingFloor;
                        break;
                    case RoomType.storage:
                        materialToAssign = d.roomSpecificAssets.storageFloor;
                        break;
                    default:
                        materialToAssign = d.roomSpecificAssets.kitchenFloor;
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
            switch (d.roomTypes[i])
            {
                case RoomType.bakery:
                    break;
                case RoomType.kitchen:
                    RectInt room = d.rooms[i];
                    // Kitchen island
                    Vector2 offset = new(-.5f * d.roomSpecificAssets.counterLength, -0.5f);
                    bool roomvertical = room.height > room.width;
                    float counterSize = d.roomSpecificAssets.counter.transform.lossyScale.x;

                    if (roomvertical) offset = new(offset.y, offset.x);
                    for (int j = 0; j < 2; j++)
                    {
                        for (int k = 0; k < d.roomSpecificAssets.counterLength; k++)
                        {
                            Transform counter = Instantiate(d.roomSpecificAssets.counter, new(
                                room.center.x + offset.x,
                                -d.wallHeight * .5f + counterSize * .5f,
                                room.center.y + offset.y
                                ),
                                j == 1 ? Quaternion.Euler(0, !roomvertical ? 0 : 90, 0) : Quaternion.Euler(0, !roomvertical ? 180 : 270, 0),
                                counterContainer.transform).transform;
                            if (roomvertical) offset.y += counterSize;
                            else offset.x += counterSize;

                            string itemToSpawn = GetItemFromLoottable(d.roomSpecificAssets.kitchenItemSpawns);
                            if (itemToSpawn != string.Empty)
                            {
                                PickupItem itemSpawned = Instantiate(d.roomSpecificAssets.itemPickup, new(
                                    counter.position.x, d.wallHeight, counter.position.z
                                    ), Quaternion.identity, itemSpawnsContainer.transform);
                                itemSpawned.itemToGive = ItemPresets.presets[itemToSpawn];
                            }
                        }
                        if (roomvertical) offset = new(offset.x + counterSize, -.5f * d.roomSpecificAssets.counterLength);
                        else offset = new(-.5f * d.roomSpecificAssets.counterLength, offset.y + counterSize);
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
            if (room == d.originRoom) continue;
            int enemiesToSpawn = 0;
            int enemyroll = d._random.Next(0, 100);
            for (int i = d.enemiesPerRoom.Length - 1; i >= 0; i--)
            {
                if (enemyroll > 100 - d.enemiesPerRoom[i].probability)
                {
                    enemiesToSpawn = d.enemiesPerRoom[i].Quantity;
                    break;
                }
            }
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                Instantiate(d.enemy, new Vector3(room.center.x + i, -d.wallHeight * .5f, room.center.y + i + 3),
                    Quaternion.identity, enemyContainer.transform);
                yield return new WaitForSeconds(d.generationInterval);
            }
            yield return new();
        }

        // Spawn Player
        Destroy(Camera.main.gameObject);
        Instantiate(d.player, new Vector3(d.originRoom.center.x, -d.wallHeight * .5f, d.originRoom.center.y), Quaternion.identity);
        d.coroutineIsDone = true;
        yield return new WaitForSeconds(d.generationInterval);
    }
    string GetItemFromLoottable(ItemLootLable[] lootTable)
    {
        int probabilityPassed = lootTable[0].probability;
        int lootRoll = d._random.Next(0, 100);
        for (int i = 0; i < lootTable.Length; i++)
        {
            if (lootRoll < probabilityPassed) return lootTable[i].itemName;
            else probabilityPassed += lootTable[i].probability;
        }
        return string.Empty;
    }
}
