using System.Collections;
using UnityEngine;

public class AttackCycleEnemy : Enemy
{
    [Header("Type specific")]
    public AttackCycle[] attackCycles;
    public float timeBetweenCycles = 1;
    void Start()
    {
        StartCoroutine(AttackCycle());
    }

    IEnumerator AttackCycle()
    {
        float originalMoveSpeed = moveSpeed;
        for (int i = 0; i < attackCycles.Length; i = (i+1) % attackCycles.Length)
        {
            ChangeMoveSpeed(0);
            yield return attackCycles[i].Attack(this);
            ChangeMoveSpeed(originalMoveSpeed);
            yield return new WaitForSeconds(timeBetweenCycles);
        }
        yield return new();
    }
}
