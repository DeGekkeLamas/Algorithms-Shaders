using System.Collections.Generic;
using UnityEngine;

public class OtherInventorySystem : MonoBehaviour
{
    static Dictionary<string, int> inventory = new Dictionary<string, int>();

    public static void AddItem(string key)
    {
        if (inventory.ContainsKey(key)) inventory[key]++;
        else inventory[key] = 1;
    }

    public static void RemoveItem(string key)
    {
        inventory[key]--;
        if (inventory[key] == 0) inventory.Remove(key);
    }

    public static void PrintInventory()
    {
        foreach(KeyValuePair<string, int> kvp in inventory) Debug.Log(kvp.Key + kvp);
    }

    public static bool ItemExists(string key) => inventory.ContainsKey(key);
}
