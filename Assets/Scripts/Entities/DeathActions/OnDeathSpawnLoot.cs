using InventoryStuff;
using UnityEngine;

namespace Entities
{
    /// <summary>
    /// Spawn an item from the loottable when the bound entity dies
    /// </summary>
    public class OnDeathSpawnLoot : EntityDeathAction
    {
        public ItemLootTable lootTable;

        protected override void OnDeath()
        {
            // Spawn drops
            InventoryItemData itemDropped = ItemLootDrop<InventoryItemData>.GetItemFromLoottable(lootTable.lootTable);
            if (itemDropped != null && itemDropped != null)
            {
                Debug.Log($"{gameObject.name} dropped {itemDropped.GetItem().itemName}");
                PickupItem pickup = PickupItem.SpawnPickup(itemDropped.GetItem(), this.transform, Vector3.up);
                pickup.transform.parent = this.transform.parent;
            }
        }
    }

}