using UnityEngine;

namespace InventoryStuff
{
    [CreateAssetMenu(
        fileName = "RangedItem",
        menuName = "ScriptableObjects/Items/RangedItem",
        order = 0)]
    public class RangedWeaponData : InventoryItem
    {
        public RangedWeapon item = new();
    }

    [System.Serializable]
    public class RangedWeapon : InventoryItemData
    {
        [Header("Type specific")]
        public float damage;
        public StatusEffect[] effectApplied;
        public Rigidbody projectile;
        public bool autoFire;
        public float cooldown;
        public float cooldownLeft;
        public bool isConsumedOnUse;
        [Tooltip("Leave empty to use no fuel")]
        public InventoryItemData fuel;

        public override void UseItem(Entity source, Vector3 inputDir)
        {
            if (cooldownLeft <= 0)
            {
                Rigidbody spawnedProjectile = MonoBehaviour.Instantiate(this.projectile,
                    source.transform.position + new Vector3(0, 1, 0), Quaternion.LookRotation(inputDir));
                Projectile projectile = spawnedProjectile.GetComponent<Projectile>();
                projectile.damage = damage;
                if (!projectile.useGravity)
                { // Straight projectile
                    spawnedProjectile.linearVelocity = inputDir.normalized *
                        projectile.projectileSpeed;
                    spawnedProjectile.linearVelocity = new(spawnedProjectile.linearVelocity.x, 0, spawnedProjectile.linearVelocity.z);
                }
                else // Lobbed projectile
                {
                    spawnedProjectile.linearVelocity = inputDir *
                        projectile.projectileSpeed;
                    spawnedProjectile.angularVelocity = Vector3.Cross(spawnedProjectile.linearVelocity, Vector3.up) * -projectile.rotationIntensity;
                }

                cooldownLeft += cooldown;

                if (isConsumedOnUse)
                    Inventory.instance.RemoveFromStack(Inventory.itemSelected);

                Debug.Log($"Spawned projectile, from {this}");
            }
        }
    }
}