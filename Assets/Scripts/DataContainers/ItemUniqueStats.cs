using NaughtyAttributes;
using System;
using UnityEngine;

namespace InventoryStuff
{
    /// <summary>
    /// Keeps track of the item it is, + the quanity of items left and durability left
    /// </summary>
    [Serializable]
    public struct ItemUniqueStats
    {
        [ReadOnly] public string itemName;
        public InventoryItem item;
        public float durabilityLeft;
        public float quantityLeft;

        public void Reset()
        {
            quantityLeft = 0;
            item = null;
        }
    }
}
