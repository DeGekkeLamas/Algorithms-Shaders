using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public Texture2D itemSprite;
    public GameObject itemModel;
    public string toolTip;
    public bool isStackable;
    public int maxStack = 1;
    public int amountLeft = 1;
    [Header("Stats")]
    public float durability;
    public float currentDurability;
    public float damage;
    public float hpHealed;
    public int amountOfTargets = 1;
    public bool targetAll;
    public int amountOfHits = 1;
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
}
public class Inventory : MonoBehaviour
{
    public InventoryItem[] currentInventory = new InventoryItem[5];
    public Image[] Hotbar = new Image[5];
    public TMP_Text[] quantityCounters = new TMP_Text[5];
    public Texture2D HotbarItemBG;

    ItemPresets presets;
    HotbarHover HH;
    private void Start()
    {
        presets = this.GetComponent<ItemPresets>();
        HH = GameObject.FindFirstObjectByType<HotbarHover>().GetComponent<HotbarHover>();
        UpdateInventory();
    }
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
            foreach (var item in currentInventory)
            {
                if (item.isStackable && item.amountLeft < item.maxStack)
                {
                    item.amountLeft++;
                    UpdateInventory();
                    return;
                }
            }
        }
        for (int i = 0; i < currentInventory.Length; i++)
        {
            if (currentInventory[i].slotIsEmty)
            {
                currentInventory[i] = ClonePreset(itemToAdd);
                UpdateInventory();
                Debug.Log("Added " + itemToAdd.itemName + ", from " + this);
                return;
            }
        }
        Debug.Log("Inventory full, from " + this);
    }
    InventoryItem ClonePreset(InventoryItem itemToAdd)
    {
        InventoryItem _clonedItem = new InventoryItem
        {
            slotIsEmty = false,
            itemModel = itemToAdd.itemModel, 
            itemName = itemToAdd.itemName,
            isConsumedOnUse = itemToAdd.isConsumedOnUse,
            isEmptyMicrowave = itemToAdd.isEmptyMicrowave,
            isFood = itemToAdd.isFood,
            isKnife = itemToAdd.isKnife,
            isMetal = itemToAdd.isMetal,
            isStackable = itemToAdd.isStackable,
            isStoveIngredient = itemToAdd.isStoveIngredient,
            inflictStuck = itemToAdd.inflictStuck,
            itemSprite = itemToAdd.itemSprite,
            fireImmunity = itemToAdd.fireImmunity,
            damage = itemToAdd.damage,
            damageAfterBlock = itemToAdd.damageAfterBlock,
            damageScalesWithHP = itemToAdd.damageScalesWithHP,
            durability = itemToAdd.durability,
            durabilityBoost = itemToAdd.durabilityBoost,
            amountLeft = itemToAdd.amountLeft,
            amountOfHits = itemToAdd.amountOfHits,
            amountOfTargets = itemToAdd.amountOfTargets,
            canAttack = itemToAdd.canAttack,
            onAttackHealHP = itemToAdd.onAttackHealHP,
            targetAll = itemToAdd.targetAll,
            canBlock = itemToAdd.canBlock,
            canConsume = itemToAdd.canConsume,
            canThrow = itemToAdd.canThrow,
            consumesBread = itemToAdd.consumesBread,
            curesOnFireWhenConsumed = itemToAdd.curesOnFireWhenConsumed,
            foodBoost = itemToAdd.foodBoost,
            foodResistanceBoost = itemToAdd.foodResistanceBoost,
            grantsImmortality = itemToAdd.grantsImmortality,
            healingBoost = itemToAdd.healingBoost,
            hpHealed = itemToAdd.hpHealed,
            inflictBlindness = itemToAdd.inflictBlindness,
            inflictFakeBlood = itemToAdd.inflictFakeBlood,
            inflictOnFire = itemToAdd.inflictOnFire,
            inflictPoisoned = itemToAdd.inflictPoisoned,
            seeEnemyInventories = itemToAdd.seeEnemyInventories,
            knifeBoost = itemToAdd.knifeBoost,
            maxStack = itemToAdd.maxStack,
            toolTip = itemToAdd.toolTip,
        };
        if (itemToAdd.currentDurability != 0) _clonedItem.currentDurability = itemToAdd.currentDurability;
        else _clonedItem.currentDurability = itemToAdd.durability;
        //object _objWithValues = System.Activator.CreateInstance(itemToAdd.GetType());
        //_clonedItem = _objWithValues as InventoryItem;

        return _clonedItem;
    }
     public void RemoveItem(int index)
    {
        currentInventory[index] = new InventoryItem { slotIsEmty = true };
        UpdateInventory();
    }

    [ContextMenu("Update inventory")]
    void UpdateInventory()
    {
        for (int i = 0; i < currentInventory.Length; i++)
        {
            if (currentInventory[i].itemModel != null)
            {
                currentInventory[i].itemSprite = RuntimePreviewGenerator.GenerateModelPreview(currentInventory[i].itemModel.transform, 256, 256, false, true);
                currentInventory[i].itemSprite = SpriteEditor.AddOutline(currentInventory[i].itemSprite);
                //currentInventory[i].itemSprite = SpriteEditor.AddOutline(currentInventory[i].itemSprite);
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
    void AddItemTest() => AddItem(presets.cryingPan);

    [ContextMenu("Test remove item)")]
    void RemoveItemTest() => RemoveItem(0);
}

