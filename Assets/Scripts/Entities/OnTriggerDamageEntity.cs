using Entities;
using UnityEngine;
using System.Collections.Generic;
using System;
using Entities.Player;

public class OnTriggerDamageEntity : MonoBehaviour
{
    [HideInInspector] public float damage;
    [HideInInspector] public List<Entity> exceptions;

    private void OnTriggerEnter(Collider other)
    {
        // Damage entity
        if (other.TryGetComponent(out Entity entity))
        {
            foreach(Entity ex in exceptions)
            {
                if (entity == ex) return;
            }
            entity.DealDamage(damage);
        }
    }
}
