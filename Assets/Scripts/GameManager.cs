using UnityEngine;
using UnityEngine.UI;
using InventoryStuff;
using DungeonGeneration;

public class GameManager : MonoBehaviour
{
    public ExitDoor exit;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        exit.killsRequired = DungeonGenerator.instance.Rda.AmountOfEnemiesToSpawn / 2;
    }
}
