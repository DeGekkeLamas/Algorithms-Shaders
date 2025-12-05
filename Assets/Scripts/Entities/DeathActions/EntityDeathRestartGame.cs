using System.Collections;
using UnityEngine;

namespace Entities
{
    public class EntityDeathRestartGame : EntityDeathAction
    {
        public float delay = 1;
        protected override void OnDeath()
        {
            StartCoroutine(DelayThenReset());
        }

        IEnumerator DelayThenReset()
        {
            yield return new WaitForSeconds(delay);
            StartCoroutine(GameManager.instance.ResetGame());
        }
    }
}
