using NaughtyAttributes;
using System;
using UnityEngine;
using Entities;
using Entities.StatusEffects;

namespace InventoryStuff
{
    /// <summary>
    /// Baseclass for scriptableObject containing itemdata, to use add a value for the type of item and override the GetItem() function
    /// </summary>
    public abstract class InventoryItemData : ScriptableObject
    {
        //[HideInInspector] public InventoryItemData item = new();
        public abstract InventoryItem GetItem();
    }

    /// <summary>
    /// Baseclass for inventory items, contains general stats like name or model
    /// </summary>
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

        Texture2D itemSprite;
        /// <summary>
        /// Get a texture of this item. If it doesnt exist yet it will be created
        /// </summary>
        public Texture2D ItemSprite => itemSprite != null ? itemSprite : SetSprite();
        /// <summary>
        /// Get a silhoutte texture, which is completely black texture of this item. If it doesnt exist yet it will be created
        /// </summary>
        Texture2D itemSilhouette;
        public Texture2D ItemSilhouette => itemSilhouette != null ? itemSilhouette : SetSilhouette();
        [HideInInspector, NonSerialized] public bool canUseItem = true;
        public bool IsStackable => maxStack > 1;

        Texture2D SetSprite()
        {
            if (itemModel == null)
            {
                Debug.Log($"Model for {itemName} is null");
                return null;
            }
            itemSprite = RuntimePreviewGenerator.GenerateModelPreview(itemModel.transform, 256, 256, false, true);
            itemSprite = SpriteEditor.AddOutline(itemSprite);

            return itemSprite;
        }

        Texture2D SetSilhouette()
        {
            if (itemModel == null)
            {
                Debug.Log($"Model for {itemName} is null");
                return null;
            }
            itemSilhouette = RuntimePreviewGenerator.GenerateModelPreview(itemModel.transform, 256, 256, false, true);
            itemSilhouette = SpriteEditor.MakeSilhouette(itemSilhouette);

            return itemSilhouette;
        }

        protected void RemoveThisItem()
        {
            Inventory.instance.RemoveItem(Inventory.itemSelected);
        }

        /// <summary>
        /// Uses this item, override for items type specific interaction
        /// </summary>
        public abstract void UseItem(Entity source, Vector3 inputDir);

        public virtual void UpdateAction() { }
    }
}
