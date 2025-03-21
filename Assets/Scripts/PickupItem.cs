using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Tooltip("Leave empty to not become any preset")]
    public string itemPresetName;
    public InventoryItem itemToGive;

    private void Start()
    {
        if (itemPresetName != string.Empty )
        {
            if (ItemPresets.presets.ContainsKey(itemPresetName))
                itemToGive = ItemPresets.presets[itemPresetName];
            else Debug.LogWarning("This item doesnt exist dumbass");
        }
        Instantiate(itemToGive.itemModel, this.transform).tag = this.tag;
    }
}
