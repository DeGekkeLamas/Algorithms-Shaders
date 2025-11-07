using InventoryStuff;
using UnityEngine;

public class ItemCrafter : MonoBehaviour
{
    public Recipe recipe;

    public void Craft()
    {
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

    bool ContainsAllItems()
    {
        int itemQTY = 0;
        foreach(InventoryItemData item in recipe.ingredients)
        {
            if (Inventory.instance.Contains(item.GetItem() ) ) itemQTY++;
        }
        return itemQTY >= recipe.ingredients.Length;
    }
}
