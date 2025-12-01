using UnityEngine;

namespace Entities
{
    public class EntityDeathRestartGame : EntityDeathAction
    {
        protected override void OnDeath()
        {
            StartCoroutine(GameManager.instance.ResetGame());
        }
    }
}
