using InventoryStuff;
using UnityEngine;

public class Sink : MonoBehaviour, IInteractible
{
    public InventoryItem waterGlass;
    public void OnInteract()
    {
        Inventory.instance.AddItem(waterGlass.GetItem());
    }
}
