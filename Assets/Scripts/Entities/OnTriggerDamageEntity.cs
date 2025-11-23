using Entities;
using UnityEngine;
using System.Collections.Generic;
using System;
using Entities.Player;

public class OnTriggerDamageEntity : MonoBehaviour
{
    [HideInInspector] public float damage;
    [HideInInspector] public float damageToExceptions;
    List<Entity> exceptions;

    private void OnTriggerEnter(Collider other)
    {
        // Damage entity
        if (other.TryGetComponent(out Entity entity))
        {
            foreach(Entity ex in exceptions)
            {
                if (entity == ex)
                {
                    entity.DealDamage(damageToExceptions);
                    return;
                }
            }
            entity.DealDamage(damage);
        }
    }

    public void AddException(Entity entity)
    {
        if (exceptions.Contains(entity)) return;
        exceptions.Add(entity);
    }
}
