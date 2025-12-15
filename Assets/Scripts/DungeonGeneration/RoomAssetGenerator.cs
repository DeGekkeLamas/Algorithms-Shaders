using InventoryStuff;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;

namespace DungeonGeneration
{
    /// <summary>
    /// Used for filling rooms with objects
    /// </summary>
    public abstract class RoomAssetGenerator : MonoBehaviour
    {
        [Header("Asset stuff")]
        [SerializeField] ItemLootTable lootSpawns;
        [SerializeField] Material wallMat;
        public Material floorMat;
        [SerializeField] PickupItem itemPickup;

        protected DungeonGenerator d;
        // Containers
        protected GameObject roomAssetContainer;
        protected GameObject itemSpawnsContainer;

        [Header("Enemy stuff")]
        public int AmountOfEnemiesToSpawn = 8;
        [ReadOnly] [SerializeField] int totalEnemyChance;
        [SerializeField] ItemLootDrop<Entity>[] enemiesToSpawn;

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

        }
        private void Start()
        {
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
            if (d.rooms.Count <= 1) yield break;

            GameObject enemyContainer = new("EnemyContainer");
            enemyContainer.transform.parent = roomAssetContainer.transform;

            AlgorithmsUtils.FillRectangle(d.tilemap, d.originRoom, 1);
            for (int i = 0; i < AmountOfEnemiesToSpawn; i++)
            {
                Vector3 spot = GetAvailableSpot();
                SpawnEnemy(spot, enemyContainer.transform);
                //yield return d.interval;
            }
            yield return new();
        }

        public IEnumerator SpawnExitDoor()
        {
            RectInt endRoom = d.rooms[d.GetRoomNearestToPoint(d.initialRoom.max)];
            ExitDoor door = GameManager.instance.exit;
            door.gameObject.SetActive(true);

            Vector3 doorPos = new(endRoom.center.x, door.transform.position.y, endRoom.yMax - 2);
            door.transform.position = doorPos;
            Vector2 doorVec2 = new(doorPos.x, doorPos.z);

            // Add connection to graph, so it doesnt get structures placed on it
            d.DungoonGraph.AddNode(doorVec2);
            d.DungoonGraph.AddEdge(endRoom.center, doorVec2);

            yield return d.interval;
        }

        /// <summary>
        /// Get a random available spot on the map, using the tilemap
        /// </summary>
        protected Vector3 GetAvailableSpot()
        {
            Vector2Int position;
            do
            {
                position = new(d.random.Next(d.initialRoom.xMin, d.initialRoom.xMax), d.random.Next(d.initialRoom.yMin, d.initialRoom.yMax));
            }
            while (d.tilemap[position.y, position.x] != 0);

            return new(position.x, 0, position.y);
        }

        void SpawnEnemy(Vector3 position, Transform parent = null)
        {
            Entity toSpawn = ItemLootDrop<Entity>.GetItemFromLoottable(enemiesToSpawn, d.random);
            if (toSpawn != null)
            {
                Entity spawned = Instantiate(toSpawn, position, Quaternion.identity, parent);
                spawned.level = GameManager.instance.CurrentRoom;
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