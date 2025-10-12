using UnityEngine;
using System.Collections.Generic;

public abstract class Entity : MonoBehaviour
{
    public int maxHP = 100;
    float currentHP;
    public float moveSpeed = 1;
    [Tooltip("strength multiplies with damage of items or the thing itself")]
    public float strength = 1;

    [HideInInspector] public List<StatusEffect> activeStatusEffects;

    MovingObjectBase[] movements;

    protected virtual void Awake()
    {
        if (this.TryGetComponent(out MovingObjectBase movingObject)) movingObject.baseSpeed = moveSpeed;
        currentHP = maxHP;
        movements = GetComponents<MovingObjectBase>();
    }

    public void ChangeMoveSpeed(float newSpeed)
    {
        moveSpeed= newSpeed;

        foreach (MovingObjectBase obj in movements) obj.baseSpeed = moveSpeed;
    }

    public void DealDamage(float dmg)
    {
        UpdateHP(currentHP - dmg);
        Debug.Log($"Dealt {dmg} damage to {this.gameObject.name}");
    }

    void UpdateHP(float newHP)
    {
        currentHP = newHP;
        if (currentHP < maxHP) Death();
    }

    protected abstract void Death();
}
