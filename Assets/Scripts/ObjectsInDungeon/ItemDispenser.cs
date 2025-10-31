using InventoryStuff;
using UnityEngine;

/// <summary>
/// Gives the player an item but does not destroy itself, allowing infinite uses
/// </summary>
public class ItemDispenser : MonoBehaviour, IInteractible
{
    public InventoryItemData toGive;
    public void OnInteract()
    {
        Inventory.instance.AddItem(toGive.GetItem());
    }
}
