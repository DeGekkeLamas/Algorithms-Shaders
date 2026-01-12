using InventoryStuff;
using UnityEngine;

namespace InventoryStuff
{
    /// <summary>
    /// Class used for crafting items currently in the inventory into another item
    /// </summary>
    public class ItemCrafter : MonoBehaviour
    {
        public Recipe recipe;

        /// <summary>
        /// Crafts item if possible, removes original items and adds result to inventory
        /// </summary>
        public void Craft()
        {
#if UNITY_EDITOR
            if (DebugCheats.alwaysCraft)
            {
                Inventory.instance.AddItem(recipe.result.GetItem());
                return;
            }
#endif
            if (ContainsAllItems())
            {
                foreach (InventoryItemData item in recipe.ingredients)
                {
                    Inventory.instance.RemoveFromStack(item.GetItem());
                }
                Inventory.instance.AddItem(recipe.result.GetItem());
                Debug.Log($"Crafted {recipe.recipeName}");
            }
            else Debug.Log($"Failed to craft {recipe.recipeName}");
        }

        /// <summary>
        /// Check if all required items are present in inventory
        /// </summary>
        bool ContainsAllItems()
        {
            int itemQTY = 0;
            foreach (InventoryItemData item in recipe.ingredients)
            {
                if (Inventory.instance.Contains(item.GetItem())) itemQTY++;
            }
            return itemQTY >= recipe.ingredients.Length;
        }
    }
}
