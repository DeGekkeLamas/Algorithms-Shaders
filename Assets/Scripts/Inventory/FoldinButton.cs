using NaughtyAttributes;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class FoldinButton : MonoBehaviour
{
    public RectTransform target;
    public Vector2 targetPos;
    public float foldTime = 1;

    Canvas canvas;
    bool isCollapsed;
    bool isFolding;
    Vector2 originalPos;

    private void Awake()
    {
        canvas = GetCanvas();

        originalPos = target.position / canvas.transform.localScale.x;
    }

    Canvas GetCanvas()
    {
        // Get canvas
        Transform current = this.transform;
        while (current != null) // find canvas in parents
        {
            if (current.TryGetComponent(out Canvas canvas))
            {
                return canvas;
            }
            current = current.parent;
        }
        Debug.LogError("Could not find canvas");
        return null;
    }

    public void OnPress()
    {
        if (isFolding) return;
        // Fold or unfold
        if (isCollapsed)
        {
            StartCoroutine(FoldOut());
            isCollapsed = false;
        }
        else
        {
            StartCoroutine(FoldIn());
            isCollapsed = true;
        }
    }

    IEnumerator FoldIn()
    {
        float timer = 0;
        while(timer < foldTime)
        {
            timer += Time.deltaTime;
            // Set pos
            target.position = Vector3.Lerp(originalPos, targetPos, timer/foldTime) * canvas.transform.localScale.x;
            //target.position = originalPos;
            yield return null;
        }
        Debug.Log("Folded in menu");
    }

    IEnumerator FoldOut()
    {
        float timer = 0;
        while (timer < foldTime)
        {
            timer += Time.deltaTime;
            // Set pos
            target.position = Vector3.Lerp(targetPos, originalPos, timer/foldTime) * canvas.transform.localScale.x;
            yield return null;
        }
        Debug.Log("Folded out menu");
    }
    [Button("(Debug) get current pos", EButtonEnableMode.Editor)]
    void PrintCurrentPos()
    {
        canvas = GetCanvas();
        Debug.Log($"Current pos = {target.position / canvas.transform.localScale.x}");
    }
}
