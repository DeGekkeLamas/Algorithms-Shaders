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
        public StatusEffect effectApplied;

        public override void UseItem(Entity source, Vector3 inputDir)
        {
            source.DealDamage(-hpHealed);
            if (effectApplied != null)
                source.activeStatusEffects.Add(effectApplied);
            Inventory.instance.RemoveItem(this);
            canUseItem = false;
        }

        public override void UpdateAction()
        {
            canUseItem = true;
        }
    }

}
