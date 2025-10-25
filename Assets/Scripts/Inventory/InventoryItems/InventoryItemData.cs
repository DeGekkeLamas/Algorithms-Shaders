using NaughtyAttributes;
using System;
using UnityEngine;

namespace InventoryStuff
{
    public abstract class InventoryItemData : ScriptableObject
    {
        //[HideInInspector] public InventoryItemData item = new();
        public abstract InventoryItem GetItem();
    }

    [System.Serializable]
    public abstract class InventoryItem
    {
        public string itemName;
        public GameObject itemModel;
        [TextArea] public string toolTip;
        public int maxStack = 1;
        [Header("Stats")]
        public float durability;
        [Header("Overworld properties")]
        [SerializeField] public StatusEffect[] grantsImmunityTo;

        [HideInInspector] public Texture2D itemSprite;
        [HideInInspector, NonSerialized] public bool canUseItem = true;
        public bool IsStackable => maxStack > 1;

        public Texture2D SetSprite()
        {
            itemSprite = RuntimePreviewGenerator.GenerateModelPreview(itemModel.transform, 256, 256, false, true);
            itemSprite = SpriteEditor.AddOutline(itemSprite);

            return itemSprite;
        }

        protected void RemoveThisItem()
        {
            Inventory.instance.RemoveItem(Inventory.itemSelected);
        }

        public abstract void UseItem(Entity source, Vector3 inputDir);

        public virtual void UpdateAction() { }
    }
}
