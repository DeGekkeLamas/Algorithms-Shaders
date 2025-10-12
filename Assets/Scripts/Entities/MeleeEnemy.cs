using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("Attack distance")]
    [InfoBox("maxAttackDistance is how far away the target will still receive damage, " +
        "attackStartDistance is from how far away the enemy will start attacking")]
    public float maxAttackDistance = 3;
    public float attackStartDistance = 2;
    public float attackAngle = 75;

    public bool showVisualDebug;
    bool isAttacking;

    AnimationController anim;

    protected override void OnValidate()
    {
        base.OnValidate();
        if (showVisualDebug) StartCoroutine(ShowVisualDebug());
    }

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<AnimationController>();
    }

    private void Update()
    {
        if (!isAttacking && PlayerInRange(attackStartDistance)) StartCoroutine(Attack());
    }

    bool PlayerInRange(float maxDst)
    {
        return TargetSight.CanSeePlayer(this.transform, PlayerController.instance.transform, maxDst, attackAngle);
    }
    IEnumerator Attack()
    {
        //Debug.Log("Started attack");
        isAttacking = true;
        float originalSpeed = moveSpeed;
        ChangeMoveSpeed(0);
        // Wait for animation to finish
        yield return anim.WaitForAnimation("Attack");

        // Do attack
        if (PlayerInRange(maxAttackDistance))
        {
            PlayerController.instance.DealDamage(strength);
        }
        isAttacking = false;
        ChangeMoveSpeed(originalSpeed);
    }

    IEnumerator ShowVisualDebug()
    {
        while(showVisualDebug)
        {
            VisualDebug();
            yield return null;
        }
    }

    /// <summary>
    /// shows vision range
    /// </summary>
    void VisualDebug()
    {
        DebugExtension.DebugCircle(this.transform.position, Color.red, maxAttackDistance);
        DebugExtension.DebugCircle(this.transform.position, Color.red, attackStartDistance);
        Debug.DrawRay(transform.position, VectorMath.RotateVectorXZ(this.transform.forward * maxAttackDistance, attackAngle), Color.red);
        Debug.DrawRay(transform.position, VectorMath.RotateVectorXZ(this.transform.forward * maxAttackDistance, -attackAngle), Color.red);
    }
}
