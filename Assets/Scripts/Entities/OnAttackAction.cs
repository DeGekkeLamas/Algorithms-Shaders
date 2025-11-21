using UnityEngine;

namespace Entities.Enemies
{
    public abstract class OnAttackAction : MonoBehaviour
    {
        AttackCycle attack;

        private void Awake()
        {
            attack = GetComponent<AttackCycle>();
        }

        private void Start()
        {
            attack.OnAttackDone += AttackAction;
        }

        private void OnDestroy()
        {
            attack.OnAttackDone -= AttackAction;
        }

        protected abstract void AttackAction(float dst);
    }
}
