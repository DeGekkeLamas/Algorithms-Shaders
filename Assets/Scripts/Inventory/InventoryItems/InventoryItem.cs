using NaughtyAttributes;
using System;
using UnityEngine;

namespace InventoryStuff
{
    public abstract class InventoryItem : ScriptableObject
    {
        //[HideInInspector] public InventoryItemData item = new();
        public abstract InventoryItemData GetItem();
    }

    [System.Serializable]
    public abstract class InventoryItemData
    {
        public string itemName;
        public GameObject itemModel;
        [TextArea] public string toolTip;
        public bool isStackable;
        public int maxStack = 1;
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

        public Texture2D SetSprite()
        {
            itemSprite = RuntimePreviewGenerator.GenerateModelPreview(itemModel.transform, 256, 256, false, true);
            itemSprite = SpriteEditor.AddOutline(itemSprite);

            return itemSprite;
        }

        public abstract void UseItem(Entity source, Vector3 inputDir);

        public abstract void UpdateAction();
    }
}
