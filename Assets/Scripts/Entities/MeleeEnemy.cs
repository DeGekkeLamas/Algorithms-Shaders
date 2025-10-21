using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class MeleeEnemy : AttackingEnemy
{
    public float maxAttackDistance = 3;

    protected override void Attack()
    {
        if (PlayerInRange(maxAttackDistance))
        {
            PlayerController.instance.DealDamage(strength);
        }
    }
    protected override void VisualDebug()
    {
        base.VisualDebug();
        DebugExtension.DebugCircle(this.transform.position, Color.red, maxAttackDistance);
        Debug.DrawRay(transform.position, VectorMath.RotateVectorXZ(this.transform.forward * maxAttackDistance, attackAngle), Color.red);
        Debug.DrawRay(transform.position, VectorMath.RotateVectorXZ(this.transform.forward * maxAttackDistance, -attackAngle), Color.red);
    }
}
