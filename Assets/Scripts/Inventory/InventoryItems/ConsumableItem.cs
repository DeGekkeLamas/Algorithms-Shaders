using UnityEngine;

namespace InventoryStuff
{
    public class ConsumableItemData : InventoryItem
    {
        public ConsumableItem item = new();
    }
    public class ConsumableItem : InventoryItemData
    {
        public float hpHealed;
        public StatusEffect[] effectApplied;

        public override void UseItem(Entity source, Vector3 inputDir)
        {
            Inventory.instance.RemoveItem(this);
        }
    }

}
