using InventoryStuff;
using UnityEngine;
using Entities;

namespace Quests
{

    [CreateAssetMenu(
        fileName = "SlayQuest",
        menuName = "ScriptableObjects/Quests/SlayQuest",
        order = 0)]
    public class SlayQuest : Quest
    {
        public Entity toKill;
        public int amount;
        int amountDone;

        private void OnValidate()
        {
            SetDescription();
        }

        public override void Initialize()
        {
            texture = RuntimePreviewGenerator.GenerateModelPreview(toKill.transform, 256, 256, false, true);
            Entity.OnAnyDeath += UpdateProgress;
            maxProgress = amount;
            base.Initialize();
        }

        void UpdateProgress(Entity toCheck)
        {
            if (toCheck.entityName == toKill.entityName) amountDone++;
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
            if (toKill == null) return "No item set";

            string desc = $"Kill {amount} {toKill.entityName}{(amount != 1 ? "s" : "")}";

            description = desc;
            return desc;
        }
    }
}
