using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Play an animation, ends the coroutine when animation is done playing
    /// </summary>
    public IEnumerator WaitForAnimation(string animation)
    {
        animator.Play(animation);
        yield return null;
        yield return WaitForCurrentAnimation();
    }

    /// <summary>
    /// Checks if an animation exists and then plays the animation, ends the coroutine when animation is done playing
    /// </summary>
    public IEnumerator WaitForAnimationIfExists(string animation)
    {
        if (AnimationExists(animation)) yield break;
        yield return WaitForAnimation(animation);

    }

    public IEnumerator WaitForCurrentAnimation()
    {
        float animationDuration = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(animationDuration);
        yield return new WaitForEndOfFrame();
    }

    /// <summary>
    /// Check if this animation controller has a certain animation
    /// </summary>
    public bool AnimationExists(string name)
    {
        return AnimationExists(name, animator);
    }

    /// <summary>
    /// Check if this animation controller has a certain animation
    /// </summary>
    public static bool AnimationExists(string name, Animator animator)
    {
        int stateId = Animator.StringToHash(name);
        return animator.HasState(0, stateId);
    }
}
