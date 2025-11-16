using UnityEngine;
using System.Collections.Generic;
using System;
using Entities.StatusEffects;

namespace Entities
{
    public abstract class Entity : MonoBehaviour
    {
        public string entityName;
        public float maxHP = 100;
        float currentHP;
        public float CurrentHP => currentHP;
        public float moveSpeed = 1;
        [Tooltip("strength multiplies with damage of items or the thing itself")]
        public float strength = 1;

        [Header("Level stuff")]
        public int level = 1;
        public float XPRequired = 100;
        float currentXP = 0;
        public float CurrentXP => currentXP;
        [Space]
        public float HPIncrement = 10;
        public float strengthIncrement = .1f;

        [HideInInspector] public List<StatusEffect> activeStatusEffects;
        // events
        [HideInInspector] public event Action OnStatsChanged;
        [HideInInspector] public event Action OnLevelUp;
        [HideInInspector] public event Action OnXPAdded;
        [HideInInspector] public event Action OnDeath;
        public static event Action<Entity> OnAnyDeath;

        MovingObjectBase[] movements;

        protected virtual void Awake()
        {
            if (this.TryGetComponent(out MovingObjectBase movingObject)) movingObject.baseSpeed = moveSpeed;
            currentHP = maxHP;
            movements = GetComponents<MovingObjectBase>();
        }
        protected virtual void Start()
        {
            OnStatsChanged?.Invoke();
        }

        public void ChangeMoveSpeed(float newSpeed)
        {
            moveSpeed = newSpeed;

            foreach (MovingObjectBase obj in movements) obj.baseSpeed = moveSpeed;
        }

        public void DealDamage(float dmg)
        {
            UpdateHP(currentHP - dmg);
            Debug.Log($"Dealt {dmg} damage to {this.entityName}");
        }

        void UpdateHP(float newHP)
        {
            currentHP = newHP;
            if (currentHP <= 0) Death();
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);

            OnStatsChanged?.Invoke();
        }

        public void AddXP(float toAdd)
        {
            currentXP += toAdd;
            if (currentXP >= XPRequired)
            {
                currentXP -= XPRequired;
                LevelUp();
            }
            OnXPAdded?.Invoke();
            OnStatsChanged?.Invoke();
        }

        void LevelUp()
        {
            level++;
            currentHP += (currentHP / maxHP) * HPIncrement;
            maxHP += HPIncrement;
            strength += strengthIncrement;
            OnLevelUp?.Invoke();
        }

        protected virtual void Death()
        {
            OnDeath?.Invoke();
            OnAnyDeath?.Invoke(this);
        }
    }
}
