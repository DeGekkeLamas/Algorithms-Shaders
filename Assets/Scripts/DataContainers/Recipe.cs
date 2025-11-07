using NaughtyAttributes;
using UnityEngine;


namespace InventoryStuff
{
    [CreateAssetMenu(
        fileName = "Recipe",
        menuName = "ScriptableObjects/Recipe",
        order = 0)]
    public class Recipe : ScriptableObject
    {
        [ReadOnly] public string recipeName;

        public InventoryItemData result;
        public InventoryItemData[] ingredients;

        void OnValidate()
        {
            if (result != null)
            {
                recipeName = $"{result.GetItem().itemName}: ";
            }
            foreach (InventoryItemData item in ingredients)
            {
                recipeName += $"{item.GetItem().itemName}, ";
            }
        }

        public bool Contains(InventoryItem item)
        {
            foreach (InventoryItemData ingredient in ingredients)
            {
                if (item == ingredient.GetItem()) return true;
            }
            return false;
        }
    }
}
