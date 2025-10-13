using UnityEngine;

namespace InventoryStuff
{
    public class MeleeWeaponData : InventoryItem
    {
        public MeleeWeapon item = new();
    }
    public class MeleeWeapon : InventoryItemData
    {
        public float damage;
        public StatusEffect[] effectApplied;
        public override void UseItem(Entity source, Vector3 inputDir)
        {

        }
    }
}