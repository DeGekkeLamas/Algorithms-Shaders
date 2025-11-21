using System;
using Unity.VisualScripting;
using UnityEngine;
using Entities.StatusEffects;

namespace InventoryStuff
{
    public static class ItemDescription
    {
        public static string GenerateDescription(InventoryItem item)
        {
            // Empty
            if (item == null)
            {
                return StringTools.Bold("Empty");
            }

            string description = $"{StringTools.Bold(item.itemName)}\n\n";
            description += item.GetItemDescription();

            description += $" \n\"{item.toolTip}\"";

            return description;
        }
    }
}
