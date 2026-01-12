using UnityEngine;
using InventoryStuff;

namespace Entities
{
    /// <summary>
    /// Entity that drops items when killed
    /// </summary>
    public class Crate : Entity
    {
        [SerializeField] InventoryItemData[] itemsToGive;
        [SerializeField] PickupItem pickupSpawned;

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
