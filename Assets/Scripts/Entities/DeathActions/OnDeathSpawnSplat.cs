using UnityEngine;

namespace Entities
{
    /// <summary>
    /// Spawns object on floor when bound entity dies
    /// </summary>
    public class OnDeathSpawnSplat : EntityDeathAction
    {
        public GameObject splat;

        protected override void OnDeath()
        {
            // Spawn at floor
            Physics.Raycast(this.transform.position + Vector3.up, Vector3.down, out RaycastHit hitInfo, 10, LayerMask.GetMask("Terrain"));
            Instantiate(splat, hitInfo.point + new Vector3(0, 0.01f, 0), Quaternion.identity);
        }
    }
}
