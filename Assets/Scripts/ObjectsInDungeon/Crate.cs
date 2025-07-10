using UnityEngine;

public class Crate : MonoBehaviour
{
    public InventoryItem[] itemsToGive;
    public GameObject pickupSpawned;

    public int boxHP;

    public void SubtractHP(int hpToRemove)
    {
        boxHP -= hpToRemove;
        Debug.Log($"Dealt {hpToRemove} damage to crate");
        if (boxHP <= 0) DestroyBox();
    }

    void DestroyBox()
    {
        foreach (var item in itemsToGive)
        {
            Instantiate(pickupSpawned, transform.position, Quaternion.identity).
                GetComponent<PickupItem>().itemToGive = item.item;
            Debug.Log($"Spawned {item}, from {this}");
        }
        Destroy(this.gameObject);
    }
}
