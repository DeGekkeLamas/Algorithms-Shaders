using UnityEngine;
using Entities;
using System.Collections;

namespace InventoryStuff
{
    [CreateAssetMenu(
        fileName = "ShieldItem",
        menuName = "ScriptableObjects/Items/ShieldItem",
        order = 0)]
    public class ShieldItemData : InventoryItemData
    {
        public ShieldItem item = new();
        public override InventoryItem GetItem() { return item; }
    }
    [System.Serializable]
    public class ShieldItem : InventoryItem
    {
        [Header("Type specific")]
        public float dmgReduction = 20;
        public float blockDuration = 1;
        static bool blockIsActive;

        public override void UseItem(Entity source, Vector3 inputDir)
        {
            if (blockIsActive)
            {
                Debug.Log("Block is already active");
                return;
            }
            Debug.Log($"Used item {itemName} for blocking");
            //Debug.LogWarning("Shield items are not implemented yet");
            source.StartCoroutine(BlockCycle(source));
        }
        IEnumerator BlockCycle(Entity source)
        {
            blockIsActive = true;
            source.processDamageReceived.Add(ReduceDamage);

            yield return new WaitForSeconds(blockDuration);

            source.processDamageReceived.Remove(ReduceDamage);
            blockIsActive = false;
            Debug.Log("Block duration over");
        }

        float ReduceDamage(float dmg)
        {
            if (dmg < 0) return dmg;

            Debug.Log($"Reduced damage taken from {dmg} to {dmg - dmgReduction}");
            return Mathf.Max(dmg - dmgReduction, 0);
        }

        public override string GetItemDescription()
        {
            string description = base.GetItemDescription();

            description += $"When used, reduce damage taken by {dmgReduction} for {blockDuration} seconds\n";

            return description;
        }
    }

}
