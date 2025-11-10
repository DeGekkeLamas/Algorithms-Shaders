using System.Collections;
using UnityEngine;

public class AttackCycleAreaSlam : AttackCycle
{
    public float attackRange = 4;
    public bool showDebug;
    private void OnValidate()
    {
        if (showDebug) StartCoroutine(ShowDebug());
    }

    public override IEnumerator Attack(AttackCycleEnemy source)
    {
        yield return source.Animator.WaitForAnimation("Attack");
        if (TargetSight.PlayerIsInRange(source.transform, PlayerController.instance.transform, attackRange, 180))
        {
            PlayerController.instance.DealDamage(source.strength);
        }
        yield return null;
        yield return source.Animator.WaitForCurrentAnimation();
    }

    IEnumerator ShowDebug()
    {
        while(showDebug)
        {
            DebugExtension.DebugCircle(this.transform.position, Color.red, attackRange);
            yield return null;
        }
    }
}
