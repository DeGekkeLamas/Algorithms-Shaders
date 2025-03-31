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
    public Rigidbody projectile;
    public bool autoFire;
    public float cooldown;
    public float cooldownLeft;
}
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
                if (currentInventory[i].isStackable && currentInventory[i].amountLeft < currentInventory[i].maxStack)
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

