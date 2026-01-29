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
        [HideInInspector] public InventoryItem item;
        private void OnValidate()
        {
            SetDescription();
        }

        public override void Destructor()
        {
            base.Destructor();
            Inventory.instance.OnItemChanged -= UpdateProgress;
        }

        public override void Initialize()
        {
            if (toCollect != null)
            {
                item = toCollect.GetItem();
                texture = item.ItemSprite;
            }
            Inventory.instance.OnItemChanged += UpdateProgress;
            progress = 0;
            maxProgress = 1;
            base.Initialize();
        }

        protected void UpdateProgress()
        {
            if (Inventory.instance.Contains(item))
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
