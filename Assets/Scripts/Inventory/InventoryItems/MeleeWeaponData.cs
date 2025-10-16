using UnityEngine;

namespace InventoryStuff
{
    [CreateAssetMenu(
        fileName = "MeleeItem",
        menuName = "ScriptableObjects/Items/MeleeItem",
        order = 0)]
    public class MeleeWeaponData : InventoryItemData
    {
        public MeleeWeapon item = new();
        public override InventoryItem GetItem() { return item; }
    }

    [System.Serializable]
    public class MeleeWeapon : InventoryItem
    {
        [Header("Type specific")]
        public float damage;
        public float swingTime;
        public StatusEffect[] effectApplied;
        public override void UseItem(Entity source, Vector3 inputDir)
        {

        }
        public override void UpdateAction()
        {

        }
    }
}