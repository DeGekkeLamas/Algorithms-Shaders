using InventoryStuff;
using UnityEngine;
using Entities;
using NaughtyAttributes;
using System;

namespace Quests
{
    /// <summary>
    /// Questtype for killing a certain amount of enemies
    /// </summary>
    [CreateAssetMenu(
        fileName = "SlayQuest",
        menuName = "ScriptableObjects/Quests/SlayQuest",
        order = 0)]
    public class SlayQuest : Quest
    {
        [InfoBox("Leave empty for any enemy to count")]
        [SerializeField] Entity toKill;
        [SerializeField] int amount;
        [NonSerialized] int amountDone;

        private void OnValidate()
        {
            SetDescription();
        }

        public override void Destructor()
        {
            base.Destructor();
            Entity.OnAnyDeath -= UpdateProgress;
        }

        public override void Initialize()
        {
            // Texture
            if (toKill != null)
                texture = RuntimePreviewGenerator.GenerateModelPreview(toKill.transform, 256, 256, false, true);
            else texture = default;

            Entity.OnAnyDeath += UpdateProgress;
            maxProgress = amount;
            base.Initialize();
        }

        void UpdateProgress(Entity toCheck)
        {
            if (toKill == null || toCheck.entityName == toKill.entityName) amountDone++;
            progress = amountDone;

            if (amountDone >= amount) OnCompleted();
            InvokeOnProgressUpdated();
        }

        protected override void OnCompleted()
        {
            base.OnCompleted();
            Entity.OnAnyDeath -= UpdateProgress;
        }

        protected override string SetDescription()
        {
            if (toKill == null) return "Kill {amount} enemies";

            string desc = $"Kill {amount} {toKill.entityName}{(amount != 1 ? "s" : "")}";

            description = desc;
            return desc;
        }
    }
}
