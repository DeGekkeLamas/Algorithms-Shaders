using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using InventoryStuff;

public class HotbarHover : MonoBehaviour
{
    [SerializeField] int itemNumber;
    private void OnValidate() => itemNumber = Mathf.Clamp(itemNumber, 1, 5);

    public TMP_Text itemname;
    public TMP_Text description;
    public GameObject[] selectedBorders = new GameObject[5];

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

        if (Inventory.instance.currentInventory[index] != null)
        {
            itemname.text = Inventory.instance.currentInventory[index].itemName;
            description.text = ItemDescription.GenerateDescription(Inventory.instance.currentInventory[index]);
        }
        else
        {
            itemname.text = "Empty";
            description.text = string.Empty;
        }
    }
}
