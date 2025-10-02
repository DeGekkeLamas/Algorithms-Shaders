using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class HotbarHover : MonoBehaviour
{
    [SerializeField] int itemNumber;
    private void OnValidate() => itemNumber = Mathf.Clamp(itemNumber, 1, 5);

    Inventory inventory;
    public TMP_Text itemname;
    public TMP_Text description;
    public GameObject[] selectedBorders = new GameObject[5];

    private void Awake() => inventory = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Inventory>();
    public void MouseHover()
    {
        //Debug.Log("Mouse over " + this);
        Inventory.itemSelected = itemNumber - 1;
        SetSelectedBorder(itemNumber - 1);
    }

    public void SetSelectedBorder(int index)
    {
        foreach (var border in selectedBorders) border.SetActive(false);
        selectedBorders[index].SetActive(true);

        if (!inventory.currentInventory[index].slotIsEmty)
        {
            itemname.text = inventory.currentInventory[index].itemName;
            description.text = CreateItemDescription(index);
        }
        else
        {
            itemname.text = "Empty";
            description.text = string.Empty;
        }
    }

    string CreateItemDescription(int index)
    {
        string _description = string.Empty;
        if (inventory.currentInventory[index].damage != 0) 
            _description += $" \nDeals {inventory.currentInventory[index].damage} damage";
        if (inventory.currentInventory[index].amountOfTargets > 1)
            _description += $" to {inventory.currentInventory[index].amountOfTargets} enemies";
        if (inventory.currentInventory[index].damageScalesWithHP) _description += ", deals more damage to damaged enemies";

        if (inventory.currentInventory[index].hpHealed != 0) 
            _description += $"\nHeals {inventory.currentInventory[index].hpHealed} HP when consumed";
        if (inventory.currentInventory[index].curesOnFireWhenConsumed) 
            _description += $"\nRemoves fire when consumed";

        if (inventory.currentInventory[index].canBlock) _description += $"\nCan be used to block attacks";
        if (inventory.currentInventory[index].damageAfterBlock > 0) 
            _description += $"\nDeals {inventory.currentInventory[index].damageAfterBlock} to the player after blocking";

        foreach(StatusEffect effect in inventory.currentInventory[index].effectApplied)
        {
            _description += $"\nInflicts {effect.name}";
        }

        if (inventory.currentInventory[index].durabilityBoost) _description += $"\nWhile in inventory, increases durability of items";
        if (inventory.currentInventory[index].foodBoost) _description += $"\nWhile in inventory, increases damage of food-based items";
        if (inventory.currentInventory[index].foodResistanceBoost) _description += $"\nWhile in inventory, increases defence against food-based items";
        if (inventory.currentInventory[index].knifeBoost) _description += $"\nWhile in inventory, increases damage of knifes and their upgrades";
        if (inventory.currentInventory[index].healingBoost > 0) 
            _description += $"\n While in inventory, healing items heal {inventory.currentInventory[index].healingBoost} more HP";
        if (inventory.currentInventory[index].seeEnemyInventories) _description += $"\nWhile in inventory, allows you to see the inventories of enemies";
        if (inventory.currentInventory[index].fireImmunity) _description += $"\nWhile in inventory, grants immunity to fire";
        if (inventory.currentInventory[index].grantsImmortality) _description += $"\nWhile in inventory, grants immunity to attacks";
        if (inventory.currentInventory[index].consumesBread) _description += $"\nUses bread as ammunition";

        if (inventory.currentInventory[index].isStoveIngredient) _description += $"\nCan be used as ingredient for crafting at stoves";

        _description += $" \n\"{inventory.currentInventory[index].toolTip}\"";
        return _description;
    }
}
