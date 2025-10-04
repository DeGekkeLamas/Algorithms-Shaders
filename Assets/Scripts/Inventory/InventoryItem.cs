using NaughtyAttributes;
using System;
using UnityEngine;

namespace InventoryStuff
{
    [CreateAssetMenu]
    public class InventoryItem : ScriptableObject
    {
        public InventoryItemData item = new();
    }

    [System.Serializable]
    public class InventoryItemData
    {
        [HideInInspector] public Texture2D itemSprite;
        public string itemName;
        public GameObject itemModel;
        [TextArea] public string toolTip;
        public bool isStackable;
        public int maxStack = 1;
        [HideInInspector] public int amountLeft = 1;
        [Header("Stats")]
        public float durability;
        [HideInInspector] public float currentDurability;
        public float damage; //
        public float hpHealed; //
        [Header("Properties")]
        [HideInInspector] public bool slotIsEmty;
        public bool isConsumedOnUse; //
        public bool isFood;
        public bool isMetal;
        public bool isKnife;
        [SerializeField] public StatusEffect[] effectApplied;
        public bool onAttackHealHP;
        public bool damageScalesWithHP;
        public float damageAfterBlock;
        [Header("Overworld properties")]
        public Rigidbody projectile; //
        public bool autoFire; //
        public float cooldown; //
        public float cooldownLeft; //
        [Header("Passive effects")]
        public bool grantsImmortality;
        public bool knifeBoost;
        public bool foodBoost;
        public bool foodResistanceBoost;
        public bool durabilityBoost;
        public int healingBoost;
        public bool seeEnemyInventories;
        [SerializeField] public StatusEffect[] grantsImmunityTo;

        public Texture2D SetSprite()
        {
            itemSprite = RuntimePreviewGenerator.GenerateModelPreview(itemModel.transform, 256, 256, false, true);
            itemSprite = SpriteEditor.AddOutline(itemSprite);

            return itemSprite;
        }

        public virtual void UseItem() { }

        [Button]
        void ConvertToConsumableItem(ConsumableItem target)
        {

        }
    }

    public class ConsumableItem : InventoryItemData
    {
        public float hpHealed;
        public StatusEffect[] effectApplied;

        public override void UseItem()
        {
            Inventory.instance.RemoveItem(this);
        }
    }
    public class RangedWeapon : InventoryItemData
    {
        public float damage;
        public StatusEffect[] effectApplied;
        public Rigidbody projectile;
        public bool autoFire;
        public float cooldown;
        public float cooldownLeft;
        public bool isConsumedOnUse;
        [Tooltip("Leave empty to use no fuel")]
        public InventoryItemData fuel;
    }
    public class MeleeWeapon : InventoryItemData
    {
        public float damage;
        public StatusEffect[] effectApplied;
    }

}
