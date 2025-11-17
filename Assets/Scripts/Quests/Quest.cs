using NaughtyAttributes;
using System;
using UnityEngine;

namespace Quests
{
    /// <summary>
    /// Baseclass for quests, progress and maxProgress values are used for presenters
    /// </summary>
    public abstract class Quest : ScriptableObject
    {
        [ReadOnly][TextArea] public string description;
        [ReadOnly] public Texture2D texture;

        public event Action OnInitialize;
        public event Action OnProgressUpdated;

        [HideInInspector, NonSerialized] public int progress = 0;
        [HideInInspector, NonSerialized] public int maxProgress = 1;

        public virtual void Initialize()
        {
            InvokeOnInitialize();
            InvokeOnProgressUpdated();
        }
        protected abstract string SetDescription();

        protected virtual void OnCompleted()
        {
            Debug.Log($"Completed quest {this.name}");
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
}
