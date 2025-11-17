using Entities;
using UnityEngine;

public class OnTriggerDamageEntity : MonoBehaviour
{
    [HideInInspector] public float damage;

    private void OnTriggerEnter(Collider other)
    {
        // Damage entity
        if (other.TryGetComponent(out Entity entity))
        {
            entity.DealDamage(damage);
        }
    }
}
