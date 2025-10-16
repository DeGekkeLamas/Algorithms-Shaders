using NaughtyAttributes;
using UnityEngine;


namespace InventoryStuff
{
    [System.Serializable]
    public struct Recipe
    {
        [ReadOnly] public string name;

        public InventoryItem result;
        public InventoryItem[] ingredients;
    }
}
