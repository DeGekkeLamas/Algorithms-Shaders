using Entities;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnitTesting
{
    public class EntityDeathTest : MonoBehaviour
    {
        [HideInInspector] public bool wasSuccess;

        private void Start()
        {
            Entity_Death_Is_Triggered();
        }

        void Entity_Death_Is_Triggered()
        {
            Entity target = this.GetComponent<Entity>();
            target.OnDeath += Death;
            target.DealDamage(target.maxHP);
        }

        void Death()
        {
            wasSuccess = true;
            Debug.Log("Death test success");
        }
    }
}
