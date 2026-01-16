using Entities.Player;
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
        [SerializeField] float XPReward = 60;

        public event Action OnInitialize;
        public event Action OnProgressUpdated;
        public event Action OnComplete;

        [HideInInspector, NonSerialized] public int progress = 0;
        [HideInInspector, NonSerialized] public int maxProgress = 1;

        /// <summary>
        /// Initialize the quest
        /// </summary>
        public virtual void Initialize()
        {
            InvokeOnInitialize();
            InvokeOnProgressUpdated();
        }

        /// <summary>
        /// Uninizializes the quest, implement this to unsubscribe from the event that calls the type-specific quest check. Questmanager calls this when it gets destroyed
        /// </summary>
        public virtual void Destructor()
        {

        }

        /// <summary>
        /// Sets the description of the quest
        /// </summary>
        protected abstract string SetDescription();

        /// <summary>
        /// Called when quest is complete
        /// </summary>
        protected virtual void OnCompleted()
        {
            OnComplete?.Invoke();
            Debug.Log($"Completed quest {this.name}");
            PlayerController.instance?.AddXP(XPReward);
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
