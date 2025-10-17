using NaughtyAttributes;
using UnityEngine;

namespace InventoryStuff
{
    public class ItemCheats : MonoBehaviour
    {
        public InventoryItemData itemToAdd;
        Inventory inventory;

        private void Awake() => inventory = GetComponent<Inventory>();

        [Button("Add item", EButtonEnableMode.Playmode)]
        void AddItemCheat()
        {
            inventory.AddItem(itemToAdd.GetItem());
        }
    }
}
