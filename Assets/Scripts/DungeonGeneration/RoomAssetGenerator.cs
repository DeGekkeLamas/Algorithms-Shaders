using InventoryStuff;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

        [Header("Enemy stuff")]
        public int AmountOfEnemiesToSpawn = 8;
        [ReadOnly] public int totalEnemyChance;
        public ItemLootDrop<Entity>[] enemiesToSpawn;

        protected virtual void OnValidate()
        {
            ItemLootDrop<Entity>.MassValidate(enemiesToSpawn);
            totalEnemyChance = ItemLootDrop<Entity>.GetTotalItemProbability(enemiesToSpawn);
            if (totalEnemyChance != 100)
            {
                Debug.LogWarning("totalEnemyChance is not 100");
            }
        }

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


        public IEnumerator SpawnEnemies()
        {
            GameObject enemyContainer = new("EnemyContainer");
            enemyContainer.transform.parent = roomAssetContainer.transform;

            AlgorithmsUtils.FillRectangle(d.tilemap, d.originRoom, 1);
            for (int i = 0; i < AmountOfEnemiesToSpawn; i++)
            {
                Vector2Int position;
                do
                {
                    position = new(d.random.Next(d.initialRoom.xMin, d.initialRoom.xMax), d.random.Next(d.initialRoom.yMin, d.initialRoom.yMax));
                }
                while (d.tilemap[position.y, position.x] != 0);

                SpawnEnemy(new(position.x, 0, position.y), enemyContainer.transform);
                //yield return d.interval;
            }
            yield return new();
        }

        void SpawnEnemy(Vector3 position, Transform parent = null)
        {
            Entity toSpawn = ItemLootDrop<Entity>.GetItemFromLoottable(enemiesToSpawn, d.random);
            if (toSpawn != null)
            {
                Instantiate(toSpawn, position, Quaternion.identity, parent);
            }
        }

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