using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Entities.Player;

namespace InventoryStuff
{
    /// <summary>
    /// Singleton managing the player inventory
    /// </summary>
    public class Inventory : MonoBehaviour
    {
        public ItemUniqueStats[] currentInventory = new ItemUniqueStats[5];

        [Header("UI")]
        public static int itemSelected;

        public static Inventory instance;
        public event Action OnItemChanged;
        public static event Action OnSeletecItemSwitched;

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(this.gameObject);
        }

        void Update()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                float _direction = (Input.mouseScrollDelta.y > 0) ? -1 : 1;
                itemSelected += Mathf.RoundToInt(_direction);
                itemSelected = (int)Mathf.Repeat(itemSelected, currentInventory.Length);
                OnSeletecItemSwitched?.Invoke();
            }

            for (int i = 0; i < currentInventory.Length; i++)
            {
                if (currentInventory[i].item !=  null) currentInventory[i].item.UpdateAction();
            }
                
        }
        /// <summary>
        /// Add an item to the inventory, returns if adding was succesful or not
        /// </summary>
        public bool AddItem(InventoryItem itemToAdd)
        {
            // Stackable
            if (itemToAdd.IsStackable)
            {
                for (int i = 0; i < currentInventory.Length; i++)
                {
                    InventoryItem item = currentInventory[i].item;

                    if (item != null && item.IsStackable && currentInventory[i].quantityLeft < item.maxStack
                        && item.itemName == itemToAdd.itemName)
                    {
                        currentInventory[i].quantityLeft++;
                        currentInventory[i].itemName = itemToAdd.itemName;
                        currentInventory[i].item.OnItemObtained(PlayerController.instance);
                        OnItemChanged?.Invoke();
                        return true;
                    }
                }
            }
            // Non stackable
            for (int i = 0; i < currentInventory.Length; i++)
            {
                if (currentInventory[i].item == null)
                {
                    currentInventory[i].quantityLeft++;
                    currentInventory[i].item = itemToAdd;
                    currentInventory[i].itemName = itemToAdd.itemName;
                    currentInventory[i].item.OnItemObtained(PlayerController.instance);
                    OnItemChanged?.Invoke();
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
                if (currentInventory[i].item == null) return true;
            return false;
        }

        public bool Contains(InventoryItem item)
        {
            foreach(ItemUniqueStats invItem in currentInventory)
            {
                if (invItem.item == item) return true;
            }
            return false;
        }

        /// <summary>
        /// Remove an item by its index
        /// </summary>
        public void RemoveItem(int index)
        {
            currentInventory[index].item?.OnItemRemoved(PlayerController.instance);
            currentInventory[index].Reset();
            OnItemChanged?.Invoke();
        }

        /// <summary>
        /// Remove an item by itemdata object
        /// </summary>
        public void RemoveItem(InventoryItem item)
        {
            int index = 0;
            for (int i = 0; i < currentInventory.Length; i++)
            {
                // Compare to find item in inventory, remove by index if match
                InventoryItem currentItem = currentInventory[i].item;
                if (currentItem != null && currentItem == item)
                {
                    index = i;
                    RemoveItem(index);
                    return;
                }
            }
            return;
        }

        public void ClearInventory()
        {
            for(int i = 0;i < currentInventory.Length; i++)
            {
                RemoveItem(i);
            }
            Debug.Log("Cleared inventory");
        }

        /// <summary>
        /// Remove an item from a stack
        /// </summary>
        public void RemoveFromStack(int index)
        {
            currentInventory[index].item?.OnItemRemoved(PlayerController.instance);
            currentInventory[index].quantityLeft--;
            if (currentInventory[index].quantityLeft <= 0) RemoveItem(index);
            OnItemChanged?.Invoke();
        }

        /// <summary>
        /// Remove an item from a stack
        /// </summary>
        public void RemoveFromStack(InventoryItem item)
        {
            int index = 0;
            for (int i = 0; i < currentInventory.Length; i++)
            {
                // Compare to find item in inventory, remove by index if match
                InventoryItem currentItem = currentInventory[i].item;
                if (currentItem != null && currentItem.itemName == item.itemName)
                {
                    index = i;
                    RemoveFromStack(index);
                    return;
                }
            }
            return;
        }
    }
}

