using System.Collections;
using UnityEngine;
using Entities.Player;
using MovementStuff;

namespace Entities.Enemies
{
    /// <summary>
    /// Melee attack cycle
    /// </summary>
    public class AttackCycleAreaSlam : AttackCycle
    {
        public float attackRange = 4;
        public float attackAngle = 75;
        public bool showDebug;
        private void OnValidate()
        {
            if (showDebug) StartCoroutine(ShowDebug());

            //HasAnimationCheck("Attack");
        }

        public override IEnumerator Attack(Enemy source)
        {
            float originalSpeed = source.moveSpeed;
            source.ChangeMoveSpeed(0);
            yield return source.Animator.WaitForAnimation("Attack");
            // Deal damage
            InvokeOnAttackDone(attackRange);
            if (TargetSight.TargetIsInRange(source.transform, PlayerController.instance.transform, attackRange, attackAngle))
            {
                PlayerController.instance.DealDamage(source.strength);
            }
            yield return null;
            yield return source.Animator.WaitForCurrentAnimation();
            source.ChangeMoveSpeed(originalSpeed);
        }

        IEnumerator ShowDebug()
        {
            while (showDebug)
            {
                DebugExtension.DebugCircle(this.transform.position, Color.red, attackRange);
                yield return null;
            }
        }
    }
}
