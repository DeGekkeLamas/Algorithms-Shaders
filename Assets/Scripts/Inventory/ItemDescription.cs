using System;
using UnityEngine;

namespace InventoryStuff
{
    public static class ItemDescription
    {
        public static string GenerateDescription(InventoryItemData item)
        {
            string _description = string.Empty;
            if (item.damage != 0)
                _description += $" \nDeals {item.damage} damage";
            if (item.damageScalesWithHP) _description += ", deals more damage to damaged enemies";

            if (item.hpHealed != 0)
                _description += $"\nHeals {item.hpHealed} HP when consumed";

            if (item.damageAfterBlock > 0)
                _description += $"\nDeals {item.damageAfterBlock} to the player after blocking";

            foreach (StatusEffect effect in item.effectApplied)
            {
                _description += $"\nInflicts {effect.name}";
            }

            if (item.durabilityBoost) _description += $"\nWhile in inventory, increases durability of items";
            if (item.foodBoost) _description += $"\nWhile in inventory, increases damage of food-based items";
            if (item.foodResistanceBoost) _description += $"\nWhile in inventory, increases defence against food-based items";
            if (item.knifeBoost) _description += $"\nWhile in inventory, increases damage of knifes and their upgrades";
            if (item.healingBoost > 0)
                _description += $"\n While in inventory, healing items heal {item.healingBoost} more HP";
            if (item.seeEnemyInventories) _description += $"\nWhile in inventory, allows you to see the inventories of enemies";
            if (item.grantsImmortality) _description += $"\nWhile in inventory, grants immunity to attacks";
            //if (item.isStoveIngredient) _description += $"\nCan be used as ingredient for crafting at stoves";

            _description += $" \n\"{item.toolTip}\"";

            return _description;
        }
    }
}
