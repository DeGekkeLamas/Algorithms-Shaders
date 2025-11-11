using NaughtyAttributes;
using UnityEngine;

namespace InventoryStuff
{
    public class ItemCheats : MonoBehaviour
    {
        public InventoryItemData itemToAdd;

        [Button("Add item", EButtonEnableMode.Playmode)]
        void AddItemCheat()
        {
            Inventory.instance.AddItem(itemToAdd.GetItem());
        }
        #if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Equals)) AddItemCheat();
        }
        #endif
    }
}
