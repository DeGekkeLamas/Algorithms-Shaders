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
        currentHP = maxHP;
    }
}
