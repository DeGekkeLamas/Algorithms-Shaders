using System.Collections;
using UnityEngine;

public class AttackCycleEnemy : Enemy
{
    [Header("Type specific")]
    public AttackCycle[] attackCycles;
    void Start()
    {
        StartCoroutine(AttackCycle());
    }

    IEnumerator AttackCycle()
    {
        for (int i = 0; i < attackCycles.Length; i = (i+1) % attackCycles.Length)
        {
            yield return attackCycles[i].Attack(this);
        }
        yield return new();
    }
}
