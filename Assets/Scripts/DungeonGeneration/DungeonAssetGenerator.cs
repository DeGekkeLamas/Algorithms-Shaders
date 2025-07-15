using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonAssetGenerator : MonoBehaviour
{
    public GameObject enemy;
    public EnemyProbabilities[] enemiesPerRoom;
    public RoomSpecificAssets rsa;
    RoomType[] roomTypes;
    List<GameObject> wallsGenerated = new();

    DungeonGenerator d;

    private void OnValidate()
    {
        // Get total item probabilities
        rsa.bakeryTotalItemChance = GetTotalItemProbability(rsa.bakeryItemSpawns);
        rsa.breakTotalItemChance = GetTotalItemProbability(rsa.breakItemSpawns);
        rsa.kitchenTotalItemChance = GetTotalItemProbability(rsa.kitchenItemSpawns);
        rsa.storageTotalItemChance = GetTotalItemProbability(rsa.storageItemSpawns);
        rsa.seatingTotalItemChance = GetTotalItemProbability(rsa.seatingItemSpawns);

        // Set names of item tables
        for (int i = 0; i < rsa.bakeryItemSpawns.Length; i++) 
            rsa.bakeryItemSpawns[i].itemName = rsa.bakeryItemSpawns[i].item.name;
        for (int i = 0; i < rsa.breakItemSpawns.Length; i++) 
            rsa.breakItemSpawns[i].itemName = rsa.breakItemSpawns[i].item.name;
        for (int i = 0; i < rsa.kitchenItemSpawns.Length; i++) 
            rsa.kitchenItemSpawns[i].itemName = rsa.kitchenItemSpawns[i].item.name;
        for (int i = 0; i < rsa.seatingItemSpawns.Length; i++)
            rsa.seatingItemSpawns[i].itemName = rsa.seatingItemSpawns[i].item.name;
        for (int i = 0; i < rsa.storageItemSpawns.Length; i++) 
            rsa.storageItemSpawns[i].itemName = rsa.storageItemSpawns[i].item.name;
        
    }

    private void Awake() => d = this.GetComponent<DungeonGenerator>();

    /// <summary>
    /// Assigns to every room what type it is
    /// </summary>
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
    }
    /// <summary>
    /// Brickifies all objects with WallGenerator script on it
    /// </summary>
    public IEnumerator Brickify()
    {
        // Brickifies generated walls
        WallGenerator[] walls = GameObject.FindObjectsByType<WallGenerator>(FindObjectsSortMode.None);
        foreach (WallGenerator wall in walls)
        {
            wall.GenerateWalls();
        }

        yield return new();
    }

    /// <summary>
    /// Places objects in rooms
    /// </summary>
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
                                0 + counterSize * .5f,
                                room.center.y + offset.y
                                ),
                                j == 1 ? Quaternion.Euler(0, !roomvertical ? 0 : 90, 0) : Quaternion.Euler(0, !roomvertical ? 180 : 270, 0),
                                counterContainer.transform).transform;
                            if (roomvertical) offset.y += counterSize;
                            else offset.x += counterSize;

                            InventoryItemData itemToSpawn = GetItemFromLoottable(rsa.kitchenItemSpawns);
                            if (!itemToSpawn.slotIsEmty)
                            {
                                PickupItem itemSpawned = Instantiate(rsa.itemPickup, new(
                                    counter.position.x, 10, counter.position.z
                                    ), Quaternion.identity, itemSpawnsContainer.transform);
                                itemSpawned.itemPreset.item = itemToSpawn;
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
                Instantiate(enemy, new Vector3(room.center.x + i, 0 * .5f, room.center.y + i + 3),
                    Quaternion.identity, enemyContainer.transform);
                yield return new WaitForSeconds(d.generationInterval);
            }
            yield return new();
        }

        yield return new WaitForSeconds(d.generationInterval);
    }
    /// <summary>
    /// Returns item based on item probabilities
    /// </summary>
    InventoryItemData GetItemFromLoottable(ItemLootLable[] lootTable)
    {
        int probabilityPassed = lootTable[0].probability;
        int lootRoll = d.GetSeed().Next(0, 100);
        for (int i = 0; i < lootTable.Length; i++)
        {
            if (lootRoll < probabilityPassed) return lootTable[i].item.item;
            else probabilityPassed += lootTable[i].probability;
        }
        // If no item, return empty slot
        return new InventoryItemData { slotIsEmty = true };
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
    public InventoryItem item;
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

