using UnityEngine;

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
        public bool grantsImmortality;
        public bool knifeBoost;
        public bool foodBoost;
        public bool foodResistanceBoost;
        public bool durabilityBoost;
        public int healingBoost;
        public bool seeEnemyInventories;

        public override void UseItem(Entity source, Vector3 inputDir)
        {
            // do nothing lmao
        }
    }
}
