using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public abstract class AttackingEnemy : Enemy
{
    [Header("Attack distance")]
    [InfoBox("maxAttackDistance is how far away the target will still receive damage, " +
        "attackStartDistance is from how far away the enemy will start attacking")]
    public float attackStartDistance = 2;
    public float attackAngle = 75;

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
        return TargetSight.CanSeePlayer(this.transform, PlayerController.instance.transform, maxDst, attackAngle);
    }
    IEnumerator AttackCycle()
    {
        //Debug.Log("Started attack");
        isAttacking = true;
        float originalSpeed = moveSpeed;
        ChangeMoveSpeed(0);
        // Wait for animation to finish
        yield return anim.WaitForAnimation("Attack");

        // Do attack
        Attack();

        yield return null;
        yield return anim.WaitForCurrentAnimation();
        isAttacking = false;
        ChangeMoveSpeed(originalSpeed);
    }

    protected abstract void Attack();

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
