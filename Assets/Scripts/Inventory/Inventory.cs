using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Inventory : MonoBehaviour
{
    public InventoryItem[] currentInventory = new InventoryItem[5];

    [Header("UI")]
    public Image[] Hotbar = new Image[5];
    public TMP_Text[] quantityCounters = new TMP_Text[5];
    public Texture2D HotbarItemBG;
    public static int itemSelected;

    static Inventory reference;

    HotbarHover HH;

    private void Awake()
    {
        if (reference == null)
        {
            reference = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this);

        HH = GameObject.FindFirstObjectByType<HotbarHover>().GetComponent<HotbarHover>();
    }
    private void Start()
    {
        for(int i = 0; i < currentInventory.Length; i++) UpdateInventory(i);
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
            if (currentInventory[i].hasOverworldUses)
                currentInventory[i].cooldownLeft -= Time.deltaTime;
    }

    public void AddItem(InventoryItem itemToAdd)
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
                    return;
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
                return;
            }
        }
        Debug.Log("Inventory full, from " + this);
    }
    public bool InventoryHasSpace()
    {
        for (int i = 0; i < currentInventory.Length; i++)
            if (currentInventory[i].slotIsEmty) return true;
        return false;
    }
     public void RemoveItem(int index)
    {
        currentInventory[index] = new InventoryItem { slotIsEmty = true };
        UpdateInventory(index);
    }
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
            currentInventory[index].itemSprite = RuntimePreviewGenerator.GenerateModelPreview(currentInventory[index].itemModel.transform, 256, 256, false, true);

            currentInventory[index].itemSprite = SpriteEditor.AddOutline(currentInventory[index].itemSprite);

            if (!currentInventory[index].hasOverworldUses && !GameManager.isInBattle)
                currentInventory[index].itemSprite = SpriteEditor.MakeGrayScale(currentInventory[index].itemSprite);
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

