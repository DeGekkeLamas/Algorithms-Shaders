using UnityEngine;

namespace InventoryStuff
{
    [CreateAssetMenu(
        fileName = "ConsumableItem",
        menuName = "ScriptableObjects/Items/ConsumableItem",
        order = 0)]
    public class ConsumableItemData : InventoryItemData
    {
        public ConsumableItem item = new();
        public override InventoryItem GetItem() { return item; }
    }

    [System.Serializable]
    public class ConsumableItem : InventoryItem
    {
        [Header("Type specific")]
        public float hpHealed;
        public StatusEffect[] effectApplied;

        public override void UseItem(Entity source, Vector3 inputDir)
        {
            Inventory.instance.RemoveItem(this);
        }

        public override void UpdateAction()
        {
            
        }
    }

}
