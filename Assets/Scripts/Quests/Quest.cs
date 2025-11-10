using NaughtyAttributes;
using UnityEngine;

public abstract class Quest : ScriptableObject
{
    [ReadOnly] [TextArea] public string description;
    [ReadOnly] public Texture2D texture;

    protected abstract void Initialize();
    protected abstract string SetDescription();

    protected void OnCompleted()
    {

    }
}
