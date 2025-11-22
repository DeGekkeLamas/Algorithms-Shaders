using UnityEngine;

namespace Entities
{
    public abstract class EntityOnLevelUpAction : MonoBehaviour
    {
        Entity boundEntity;
        private void Awake()
        {
            boundEntity = this.GetComponent<Entity>();
        }
        private void OnEnable()
        {
            boundEntity.OnLevelUp += OnLevelUp;
        }
        private void OnDisable()
        {
            boundEntity.OnLevelUp -= OnLevelUp;
        }

        protected abstract void OnLevelUp();
    }
}

