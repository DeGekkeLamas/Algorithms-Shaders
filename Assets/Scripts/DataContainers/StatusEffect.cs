using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : ScriptableObject
{
    public string name;
    public float duration;
    protected float durationLeft;
    public ParticleSystem particleEffect;

    public Entity toAffect;

    public void ApplyEffect(Entity target)
    {
        target.activeStatusEffects.Add(this);
        toAffect = target;
    }
    public IEnumerator Lifespan()
    {
        OnDebuffStart();
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            OnDebuffStay();
            yield return null;
        }
    }

    /// <summary>
    /// Triggered when a effect is first applied
    /// </summary>
    public abstract void OnDebuffStart();

    /// <summary>
    /// Triggered every frame when effect is active
    /// </summary>
    public abstract void OnDebuffStay();

    /// <summary>
    /// Triggered when a effect is removed
    /// </summary>
    public abstract void OnDebuffEnd();
}
