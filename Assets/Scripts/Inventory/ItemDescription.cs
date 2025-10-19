using System;
using Unity.VisualScripting;
using UnityEngine;

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
            description += GeneralItemInfo(item);
            // Consumable type
            if (item.GetType() == typeof(ConsumableItem))
                description += ConsumableItemInfo(item as ConsumableItem);
            // Melee type
            if (item.GetType() == typeof(MeleeWeapon)) 
                description += MeleeItemInfo(item as MeleeWeapon);
            // Ranged type
            if (item.GetType() == typeof(RangedWeapon))
                description += RangedItemInfo(item as RangedWeapon);
            // Passive type
            if (item.GetType() == typeof(PassiveItem))
                description += PassiveItemInfo(item as PassiveItem);

            description += $" \n\"{item.toolTip}\"";

            return description;
        }

        public static string GeneralItemInfo(InventoryItem item)
        {
            string description = string.Empty;
            // Max stacj
            if (item.IsStackable)
            {
                description += $"Max stack = {item.maxStack}.\n";
            }
            // Immune effects
            foreach (StatusEffect effect in item.grantsImmunityTo)
            {
                description += $"Grants immunity to {effect.name}.\n";
            }

            return description;
        }
        public static string ConsumableItemInfo(ConsumableItem item)
        {
            string description = string.Empty;

            // Healing
            description += $"Recovers {item.hpHealed} HP.\n";
            // Given effects
            foreach (StatusEffect effect in item.effectsApplied)
            {
                description += $"Gives {effect.name} effect.\n";
            }

            return description;
        }
        public static string MeleeItemInfo(MeleeWeapon item)
        {
            string description = string.Empty;

            // Damage
            description += $"Deals {item.damage} melee damage.\n";
            // Effects applied
            foreach (StatusEffect effect in item.effectApplied)
            {
                description += $"Inflicts {effect.name}.\n";
            }

            return description;
        }
        public static string RangedItemInfo(RangedWeapon item)
        {
            string description = string.Empty;

            // Damage
            description += $"Deals {item.damage} {(!item.projectile.useGravity ? "ranged" : "lobbed")} damage.\n";
            // fuel
            if (item.fuel != null) description += $"Uses {item.itemName} as fuel.\n";
            // Onetime use
            if (item.isConsumedOnUse) description += "Single use.\n";

            return description;
        }
        public static string PassiveItemInfo(PassiveItem item)
        {
            string description = string.Empty;

            // stuff

            return description;
        }
    }
}
