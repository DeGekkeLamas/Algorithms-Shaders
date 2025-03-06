using UnityEngine;

public class OtherInventoryChecker : MonoBehaviour
{
    public string key;

    [ContextMenu("PrintItems")]
    void PrintItems() => OtherInventorySystem.PrintInventory();
    [ContextMenu("AddItem")]
    void AddItem() => OtherInventorySystem.AddItem(key);
    [ContextMenu("RemoveItem")]
    void RemoveItem() => OtherInventorySystem.RemoveItem(key);
    [ContextMenu("ItemExists")]
    void ItemExists() => Debug.Log(OtherInventorySystem.ItemExists(key));
}
