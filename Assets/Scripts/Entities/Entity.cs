using UnityEngine;
using System.Collections.Generic;

public abstract class Entity : MonoBehaviour
{
    public int maxHP = 100;
    float currentHP;
    public float moveSpeed;
    [Tooltip("strength multiplies with damage of items or the thing itself")]
    public float strength = 1;

    public List<StatusEffect> activeStatusEffects;

    protected virtual void Awake()
    {
        if (this.TryGetComponent(out MovingObjectBase movingObject)) movingObject.baseSpeed = moveSpeed;
        currentHP = maxHP;
    }

    public void ChangeMoveSpeed(float newSpeed)
    {
        moveSpeed= newSpeed;

        MovingObjectBase[] objs = GetComponents<MovingObjectBase>();
        foreach (MovingObjectBase obj in objs) obj.baseSpeed = moveSpeed;
    }
}
