using InventoryStuff;
using UnityEngine;

namespace Entities.Player
{
    public class DropHeldItemOnEPressed : MonoBehaviour
    {
        [SerializeField] Rigidbody pickupSpawned;
        [SerializeField] Transform forwardReference;

        void Update()
        {
            // Drop currently held item
            if (Input.GetKeyDown(KeyCode.E)) DropHeldItem();
        }
        void DropHeldItem()
        {
            InventoryItem itemSelected = Inventory.instance.currentInventory[Inventory.itemSelected].item;
            if (itemSelected != null)
            {
                Debug.Log($"Dropped {itemSelected.itemName}, from {this}");
                Rigidbody droppedItem = Instantiate(pickupSpawned, this.transform.position + forwardReference.forward, Quaternion.identity);
                droppedItem.gameObject.GetComponent<PickupItem>().itemToGive = Inventory.instance.currentInventory[Inventory.itemSelected].item;
                if (itemSelected.IsStackable) Inventory.instance.RemoveFromStack(Inventory.itemSelected);
                else Inventory.instance.RemoveItem(Inventory.itemSelected);
            }

        }
    }
}
