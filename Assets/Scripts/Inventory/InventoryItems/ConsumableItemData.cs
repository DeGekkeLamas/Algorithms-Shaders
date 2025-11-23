using Entities;
using Entities.StatusEffects;
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
            RemoveThisItem();
            canUseItem = false;
        }

        public override void UpdateAction()
        {
            canUseItem = true;
        }

        public override string GetItemDescription()
        {
            string description = base.GetItemDescription();

            // Healing
            description += $"Recovers {hpHealed} HP.\n";
            // Given effects
            foreach (StatusEffect effect in effectsApplied)
            {
                description += $"Gives {effect.name} effect.\n";
            }

            return description;

        }
    }

}
