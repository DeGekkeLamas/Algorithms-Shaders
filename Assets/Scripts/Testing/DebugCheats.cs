using InventoryStuff;
using UnityEngine;


public class DebugCheats : MonoBehaviour
{
    public static bool alwaysCraft;
    public static bool unlockAllDoors;

    private void Awake()
    {
#if !UNITY_EDITOR
        Destroy(this.gameObject);
#endif
    }

    public void AddItem(InventoryItemData toAdd)
    {
#if UNITY_EDITOR
        Inventory.instance.AddItem(toAdd.GetItem());
#endif
    }

    public void ToggleAlwaysCraft()
    {
#if UNITY_EDITOR
        alwaysCraft = !alwaysCraft;
        Debug.Log("Toggled always craft");
#endif
    }

    public void UnlockAllDoors()
    {
#if UNITY_EDITOR
        unlockAllDoors = !unlockAllDoors;
        Debug.Log("Toggled unlock all doors");
#endif
    }
}
