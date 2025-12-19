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
        protected void HasAnimationCheck(string toCheck)
        {
            if (!AnimationController.AnimationExists(toCheck, MathTools.GetComponentInParents<Animator>(this.transform)))
                Debug.LogWarning($"Animation \"{toCheck}\" does not exist on this entity, using this attack will cause an error");
            //else Debug.Log("Animation exists");
        }
    }
}
