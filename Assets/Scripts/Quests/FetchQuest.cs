using Entities;
using InventoryStuff;
using UnityEngine;

namespace Quests
{
    /// <summary>
    /// Questtype for obtaining a specific item
    /// </summary>
    [CreateAssetMenu(
        fileName = "FetchQuest",
        menuName = "ScriptableObjects/Quests/FetchQuest",
        order = 0)]
    public class FetchQuest : Quest
    {
        [SerializeField] InventoryItemData toCollect;

        private void OnValidate()
        {
            SetDescription();
        }

        public override void Destructor()
        {
            Inventory.instance.OnItemChanged -= UpdateProgress;
        }

        public override void Initialize()
        {
            texture = toCollect.GetItem().ItemSprite;
            Inventory.instance.OnItemChanged += UpdateProgress;
            maxProgress = 1;
            base.Initialize();
        }

        protected void UpdateProgress()
        {
            if (Inventory.instance.Contains(toCollect.GetItem()))
            {
                progress = 1;
                OnCompleted();
            }
            else progress = 0;
                InvokeOnProgressUpdated();

        }

        protected override void OnCompleted()
        {
            base.OnCompleted();
            Inventory.instance.OnItemChanged -= UpdateProgress;
        }

        protected override string SetDescription()
        {
            if (toCollect == null) return "No item set";

            string desc = $"Obtain {("aeiouAEIOU".Contains(toCollect.GetItem().itemName[0]) ? "an" : "a")} {toCollect.GetItem().itemName}";

            description = desc;
            return desc;
        }
    }
}
