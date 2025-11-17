using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using Entities.Player;
using MovementStuff;

namespace Entities.Enemies
{
    public class AttackingEnemy : Enemy
    {
        [Header("Attack distance")]
        [InfoBox("maxAttackDistance is how far away the target will still receive damage, " +
            "attackStartDistance is from how far away the enemy will start attacking")]
        public float attackStartDistance = 4;
        public float attackAngle = 75;
        public AttackCycle attackToDo;

        public bool showVisualDebug;
        bool isAttacking;

        protected void OnValidate()
        {
            if (showVisualDebug) StartCoroutine(ShowVisualDebug());
        }

        protected override void Awake()
        {
            base.Awake();
        }

        private void Update()
        {
            if (!isAttacking && PlayerInRange(attackStartDistance)) StartCoroutine(AttackCycle());
        }

        protected bool PlayerInRange(float maxDst)
        {
            return TargetSight.CanSeeTarget(this.transform, PlayerController.instance.transform, maxDst, attackAngle);
        }
        IEnumerator AttackCycle()
        {
            //Debug.Log("Started attack");
            isAttacking = true;
            yield return attackToDo.Attack(this);
            isAttacking = false;
        }

        IEnumerator ShowVisualDebug()
        {
            while (showVisualDebug)
            {
                VisualDebug();
                yield return null;
            }
        }

        /// <summary>
        /// shows vision range
        /// </summary>
        protected virtual void VisualDebug()
        {
            DebugExtension.DebugCircle(this.transform.position, Color.red, attackStartDistance);
            Debug.DrawRay(transform.position, VectorMath.RotateVectorXZ(this.transform.forward * attackStartDistance, attackAngle), Color.red);
            Debug.DrawRay(transform.position, VectorMath.RotateVectorXZ(this.transform.forward * attackStartDistance, -attackAngle), Color.red);
        }
    }
}
