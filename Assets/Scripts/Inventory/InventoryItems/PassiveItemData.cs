using UnityEngine;
using Entities;

namespace InventoryStuff
{
    [CreateAssetMenu(
        fileName = "PassiveItem",
        menuName = "ScriptableObjects/Items/PassiveItem",
        order = 0)]
    public class PassiveItemData : InventoryItemData
    {
        public PassiveItem item = new();
        public override InventoryItem GetItem() { return item; }
    }

    [System.Serializable]
    public class PassiveItem : InventoryItem
    {
        [Header("Type specific")]
        public bool durabilityBoost;
        public int healingBoost;

        public override void UseItem(Entity source, Vector3 inputDir)
        {
            // do nothing lmao
        }
    }
}
