using UnityEngine;

namespace InventoryStuff
{
    [CreateAssetMenu(
        fileName = "ShieldItem",
        menuName = "ScriptableObjects/Items/ShieldItem",
        order = 0)]
    public class ShieldItemData : InventoryItem
    {
        public ShieldItem item = new();
        public override InventoryItemData GetItem() { return item; }
    }
    [System.Serializable]
    public class ShieldItem : InventoryItemData
    {
        [Header("Type specific")]
        public float dmgReduction;
        public override void UseItem(Entity source, Vector3 inputDir)
        {

        }
    }

}
