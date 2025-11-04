using InventoryStuff;
using UnityEngine;
using NaughtyAttributes;
using System;

namespace InventoryStuff
{
    [System.Serializable]
    public class ItemLootDrop<T>
    {
        [ReadOnly] public string itemName;
        public T item;
        public int probability;

        public void OnValidate()
        {
            // Set name
            if (item != null) itemName = $"{item}, {probability}%";
        }
        public static void MassValidate(ItemLootDrop<T>[] table)
        {
            for (int i = 0; i < table.Length; i++)
            {
                ItemLootDrop<T> item = table[i];
                item?.OnValidate();
            }
        }

        /// <summary>
        /// Returns item based on item probabilities, using a set seed
        /// </summary>
        public static T GetItemFromLoottable(ItemLootDrop<T>[] lootTable, System.Random seed)
        {
            if (lootTable.Length < 1)
            {
                Debug.LogWarning("Loottable is empty");
                return default;
            }

            int probabilityPassed = lootTable[0].probability;
            int lootRoll = seed.Next(0, 100);
            for (int i = 0; i < lootTable.Length; i++)
            {
                if (lootRoll < probabilityPassed) return lootTable[i].item;
                else probabilityPassed += lootTable[i].probability;
            }
            // If no item, return empty slot
            return default;
        }
        /// <summary>
        /// Returns item based on item probabilities
        /// </summary>
        public static T GetItemFromLoottable(ItemLootDrop<T>[] lootTable)
        {
            return GetItemFromLoottable(lootTable, new());
        }

        /// <summary>
        /// Get total chance of getting any item
        /// </summary>
        public static int GetTotalItemProbability(ItemLootDrop<T>[] lootTable)
        {
            int probability = 0;
            foreach (var loot in lootTable) probability += loot.probability;
            return probability;
        }
    }
}
