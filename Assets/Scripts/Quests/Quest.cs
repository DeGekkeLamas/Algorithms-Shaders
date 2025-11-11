using NaughtyAttributes;
using System;
using UnityEngine;

public abstract class Quest : ScriptableObject
{
    [ReadOnly] [TextArea] public string description;
    [ReadOnly] public Texture2D texture;

    public event Action OnInitialize;
    public event Action OnProgressUpdated;

    public virtual void Initialize()
    {
        InvokeOnInitialize();
    }
    protected abstract string SetDescription();

    protected virtual void OnCompleted()
    {

    }

    protected void InvokeOnProgressUpdated()
    {
        OnProgressUpdated?.Invoke();
    }
    protected void InvokeOnInitialize()
    {
        OnInitialize?.Invoke();
    }
}
