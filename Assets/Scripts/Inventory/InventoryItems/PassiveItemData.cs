using UnityEngine;

namespace InventoryStuff
{
    [CreateAssetMenu(
        fileName = "PassiveItem",
        menuName = "ScriptableObjects/Items/PassiveItem",
        order = 0)]
    public class PassiveItemData : InventoryItem
    {
        public PassiveItem item = new();
    }

    public class PassiveItem : InventoryItemData
    {
        public override void UseItem(Entity source, Vector3 inputDir)
        {
            // do nothing lmao
        }
    }
}
