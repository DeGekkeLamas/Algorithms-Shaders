using InventoryStuff;
using TMPro;
using UnityEngine;

public class ItemDescriptionPresenter : MonoBehaviour
{
    public TMP_Text descrText;
    void Start()
    {
        Inventory.instance.onItemChanged += UpdateText;
        Inventory.instance.onSeletecItemSwitched += UpdateText;
        UpdateText();
    }

    void UpdateText()
    {
        InventoryItem item = Inventory.instance.currentInventory[Inventory.itemSelected].item;
        descrText.text = ItemDescription.GenerateDescription(item);
    }
}
