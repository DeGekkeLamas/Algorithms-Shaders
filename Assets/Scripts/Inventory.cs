using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

[System.Serializable]
public struct InventoryItem
{
    public string itemName;
    public Texture2D itemSprite;
    public GameObject itemModel;
    public string toolTip;
    public bool isStackable;
    public int maxStack ;
    public int amountLeft;
    [Header("Stats")]
    public float durability;
    public float currentDurability;
    public float damage;
    public float hpHealed;
    public int amountOfTargets;
    public bool targetAll;
    public int amountOfHits;
    [Header("Properties")]
    public bool slotIsEmty;
    public bool isConsumedOnUse;
    public bool isFood;
    public bool isMetal;
    public bool isKnife;
    public bool isEmptyMicrowave;
    public bool inflictOnFire;
    public bool inflictPoisoned;
    public bool inflictBlindness;
    public bool inflictFakeBlood;
    public bool onAttackHealHP;
    public bool inflictStuck;
    public bool damageScalesWithHP;
    public bool knifeBoost;
    public bool foodBoost;
    public bool foodResistanceBoost;
    public bool fireImmunity;
    public bool seeEnemyInventories;
    public bool durabilityBoost;
    public int healingBoost;
    public bool consumesBread;
    public bool curesOnFireWhenConsumed;
    public float damageAfterBlock;
    public bool grantsImmortality;
    public bool isStoveIngredient;
    [Header("Buttons")]
    public bool canAttack;
    public bool canBlock;
    public bool canConsume;
    public bool canThrow;
    [Header("Overworld properties")]
    public bool hasOverworldUses;
    public GameObject projectile;
}
public class Inventory : MonoBehaviour
{
    public InventoryItem[] currentInventory = new InventoryItem[5];

    [Header("UI")]
    public Image[] Hotbar = new Image[5];
    public TMP_Text[] quantityCounters = new TMP_Text[5];
    public Texture2D HotbarItemBG;

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
    private void Start() => UpdateInventory();
    void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            float _direction = (Input.mouseScrollDelta.y > 0) ? -1 : 1;
            HotbarHover.itemSelected += Mathf.RoundToInt(_direction);
            if (HotbarHover.itemSelected < 0) HotbarHover.itemSelected = 4;
            if (HotbarHover.itemSelected > 4) HotbarHover.itemSelected = 0;
            HH.SetSelectedBorder(HotbarHover.itemSelected);
        }
    }

    public void AddItem(InventoryItem itemToAdd)
    {
        if (itemToAdd.isStackable)
        {
            for (int i = 0; i < currentInventory.Length; i++)
            {
                if (currentInventory[i].isStackable && currentInventory[i].amountLeft < currentInventory[i].maxStack)
                {
                    currentInventory[i].amountLeft++;
                    UpdateInventory();
                    return;
                }
            }
        }
        for (int i = 0; i < currentInventory.Length; i++)
        {
            if (currentInventory[i].slotIsEmty)
            {
                currentInventory[i] = itemToAdd;
                UpdateInventory();
                Debug.Log("Added " + itemToAdd.itemName + ", from " + this);
                return;
            }
        }
        Debug.Log("Inventory full, from " + this);
    }
     public void RemoveItem(int index)
    {
        currentInventory[index] = new InventoryItem { slotIsEmty = true };
        UpdateInventory();
    }

    [ContextMenu("Update inventory")]
    public void UpdateInventory()
    {
        for (int i = 0; i < currentInventory.Length; i++)
        {
            if (currentInventory[i].itemModel != null)
            {
                currentInventory[i].itemSprite = RuntimePreviewGenerator.GenerateModelPreview(currentInventory[i].itemModel.transform, 256, 256, false, true);
                
                currentInventory[i].itemSprite = SpriteEditor.AddOutline(currentInventory[i].itemSprite);

                if (!currentInventory[i].hasOverworldUses && !GameManager.isInBattle)
                    currentInventory[i].itemSprite = SpriteEditor.MakeGrayScale(currentInventory[i].itemSprite);
            }
            else currentInventory[i].itemSprite = HotbarItemBG;

            Hotbar[i].sprite = Sprite.Create(currentInventory[i].itemSprite, new Rect(0.0f, 0.0f, 
                currentInventory[i].itemSprite.width, currentInventory[i].itemSprite.height), new Vector2(0, 0));

            HH.SetSelectedBorder(HotbarHover.itemSelected);

            if (currentInventory[i].isStackable)
            {
                quantityCounters[i].gameObject.SetActive(true);
                quantityCounters[i].text = currentInventory[i].amountLeft.ToString();
            }
            else quantityCounters[i].gameObject.SetActive(false);
        }
    }

    [ContextMenu("Test add item)")]
    void AddItemTest() => AddItem(ItemPresets.presets["CryingPan"]);

    [ContextMenu("Test remove item)")]
    void RemoveItemTest() => RemoveItem(0);
}

