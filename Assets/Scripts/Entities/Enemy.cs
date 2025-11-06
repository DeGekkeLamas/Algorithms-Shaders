using System.Collections;
using TMPro;
using UnityEngine;
using InventoryStuff;
using NaughtyAttributes;

public class Enemy : Entity
{
    public float xpToGive = 10;
    [Header("Item drops")]
    [ReadOnly] public int totalDropChance;
    public ItemLootTable lootTable;
    public GameObject corpse;
    public GameObject tomatoSplat;

    protected virtual void OnValidate()
    {
        totalDropChance = ItemLootDrop<InventoryItemData>.GetTotalItemProbability(lootTable.lootTable);
        ItemLootDrop<InventoryItemData>.MassValidate(lootTable.lootTable);
    }

    /// <summary>
    /// Spawns corpse and lootdrops, also destroys object
    /// </summary>
    protected override void Death()
    {
        base.Death();
        PlayerController.instance.AddXP(xpToGive);
        SpawnCorpse();
        SpawnDrops();
        StopAllCoroutines();
        Destroy(this.gameObject);
    }

    void SpawnCorpse()
    {
        GameObject oldModel = this.transform.GetChild(0).gameObject;
        Instantiate(corpse, oldModel.transform.position, oldModel.transform.rotation, this.transform.parent);
        Destroy(oldModel);
        Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hitInfo);
        Instantiate(tomatoSplat, hitInfo.point + new Vector3(0, 0.01f, 0), Quaternion.identity, Projectile.projectileContainer.transform);
    }

    void SpawnDrops()
    {
        InventoryItemData itemDropped = ItemLootDrop<InventoryItemData>.GetItemFromLoottable(lootTable.lootTable);
        if (itemDropped != null && itemDropped != null)
        {
            Debug.Log($"{entityName} dropped {itemDropped.GetItem().itemName}");
            PickupItem pickup = PickupItem.SpawnPickup(itemDropped.GetItem(), this.transform, Vector3.up);
            pickup.transform.parent = this.transform.parent;
        }
    }
}
