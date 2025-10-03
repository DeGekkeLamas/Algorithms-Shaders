using UnityEngine;
using UnityEngine.UI;
using InventoryStuff;

public class GameManager : MonoBehaviour
{
    Inventory inventory;
    public static GameManager instance;

    [Header("UI")]
    public GameObject itemDescription;


    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        instance = this;
    }
}
