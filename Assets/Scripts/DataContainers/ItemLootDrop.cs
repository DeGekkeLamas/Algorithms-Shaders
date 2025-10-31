using InventoryStuff;
using UnityEngine;
using NaughtyAttributes;
using System;

namespace InventoryStuff
{
    [System.Serializable]
    public class ItemLootDrop
    {
        [ReadOnly] public string itemName;
        public InventoryItemData item;
        public int probability;

        public void OnValidate()
        {
            if (item != null) itemName = $"{item.GetItem().itemName}, {probability}%";
        }
        public static void MassValidate(ItemLootDrop[] table)
        {
            for (int i = 0; i < table.Length; i++)
            {
                ItemLootDrop item = table[i];
                item?.OnValidate();
            }
        }

        /// <summary>
        /// Returns item based on item probabilities, using a set seed
        /// </summary>
        public static InventoryItem GetItemFromLoottable(ItemLootDrop[] lootTable, System.Random seed)
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
                if (lootRoll < probabilityPassed) return lootTable[i].item.GetItem();
                else probabilityPassed += lootTable[i].probability;
            }
            // If no item, return empty slot
            return null;
        }
        /// <summary>
        /// Returns item based on item probabilities
        /// </summary>
        public static InventoryItem GetItemFromLoottable(ItemLootDrop[] lootTable)
        {
            return GetItemFromLoottable(lootTable, new());
        }

        /// <summary>
        /// Get total chance of getting any item
        /// </summary>
        public static int GetTotalItemProbability(ItemLootDrop[] lootTable)
        {
            int probability = 0;
            foreach (var loot in lootTable) probability += loot.probability;
            return probability;
        }
    }
}
