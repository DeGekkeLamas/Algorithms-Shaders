using UnityEngine;
using InventoryStuff;
using Unity.VisualScripting;

public class PickupItem : MonoBehaviour, IInteractible
{
    [Tooltip("Leave empty to not become any preset")]
    public InventoryItem itemPreset;
    public InventoryItemData itemToGive;
    public GameObject placeholderModel;
    static GameObject staticPlaceholderModel;
    private void Awake()
    {
        if (staticPlaceholderModel == null && placeholderModel == null)
            staticPlaceholderModel = placeholderModel;
    }

    private void Start()
    {
        if (itemPreset != null )
        {
                itemToGive = itemPreset.GetItem();
        }
        GameObject spawned; 
        if (itemToGive.itemModel != null) spawned = Instantiate(itemToGive.itemModel, this.transform);
        else spawned = Instantiate(staticPlaceholderModel, this.transform);
        spawned.tag = this.tag;
        spawned.layer = this.gameObject.layer;
    }
    public void OnInteract()
    {
        Pickup();
    }

    public void Pickup()
    {
        bool couldAdd = Inventory.instance.AddItem(itemToGive);
        if (couldAdd) Destroy(this.gameObject);
    }

    public static PickupItem SpawnPickup(InventoryItemData item, Transform source, Vector3 offset = new())
    {
        GameObject obj = new("ItemPickup");
        PickupItem pickup = obj.AddComponent<PickupItem>();
        pickup.AddComponent<BoxCollider>().size = Vector3.one * .5f;
        SphereCollider sphere = pickup.AddComponent<SphereCollider>();
        sphere.isTrigger = true;
        sphere.radius = .75f;
        pickup.AddComponent<Rigidbody>();
        pickup.itemToGive = item;
        // Set transform
        pickup.transform.parent = source;
        pickup.transform.position = source.position + offset;

        return pickup;
    }
}
