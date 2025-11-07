using UnityEngine;

public class OnDeathSpawnSplat : EntityDeathAction
{
    public GameObject splat;

    protected override void OnDeath()
    {
        Physics.Raycast(this.transform.position + Vector3.up, Vector3.down, out RaycastHit hitInfo);
        Instantiate(splat, hitInfo.point + new Vector3(0, 0.01f, 0), Quaternion.identity, Projectile.projectileContainer.transform);
    }
}
