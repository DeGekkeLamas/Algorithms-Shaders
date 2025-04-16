using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Tooltip("Leave empty to not become any preset")]
    public string itemPresetName;
    public InventoryItem itemToGive;
    public GameObject placeholderModel;

    private void Start()
    {
        if (itemPresetName != string.Empty )
        {
            if (ItemPresets.presets.ContainsKey(itemPresetName))
                itemToGive = ItemPresets.presets[itemPresetName];
            else Debug.LogWarning("This item doesnt exist dumbass");
        }
        GameObject spawned; 
        if (itemToGive.itemModel != null) spawned = Instantiate(itemToGive.itemModel, this.transform);
        else spawned = Instantiate(placeholderModel, this.transform);
        spawned.tag = this.tag;
        spawned.layer = this.gameObject.layer;
    }
}
