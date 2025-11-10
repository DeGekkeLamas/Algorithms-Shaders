using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AttackCycleLunge : AttackCycle
{
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
    }

    public override IEnumerator Attack(AttackCycleEnemy source)
    {
        Vector3 oriPos = source.transform.position;
        // Jump up
        for(float i = 0; i < jumpDuration; i += Time.deltaTime)
        {
            source.transform.position = Vector3.Lerp(oriPos, oriPos + new Vector3(0,jumpHeight,0), i/jumpDuration);
            yield return null;
        }
        Vector3 playerPos = PlayerController.instance.transform.position;
        oriPos = source.transform.position;
        // Lunge down
        for (float i = 0; i < lungeDuration; i += Time.deltaTime)
        {
            source.transform.position = Vector3.Lerp(oriPos, playerPos, i/lungeDuration);
            yield return null;
        }
        // Attack
        if (TargetSight.PlayerIsInRange(source.transform, PlayerController.instance.transform, lungeAttackRange, 180))
        {
            PlayerController.instance.DealDamage(source.strength);
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
