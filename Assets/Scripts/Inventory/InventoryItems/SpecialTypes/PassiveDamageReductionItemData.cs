using Entities;
using UnityEngine;

namespace InventoryStuff
{
    /// <summary>
    /// ScriptableObject for PassiveDamageReductionItem
    /// </summary>
    [CreateAssetMenu(
        fileName = "PassiveDamageReductionItem",
        menuName = "ScriptableObjects/Items/Special/PassiveDamageReductionItem",
        order = 0)]
    public class PassiveDamageReductionItemData : InventoryItemData
    {
        public PassiveDamageReductionItem item = new();
        public override InventoryItem GetItem() { return item; }
    }

    /// <summary>
    /// Itemtype that passively reduces damage taken and/or increased hp recovered from healing items
    /// </summary>
    [System.Serializable]
    public class PassiveDamageReductionItem : PassiveItem
    {
        [SerializeField] float damageReduced;
        [SerializeField] float extraHealing;

        public override void OnItemObtained(Entity source)
        {
            source.processDamageReceived.Add(ReduceDamageTaken);
        }

        public override void OnItemRemoved(Entity source)
        {
            source.processDamageReceived.Remove(ReduceDamageTaken);
        }

        float ReduceDamageTaken(float dmg)
        {
            if (dmg < 0)
            {
                Debug.Log($"Increased HP recovered from {-dmg} to {-(dmg - damageReduced)}");
                return dmg - extraHealing;
            }
            else
            {
                Debug.Log($"Reduced damage taken from {dmg} to {dmg - damageReduced}");
                return Mathf.Max(dmg - damageReduced, 0f);
            }

        }

        public override string GetItemDescription()
        {
            string description = base.GetItemDescription();

            if (damageReduced != 0) description += $"Passively reduces damage taken by {damageReduced}.\n";
            if (extraHealing != 0) description += $"Consumable items recover {extraHealing} extra HP.\n";

            return description;
        }
    }
}
