using NaughtyAttributes;
using System;
using UnityEngine;

namespace Quests
{
    public abstract class Quest : ScriptableObject
    {
        [ReadOnly][TextArea] public string description;
        [ReadOnly] public Texture2D texture;

        public event Action OnInitialize;
        public event Action OnProgressUpdated;

        [HideInInspector] public int progress = 0;
        [HideInInspector] public int maxProgress = 1;

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
