using System.Collections;
using UnityEngine;

public class IconRotator : MonoBehaviour
{
    public float duration = .5f;
    bool moving;
    public void Rotate()
    {
        if (!moving) StartCoroutine(RotateAnimation());
    }

    IEnumerator RotateAnimation()
    {
        moving = true;
        float original = this.transform.eulerAngles.z;
        for (float i = 0; i <= duration; i += Time.deltaTime)
        {
            this.transform.eulerAngles = Mathf.Lerp(original, original+180, i/duration) * Vector3.forward;
            yield return null;
        }
        this.transform.eulerAngles = (original + 180) * Vector3.forward;
        moving = false;
    }
}
