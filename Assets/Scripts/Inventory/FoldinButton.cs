using NaughtyAttributes;
using System.Collections;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Interpolate an object between its original position and target position based on the scale of the canvas its on
/// </summary>
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

    }

    private void Start()
    {
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
            StartCoroutine(LerpBetween(target, targetPos, originalPos, foldTime));
            isCollapsed = false;
        }
        else
        {
            StartCoroutine(LerpBetween(target, originalPos, targetPos, foldTime));
            isCollapsed = true;
        }
    }

    IEnumerator LerpBetween(Transform target, Vector3 initialPos, Vector3 targetPos, float duration)
    {
        float timer = 0;
        while(timer < duration)
        {
            timer += Time.deltaTime;
            // Set pos
            target.position = Vector3.Lerp(initialPos, targetPos, timer/ duration) * canvas.transform.localScale.x;
            //target.position = originalPos;
            yield return null;
        }
        Debug.Log("Folded in menu");
    }

    [Button("(Debug) get current pos", EButtonEnableMode.Editor)]
    void PrintCurrentPos()
    {
        canvas = GetCanvas();
        Debug.Log($"Current pos = {target.position / canvas.transform.localScale.x}");
    }
}
