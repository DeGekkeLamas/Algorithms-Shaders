using InventoryStuff;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGeneration
{
    /// <summary>
    /// Used for filling rooms with objects
    /// </summary>
    public abstract class RoomAssetGenerator : MonoBehaviour
    {
        [Header("Asset stuff")]
        public ItemLootTable lootSpawns;
        public Material wallMat;
        public Material floorMat;
        public PickupItem itemPickup;

        protected DungeonGenerator d;
        // Containers
        protected GameObject roomAssetContainer;
        protected GameObject itemSpawnsContainer;

        private void Awake()
        {
            d = this.GetComponent<DungeonGenerator>();

            // Containers
            roomAssetContainer = new("RoomAssetContainer");
            roomAssetContainer.transform.parent = d.assetContainer.transform;
            itemSpawnsContainer = new("ItemSpawnsContainer");
            itemSpawnsContainer.transform.parent = roomAssetContainer.transform;
        }

        /// <summary>
        /// Assigns to every room what type it is
        /// </summary>
        public void AssignRoomTypes()
        {
            return;
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
        public abstract IEnumerator SpawnObjects();

        protected PickupItem SpawnPickup(Vector3 position)
        {
            InventoryItemData itemToSpawn = ItemLootDrop<InventoryItemData>.GetItemFromLoottable(lootSpawns.lootTable, d.random);
            if (itemToSpawn != null) // only actually spawn if item isnt null
            {
                PickupItem itemSpawned = Instantiate(itemPickup, position, Quaternion.identity, itemSpawnsContainer.transform);
                itemSpawned.itemToGive = itemToSpawn.GetItem();
                return itemSpawned;
            }
            return null;
        }
    }

    public enum RoomType { bakery, breakRoom, kitchen, seating, storage };
}