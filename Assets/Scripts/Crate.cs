using UnityEngine;

public class Crate : MonoBehaviour
{
    public string[] itemsToGive;
    public GameObject pickupSpawned;

    public int boxHP;

    public void SubtractHP(int hpToRemove)
    {
        boxHP -= hpToRemove;
        Debug.Log($"Dealt {hpToRemove} damage, from {this}");
        if (boxHP <= 0) DestroyBox();
    }

    void DestroyBox()
    {
        foreach (var item in itemsToGive)
        {
            if (!ItemPresets.presets.ContainsKey(item))
            {
                Debug.LogWarning($"This item doesnt exist dumbass, from {this}");
                continue;
            }
            Instantiate(pickupSpawned, transform.position, Quaternion.identity).
                GetComponent<PickupItem>().itemToGive = ItemPresets.presets[item];
            Debug.Log($"Spawned {item}, from {this}");
        }
        Destroy(this.gameObject);
    }
}
