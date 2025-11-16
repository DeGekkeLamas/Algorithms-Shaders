using UnityEngine;
using InventoryStuff;

namespace Entities
{
    public class Crate : Entity
    {
        public InventoryItemData[] itemsToGive;
        public PickupItem pickupSpawned;

        protected override void Death()
        {
            foreach (var item in itemsToGive)
            {
                Instantiate(pickupSpawned, transform.position, Quaternion.identity).itemToGive = item.GetItem();
                Debug.Log($"Spawned {item.GetItem().itemName}, from {entityName}");
            }
            Destroy(this.gameObject);
        }
    }
}
