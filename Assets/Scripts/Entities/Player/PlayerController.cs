using InventoryStuff;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.Player
{
    /// <summary>
    /// Contains player data
    /// </summary>
    public class PlayerController : Entity
    {
        public float projectileForce = 5;

        [Header("References")]
        public WeaponHandle meleeWeaponHandle;
        public static PlayerController instance;
        protected override void Awake()
        {
            if (instance == null) instance = this;
            else
            {
                instance.transform.position = this.transform.position;
                Destroy(this.gameObject);
            }

            base.Awake();
        }
        protected override void Start()
        {
            base.Start();
            transform.parent = GameManager.instance.transform;
        }

        public static Vector3 GetMousePosition()
        {
            (bool, RaycastHit) rayHit = GetCamCast(~LayerMask.GetMask("Player"));
            return rayHit.Item2.point;
        }
        public static (bool, RaycastHit) GetCamCast(int layermask)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hitSomething = Physics.Raycast(mouseRay,
                out RaycastHit rayHit, 1000, layermask, QueryTriggerInteraction.Ignore);
            return (hitSomething, rayHit);
        }

        protected override void Death()
        {
            base.Death();
            Debug.Log("You suck at this game LMAO");
        }
    }
}
