using InventoryStuff;
using UnityEngine;

public class OnDeathSpawnLoot : EntityDeathAction
{
    public ItemLootTable lootTable;

    protected override void OnDeath()
    {
        InventoryItemData itemDropped = ItemLootDrop<InventoryItemData>.GetItemFromLoottable(lootTable.lootTable);
        if (itemDropped != null && itemDropped != null)
        {
            Debug.Log($"{gameObject.name} dropped {itemDropped.GetItem().itemName}");
            PickupItem pickup = PickupItem.SpawnPickup(itemDropped.GetItem(), this.transform, Vector3.up);
            pickup.transform.parent = this.transform.parent;
        }
    }
}
