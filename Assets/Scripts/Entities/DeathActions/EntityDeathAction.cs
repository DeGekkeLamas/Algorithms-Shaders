using UnityEngine;

namespace Entities
{
    /// <summary>
    /// Baseclass for actions that happen when the bound entity dies
    /// </summary>
    public abstract class EntityDeathAction : MonoBehaviour
    {
        protected Entity boundEntity;
        private void Awake()
        {
            boundEntity = this.GetComponent<Entity>();
        }
        private void OnEnable()
        {
            boundEntity.OnDeath += OnDeath;
        }
        private void OnDisable()
        {
            boundEntity.OnDeath -= OnDeath;
        }

        protected abstract void OnDeath();
    }
}
