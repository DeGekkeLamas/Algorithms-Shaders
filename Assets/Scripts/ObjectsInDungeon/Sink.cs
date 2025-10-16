using InventoryStuff;
using UnityEngine;

public class Sink : MonoBehaviour, IInteractible
{
    public InventoryItemData waterGlass;
    public void OnInteract()
    {
        Inventory.instance.AddItem(waterGlass.GetItem());
    }
}
