using UnityEngine;

namespace Entities
{
    public abstract class EntityDeathAction : MonoBehaviour
    {
        Entity boundEntity;
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
