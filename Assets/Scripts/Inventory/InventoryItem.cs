using UnityEngine;

[CreateAssetMenu]
public class InventoryItem : ScriptableObject
{
    public InventoryItemData item = new InventoryItemData
        {
            maxStack = 1,
            amountLeft = 1,
            amountOfHits = 1,
            amountOfTargets = 1,
        };
}

[System.Serializable]
public struct InventoryItemData
{
    [HideInInspector] public Texture2D itemSprite;
    public string itemName;
    public GameObject itemModel;
    [TextArea] public string toolTip;
    public bool isStackable;
    public int maxStack;
    [HideInInspector] public int amountLeft;
    [Header("Stats")]
    public float durability;
    [HideInInspector] public float currentDurability;
    public float damage;
    public float hpHealed;
    public int amountOfTargets;
    public bool targetAll;
    public int amountOfHits;
    [Header("Properties")]
    [HideInInspector] public bool slotIsEmty;
    public bool isConsumedOnUse;
    public bool isFood;
    public bool isMetal;
    public bool isKnife;
    public bool isEmptyMicrowave;
    public bool inflictOnFire;
    public bool inflictPoisoned;
    public bool inflictBlindness;
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
