using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace InventoryStuff
{
    public class Inventory : MonoBehaviour
    {
        [HideInInspector, NonSerialized] public InventoryItemData[] currentInventory = new InventoryItemData[5];

        [Header("UI")]
        public Image[] Hotbar = new Image[5];
        public TMP_Text[] quantityCounters = new TMP_Text[5];
        public Texture2D HotbarItemBG;
        public static int itemSelected;

        public static Inventory instance;

        HotbarHover HH;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else Destroy(this);

            HH = GameObject.FindFirstObjectByType<HotbarHover>().GetComponent<HotbarHover>();
        }
        private void Start()
        {
            for (int i = 0; i < currentInventory.Length; i++) UpdateInventory(i);
        }
        void Update()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                float _direction = (Input.mouseScrollDelta.y > 0) ? -1 : 1;
                itemSelected += Mathf.RoundToInt(_direction);
                if (itemSelected < 0) itemSelected = 4;
                if (itemSelected > 4) itemSelected = 0;
                HH.SetSelectedBorder(itemSelected);
            }

            for (int i = 0; i < currentInventory.Length; i++)
            {
                if (currentInventory[i] !=  null) currentInventory[i].UpdateAction();
            }
                
        }
        /// <summary>
        /// Add an item to the inventory, returns if adding was succesful or not
        /// </summary>
        public bool AddItem(InventoryItemData itemToAdd)
        {
            // Stackable
            if (itemToAdd.isStackable)
            {
                for (int i = 0; i < currentInventory.Length; i++)
                {
                    InventoryItemData item = currentInventory[i];

                    if (item != null && item.isStackable && item.amountLeft < item.maxStack
                        && item.itemName == itemToAdd.itemName)
                    {
                        currentInventory[i].amountLeft++;
                        UpdateInventoryTexts();
                        return true;
                    }
                }
            }
            // Non stackable
            for (int i = 0; i < currentInventory.Length; i++)
            {
                if (currentInventory[i] == null)
                {
                    currentInventory[i] = itemToAdd;
                    UpdateInventory(i);
                    Debug.Log("Added " + itemToAdd.itemName + ", from " + this);
                    return true;
                }
            }
            // Failed to add
            Debug.Log("Inventory full, from " + this);
            return false;
        }
        public bool InventoryHasSpace()
        {
            for (int i = 0; i < currentInventory.Length; i++)
                if (currentInventory[i] == null) return true;
            return false;
        }
        /// <summary>
        /// Remove an item by its index
        /// </summary>
        public void RemoveItem(int index)
        {
            currentInventory[index] = null;
            UpdateInventory(index);
        }

        /// <summary>
        /// Remove an item by itemdata object
        /// </summary>
        public void RemoveItem(InventoryItemData item)
        {
            int index = 0;
            for (int i = 0; i < currentInventory.Length; i++)
            {
                // Compare to find item in inventory, remove by index if match
                if (currentInventory[i].itemName == item.itemName)
                {
                    index = i;
                    RemoveItem(index);
                }
            }
            return;
        }

        /// <summary>
        /// Remove an item from a stack
        /// </summary>
        public void RemoveFromStack(int index)
        {
            currentInventory[index].amountLeft--;
            if (currentInventory[index].amountLeft == 0) RemoveItem(index);
            UpdateInventoryTexts();
        }

        [ContextMenu("Update inventory")]
        public void UpdateInventory(int index)
        {
            InventoryItemData item = currentInventory[index];

            if (item == null)
            {
                Hotbar[index].sprite = Sprite.Create(HotbarItemBG, new Rect(0.0f, 0.0f,
                HotbarItemBG.width, HotbarItemBG.height), new Vector2(0, 0));
                UpdateInventoryTexts();
                return;
            }

            item.SetSprite();

            Hotbar[index].sprite = Sprite.Create(item.itemSprite, new Rect(0.0f, 0.0f,
                item.itemSprite.width, item.itemSprite.height), new Vector2(0, 0));

            HH.SetSelectedBorder(itemSelected);
            UpdateInventoryTexts();
        }
        void UpdateInventoryTexts()
        {

            for (int i = 0; i < currentInventory.Length; i++)
            {
                InventoryItemData item = currentInventory[i];
                quantityCounters[i].gameObject.SetActive(false);
                if (item != null)
                {
                    // Set quantity counters
                    if (item.isStackable)
                    {
                        quantityCounters[i].gameObject.SetActive(true);
                        quantityCounters[i].text = currentInventory[i].amountLeft.ToString();
                    }
                }
            }
        }
    }
}

