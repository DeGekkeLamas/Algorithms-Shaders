using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace InventoryStuff
{
    public class Inventory : MonoBehaviour
    {
        public InventoryItemData[] currentInventory = new InventoryItemData[5];

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
                currentInventory[i].cooldownLeft -= Time.deltaTime;
        }

        public bool AddItem(InventoryItemData itemToAdd)
        {
            if (itemToAdd.isStackable)
            {
                for (int i = 0; i < currentInventory.Length; i++)
                {
                    if (currentInventory[i].isStackable && currentInventory[i].amountLeft < currentInventory[i].maxStack
                        && currentInventory[i].itemName == itemToAdd.itemName)
                    {
                        currentInventory[i].amountLeft++;
                        UpdateInventoryTexts();
                        return true;
                    }
                }
            }
            for (int i = 0; i < currentInventory.Length; i++)
            {
                if (currentInventory[i].slotIsEmty)
                {
                    currentInventory[i] = itemToAdd;
                    UpdateInventory(i);
                    Debug.Log("Added " + itemToAdd.itemName + ", from " + this);
                    return true;
                }
            }
            Debug.Log("Inventory full, from " + this);
            return false;
        }
        public bool InventoryHasSpace()
        {
            for (int i = 0; i < currentInventory.Length; i++)
                if (currentInventory[i].slotIsEmty) return true;
            return false;
        }
        /// <summary>
        /// Remove an item by its index
        /// </summary>
        public void RemoveItem(int index)
        {
            currentInventory[index] = new InventoryItemData { slotIsEmty = true };
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
                    currentInventory[index] = new InventoryItemData { slotIsEmty = true };
                    UpdateInventory(index);
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
            if (currentInventory[index].itemModel != null)
            {
                currentInventory[index].SetSprite();
            }
            else currentInventory[index].itemSprite = HotbarItemBG;

            Hotbar[index].sprite = Sprite.Create(currentInventory[index].itemSprite, new Rect(0.0f, 0.0f,
                currentInventory[index].itemSprite.width, currentInventory[index].itemSprite.height), new Vector2(0, 0));

            HH.SetSelectedBorder(itemSelected);
            UpdateInventoryTexts();
        }
        void UpdateInventoryTexts()
        {

            for (int i = 0; i < currentInventory.Length; i++)
            {
                // Set quantity counters
                if (currentInventory[i].isStackable)
                {
                    quantityCounters[i].gameObject.SetActive(true);
                    quantityCounters[i].text = currentInventory[i].amountLeft.ToString();
                }
                else quantityCounters[i].gameObject.SetActive(false);
            }
        }
    }
}

