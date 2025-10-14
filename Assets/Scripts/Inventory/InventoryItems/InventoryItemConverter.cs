using InventoryStuff;
using NaughtyAttributes;
using UnityEngine;

public class InventoryItemConverter : MonoBehaviour
{
    public InventoryItem original;

    // Ranged
    [Header("Ranged")]
    public RangedWeaponData rangedNew;
    [Button]
    public void ConvertToRanged()
    {
        rangedNew.item = (RangedWeapon)original.item;
        rangedNew.item.damage = original.item.damage;
        rangedNew.item.effectApplied = original.item.effectApplied;
        rangedNew.item.projectile = original.item.projectile;
        rangedNew.item.autoFire = original.item.autoFire;
        rangedNew.item.cooldown = original.item.cooldown;
        rangedNew.item.isConsumedOnUse = original.item.isConsumedOnUse;
    }
    // Melee
    [Header("Melee")]
    public MeleeWeaponData meleeNew;
    [Button]
    public void ConvertToMelee()
    {
        meleeNew.item = (MeleeWeapon)original.item;
        meleeNew.item.damage = original.item.damage;
        meleeNew.item.effectApplied = original.item.effectApplied;
    }
    // Consumable
    [Header("Consumable")]
    public ConsumableItemData consumableNew;
    [Button]
    public void ConvertToConsumable()
    {
        consumableNew.item = (ConsumableItem)original.item;
        consumableNew.item.hpHealed = original.item.hpHealed;
        consumableNew.item.effectApplied = original.item.effectApplied;
    }
    // Passive
    [Header("Consumable")]
    public PassiveItemData passiveNew;
    [Button]
    public void ConvertToPassive()
    {
        passiveNew.item = (PassiveItem)original.item;
        passiveNew.item.hpHealed = original.item.hpHealed;
        passiveNew.item.effectApplied = original.item.effectApplied;
    }
}
