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
        public StatusEffect[] effectsApplied;

        public override void UseItem(Entity source, Vector3 inputDir)
        {
            source.DealDamage(-hpHealed);
            for (int i = 0; i < effectsApplied.Length; i++)
            {
                source.activeStatusEffects.Add(effectsApplied[i]);
            }
            Inventory.instance.RemoveItem(this);
            canUseItem = false;
        }

        public override void UpdateAction()
        {
            canUseItem = true;
        }
    }

}
