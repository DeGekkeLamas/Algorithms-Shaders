using UnityEngine;

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
        public float dmgReduction;
        public override void UseItem(Entity source, Vector3 inputDir)
        {

        }
        public override void UpdateAction()
        {

        }
    }

}
