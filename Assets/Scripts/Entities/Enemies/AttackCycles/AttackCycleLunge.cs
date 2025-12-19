using Entities.Player;
using MovementStuff;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;


namespace Entities.Enemies
{
    public class AttackCycleLunge : AttackCycle
    {
        [InfoBox("Leave target empty for it to be automatically set to player instance")]
        Transform target;
        [Header("Jump")]
        public float jumpHeight = 8;
        public float jumpDuration = .5f;
        [Header("Lunge")]
        public float lungeDuration = .5f;
        public float lungeAttackRange = 3;

        public bool showDebug;
        private void OnValidate()
        {
            if (showDebug) StartCoroutine(ShowDebug());

            //HasAnimationCheck("Attack");
        }
        private void Start()
        {
            if (target == null) target = PlayerController.instance.transform;
        }

        public override IEnumerator Attack(Enemy source)
        {
            Vector3 oriPos = source.transform.position;
            // Jump up
            for (float i = 0; i < jumpDuration; i += Time.deltaTime)
            {
                source.transform.position = Vector3.Lerp(oriPos, oriPos + new Vector3(0, jumpHeight, 0), i / jumpDuration);
                yield return null;
            }
            Vector3 playerPos = target.position;
            oriPos = source.transform.position;

            // Lunge down
            for (float i = 0; i < lungeDuration; i += Time.deltaTime)
            {
                source.transform.position = Vector3.Lerp(oriPos, playerPos, i / lungeDuration);
                yield return null;
            }

            // Deal damage
            InvokeOnAttackDone(lungeAttackRange);
            if (TargetSight.TargetIsInRange(source.transform, target, lungeAttackRange, 180))
            {
                target.GetComponent<Entity>().DealDamage(source.strength);
            }
        }

        IEnumerator ShowDebug()
        {
            while (showDebug)
            {
                DebugExtension.DebugCircle(this.transform.position, Color.red, lungeAttackRange);
                yield return null;
            }
        }
    }
}
