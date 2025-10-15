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
    }
    //[System.Serializable]
    public class ShieldItem : InventoryItemData
    {
        public float dmgReduction;
        public override void UseItem(Entity source, Vector3 inputDir)
        {

        }
    }

}
