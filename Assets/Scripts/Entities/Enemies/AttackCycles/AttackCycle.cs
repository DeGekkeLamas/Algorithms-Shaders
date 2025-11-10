using System.Collections;
using UnityEngine;

public abstract class AttackCycle : MonoBehaviour
{
    public abstract IEnumerator Attack(AttackCycleEnemy source);
}
