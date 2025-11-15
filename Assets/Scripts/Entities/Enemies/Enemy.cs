using System.Collections;
using TMPro;
using UnityEngine;
using InventoryStuff;
using NaughtyAttributes;

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

    /// <summary>
    /// Spawns lootdrops, also destroys object
    /// </summary>
    protected override void Death()
    {
        base.Death();

        PlayerController.instance.AddXP(xpToGive);
        StopAllCoroutines();
        Destroy(this.gameObject);
    }
}
