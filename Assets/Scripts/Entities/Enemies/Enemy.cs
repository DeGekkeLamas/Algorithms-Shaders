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
    protected AnimationController anim;
    public AnimationController Animator => anim;

    protected virtual void OnValidate()
    {
        totalDropChance = ItemLootDrop<InventoryItemData>.GetTotalItemProbability(lootTable.lootTable);
        ItemLootDrop<InventoryItemData>.MassValidate(lootTable.lootTable);
    }

    protected override void Awake()
    {
        anim = GetComponent<AnimationController>();
        base.Awake();
    }

    /// <summary>
    /// Spawns lootdrops, also destroys object
    /// </summary>
    protected override void Death()
    {
        base.Death();

        PlayerController.instance.AddXP(xpToGive);
        SpawnDrops();
        StopAllCoroutines();
        Destroy(this.gameObject);
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
