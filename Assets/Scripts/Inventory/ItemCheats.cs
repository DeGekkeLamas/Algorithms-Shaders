using NaughtyAttributes;
using UnityEngine;

namespace InventoryStuff
{
    /// <summary>
    /// Cheat to add a certain item to inventory, only compiles in unityeditor
    /// </summary>
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
