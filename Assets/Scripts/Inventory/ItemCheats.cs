using UnityEngine;

public class ItemCheats : MonoBehaviour
{
    Inventory inventory;
    private void Awake() => inventory = GetComponent<Inventory>();
    private void Update()
    {
        if (Input.GetKey(KeyCode.Equals))
        {
            switch(Input.inputString)
            {

                case "-":
                    for(int i = 0; i < inventory.currentInventory.Length; i++)
                        inventory.RemoveItem(i);
                    Debug.Log($"Cleared inventory, from {this}");
                    break;
            }
        }
    }
}
