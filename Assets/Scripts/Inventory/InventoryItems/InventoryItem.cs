using NaughtyAttributes;
using System;
using UnityEngine;

namespace InventoryStuff
{
    [CreateAssetMenu(
        fileName = "InventoryItem",
        menuName = "ScriptableObjects/Items/InventoryItem", 
        order = 0)]
    public class InventoryItem : ScriptableObject
    {
        //[HideInInspector] public InventoryItemData item = new();
        public virtual InventoryItemData GetItem() { return default; }
    }

    [System.Serializable]
    public class InventoryItemData
    {
        public string itemName;
        public GameObject itemModel;
        [TextArea] public string toolTip;
        public bool isStackable;
        public int maxStack = 1;
        [HideInInspector] public int amountLeft = 1;
        [Header("Stats")]
        public float durability;
        [Header("Properties")]
        public bool isFood;
        public bool isMetal;
        public bool isKnife;
        public bool onAttackHealHP;
        public bool damageScalesWithHP;
        public float damageAfterBlock;
        [Header("Overworld properties")]
        [Header("Passive effects")]
        public bool grantsImmortality;
        public bool knifeBoost;
        public bool foodBoost;
        public bool foodResistanceBoost;
        public bool durabilityBoost;
        public int healingBoost;
        public bool seeEnemyInventories;
        [SerializeField] public StatusEffect[] grantsImmunityTo;

        [HideInInspector] public Texture2D itemSprite;
        [HideInInspector] public bool canUseItem;
        [HideInInspector] public float currentDurability;

        public Texture2D SetSprite()
        {
            itemSprite = RuntimePreviewGenerator.GenerateModelPreview(itemModel.transform, 256, 256, false, true);
            itemSprite = SpriteEditor.AddOutline(itemSprite);

            return itemSprite;
        }

        public virtual void UseItem(Entity source, Vector3 inputDir) { }

        public virtual void UpdateAction() { }
    }
}
