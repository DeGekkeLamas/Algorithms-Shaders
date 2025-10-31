using NaughtyAttributes;
using UnityEngine;

namespace InventoryStuff
{
    [CreateAssetMenu(
        fileName = "ItemLootTable",
        menuName = "ScriptableObjects/Item Loottable",
        order = 0)]
    public class ItemLootTable : ScriptableObject
    {
        [ReadOnly] public int totalChance;
        public ItemLootDrop[] lootTable;

        private void OnValidate()
        {
            ItemLootDrop.MassValidate(lootTable);
            totalChance = ItemLootDrop.GetTotalItemProbability(lootTable);
        }

        [Button]
        void DoubleAll()
        {
            for (int i = 0; i < lootTable.Length; i++)
            {
                lootTable[i].probability *= 2;
            }
            OnValidate();
        }

        [Button]
        void HalfAll()
        {
            for (int i = 0; i < lootTable.Length; i++)
            {
                lootTable[i].probability /= 2;
            }
            OnValidate();
        }
    }
}
