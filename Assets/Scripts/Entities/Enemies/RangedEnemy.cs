using UnityEngine;

public class RangedEnemy : AttackingEnemy
{
    public Projectile projectile;
    public Transform projectileOrigin;
    protected override void Attack()
    {
        Projectile spawned = Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation, Projectile.projectileContainer);
        spawned.damage = strength;
        Rigidbody rigidbody = spawned.GetComponent<Rigidbody>();
        rigidbody.AddForce(spawned.projectileSpeed * transform.forward);
        rigidbody.angularVelocity = Vector3.Cross(transform.forward, Vector3.up) * -projectile.rotationIntensity;
    }

    protected override void VisualDebug()
    {
        DebugExtension.DebugArrow(projectileOrigin.position, -projectileOrigin.forward, Color.magenta, .02f);
        base.VisualDebug();
    }
}
