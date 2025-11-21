using Entities;
using Entities.StatusEffects;
using MovementStuff;
using UnityEngine;
using static UnityEditor.Progress;

namespace InventoryStuff
{
    [CreateAssetMenu(
        fileName = "RangedItem",
        menuName = "ScriptableObjects/Items/RangedItem",
        order = 0)]
    public class RangedWeaponData : InventoryItemData
    {
        public RangedWeapon item = new();
        public override InventoryItem GetItem() { return item; }
    }

    [System.Serializable]
    public class RangedWeapon : InventoryItem
    {
        [Header("Type specific")]
        public float damage;
        public StatusEffect[] effectApplied;
        public Projectile projectile;
        public bool autoFire;
        public float cooldown;
        public bool isConsumedOnUse;
        [Tooltip("Leave empty to use no fuel")]
        public InventoryItemData fuel;

        [HideInInspector] public float cooldownLeft;

        public override void UseItem(Entity source, Vector3 inputDir)
        {
            if (cooldownLeft <= 0)
            {
                // Fuel
                if (fuel != null)
                {
                    if (!Inventory.instance.Contains(fuel.GetItem())) return;

                    Inventory.instance.RemoveFromStack(fuel.GetItem());
                }
                // Spawn projectie
                Projectile spawnedProjectile = MonoBehaviour.Instantiate(this.projectile,
                    source.transform.position + new Vector3(0, 1, 0), Quaternion.LookRotation(inputDir), Projectile.projectileContainer);
                Rigidbody rigidbody = spawnedProjectile.GetComponent<Rigidbody>();
                rigidbody.excludeLayers = rigidbody.excludeLayers + (int)Mathf.Pow(2, source.gameObject.layer);
                projectile.GetComponent<OnTriggerDamageEntity>().damage = damage * source.strength;
                if (!projectile.useGravity)
                { // Straight projectile
                    rigidbody.linearVelocity = inputDir.normalized *
                        projectile.projectileSpeed;
                    rigidbody.linearVelocity = new(rigidbody.linearVelocity.x, 0, rigidbody.linearVelocity.z);
                }
                else // Lobbed projectile
                {
                    rigidbody.linearVelocity = inputDir *
                        projectile.projectileSpeed;
                    rigidbody.angularVelocity = Vector3.Cross(rigidbody.linearVelocity, Vector3.up) * -projectile.rotationIntensity;
                }

                cooldownLeft += cooldown;

                if (isConsumedOnUse)
                    Inventory.instance.RemoveFromStack(Inventory.itemSelected);

                Debug.Log($"Spawned projectile, from {source.entityName}");
            }
        }
        public override void UpdateAction()
        {
            cooldownLeft -= Time.deltaTime;
            canUseItem = cooldownLeft <= 0;
        }

        public override string GetItemDescription()
        {
            string description = base.GetItemDescription();

            // Damage
            description += $"Deals {damage} {(!projectile.useGravity ? "ranged" : "lobbed")} damage.\n";
            // fuel
            if (fuel != null) description += $"Uses {fuel.GetItem().itemName} as fuel.\n";
            // Onetime use
            if (isConsumedOnUse) description += "Single use.\n";

            return description;
        }
    }
}