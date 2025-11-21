using System;
using System.Collections;
using UnityEngine;

namespace Entities.Enemies
{
    public abstract class AttackCycle : MonoBehaviour
    {
        [HideInInspector] public event Action<float> OnAttackDone;
        public abstract IEnumerator Attack(Enemy source);

        protected void InvokeOnAttackDone(float dst) => OnAttackDone?.Invoke(dst);
    }
}
