using UnityEngine;

namespace InventoryStuff
{
    [CreateAssetMenu(
        fileName = "MeleeItem",
        menuName = "ScriptableObjects/Items/MeleeItem",
        order = 0)]
    public class MeleeWeaponData : InventoryItem
    {
        public MeleeWeapon item = new();
        public override InventoryItemData GetItem() { return item; }
    }

    [System.Serializable]
    public class MeleeWeapon : InventoryItemData
    {
        [Header("Type specific")]
        public float damage;
        public StatusEffect[] effectApplied;
        public override void UseItem(Entity source, Vector3 inputDir)
        {

        }
    }
}