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

    public IEnumerator WaitForCurrentAnimation()
    {
        float animationDuration = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(animationDuration);
        yield return new WaitForEndOfFrame();
    }
}
