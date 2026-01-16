using UnityEngine;

namespace Entities
{
    /// <summary>
    /// Set the movement speed of the entity to 0 when it dies
    /// </summary>
    public class OnDeathStopMovement : EntityDeathAction
    {
        protected override void OnDeath()
        {
            boundEntity.ChangeMoveSpeed(0);
        }
    }
}
