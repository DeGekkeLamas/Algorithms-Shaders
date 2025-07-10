using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Tooltip("Leave empty to not become any preset")]
    public InventoryItem itemPreset;
    public InventoryItemData itemToGive;
    public GameObject placeholderModel;

    private void Start()
    {
        if (itemPreset != null )
        {
                itemToGive = itemPreset.item;
        }
        GameObject spawned; 
        if (itemToGive.itemModel != null) spawned = Instantiate(itemToGive.itemModel, this.transform);
        else spawned = Instantiate(placeholderModel, this.transform);
        spawned.tag = this.tag;
        spawned.layer = this.gameObject.layer;
    }
}
