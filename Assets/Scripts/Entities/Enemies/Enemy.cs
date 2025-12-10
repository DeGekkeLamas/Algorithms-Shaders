using System.Collections;
using UnityEngine;
using Entities.Player;

namespace Entities.Enemies
{
    /// <summary>
    /// Baseclass for enemies
    /// </summary>
    public class Enemy : Entity
    {
        public float xpToGive = 20;

        protected AnimationController anim;
        public AnimationController Animator => anim;

        protected override void Awake()
        {
            anim = GetComponent<AnimationController>();
            base.Awake();
        }

        protected override void Death()
        {
            base.Death();

            PlayerController.instance?.AddXP(xpToGive);
            StopAllCoroutines();
            Destroy(this.gameObject);
        }
    }
}
