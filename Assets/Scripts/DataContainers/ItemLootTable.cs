using InventoryStuff;
using UnityEngine;
using NaughtyAttributes;
using System;

namespace InventoryStuff
{
    [System.Serializable]
    public struct ItemLootTable
    {
        [ReadOnly] public string itemName;
        public InventoryItem item;
        public int probability;

        public void OnValidate()
        {
            if (item != null) itemName = item.item.itemName;
        }

        /// <summary>
        /// Returns item based on item probabilities, using a set seed
        /// </summary>
        public static InventoryItemData GetItemFromLoottable(ItemLootTable[] lootTable, System.Random seed)
        {
            if (lootTable.Length < 1)
            {
                Debug.LogWarning("Loottable is empty");
                return null;
            }

            int probabilityPassed = lootTable[0].probability;
            int lootRoll = seed.Next(0, 100);
            for (int i = 0; i < lootTable.Length; i++)
            {
                if (lootRoll < probabilityPassed) return lootTable[i].item.item;
                else probabilityPassed += lootTable[i].probability;
            }
            // If no item, return empty slot
            return new InventoryItemData { slotIsEmty = true };
        }
        /// <summary>
        /// Returns item based on item probabilities
        /// </summary>
        public static InventoryItemData GetItemFromLoottable(ItemLootTable[] lootTable)
        {
            return GetItemFromLoottable(lootTable, new());
        }

        /// <summary>
        /// Get total chance of getting any item
        /// </summary>
        public static int GetTotalItemProbability(ItemLootTable[] lootTable)
        {
            int probability = 0;
            foreach (var loot in lootTable) probability += loot.probability;
            return probability;
        }
    }
}
