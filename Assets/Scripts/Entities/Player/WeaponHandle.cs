using UnityEngine;

namespace Entities.Player
{
    [RequireComponent(typeof(OnTriggerDamageEntity))]
    [RequireComponent(typeof(BoxCollider))]
    public class WeaponHandle : MonoBehaviour
    {
        [HideInInspector] public BoxCollider handleCollider;
        [HideInInspector] public OnTriggerDamageEntity damager;

        private void Awake()
        {
            handleCollider = this.GetComponent<BoxCollider>();
            damager = this.GetComponent<OnTriggerDamageEntity>();
        }
    }
}
