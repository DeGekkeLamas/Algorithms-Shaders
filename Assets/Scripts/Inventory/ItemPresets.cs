using System.Collections.Generic;
using UnityEngine;

namespace InventoryStuff
{
    public class ItemPresets : MonoBehaviour
    {
        public InventoryItem[] items;
        public static InventoryItem[] presets;
        public GameObject placeholderModel;
        public Rigidbody placeholderProjectile;

        private void Awake()
        {
            presets = items;
            foreach (var preset in presets)
            {
                if (preset.item.itemModel == null)
                    preset.item.itemModel = placeholderModel;
            }
        }
    }
}
