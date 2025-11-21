using Entities;
using UnityEngine;

namespace InventoryStuff
{
    [CreateAssetMenu(
        fileName = "PassiveDamageReductionItem",
        menuName = "ScriptableObjects/Items/PassiveItems/PassiveDamageReductionItem",
        order = 0)]
    public class PassiveDamageReductionItemData : InventoryItemData
    {
        public PassiveDamageReductionItem item = new();
        public override InventoryItem GetItem() { return item; }
    }

    [System.Serializable]
    public class PassiveDamageReductionItem : PassiveItem
    {
        public float damageReduced;

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
            if (dmg < 0) return dmg;

            Debug.Log($"Reduced damage taken from {dmg} to {dmg - damageReduced}");
            return Mathf.Max(dmg - damageReduced, 0f);
        }

        public override string GetItemDescription()
        {
            string description = base.GetItemDescription();

            description += $"Passively reduces damage taken by {damageReduced}.\n";

            return description;
        }
    }
}
