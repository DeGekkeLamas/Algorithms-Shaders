using System.Collections;
using UnityEngine;

namespace Entities.Enemies
{
    public class AttackCycleProjectile : AttackCycle
    {
        public Projectile projectile;
        public Transform projectileOrigin;

        public override IEnumerator Attack(Enemy source)
        {
            float originalSpeed = source.moveSpeed;
            source.ChangeMoveSpeed(0);
            // Wait for animation to finish
            yield return source.Animator.WaitForAnimation("Attack");

            // Do attack
            Projectile spawned = Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation, Projectile.projectileContainer);
            spawned.GetComponent<OnTriggerDamageEntity>().damage = source.strength;
            Rigidbody rigidbody = spawned.GetComponent<Rigidbody>();
            rigidbody.AddForce(spawned.projectileSpeed * transform.forward);
            rigidbody.angularVelocity = Vector3.Cross(transform.forward, Vector3.up) * -projectile.rotationIntensity;

            yield return null;
            yield return source.Animator.WaitForCurrentAnimation();
            source.ChangeMoveSpeed(originalSpeed);
        }
    }
}
