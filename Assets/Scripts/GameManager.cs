using UnityEngine;
using DungeonGeneration;
using UnityEngine.SceneManagement;
using System;
using InventoryStuff;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    int currentRoom = 1;
    int seed;
    public int scene;
    public IntScene[] roomExceptions;
    public ExitDoor exit;
    public Transform RecipeUI;

    public static GameManager instance;
    public event Action OnNewFloorLoaded;
    
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Start()
    {
        seed = DungeonGenerator.instance.seed;
    }

    public void MoveToNextRoom()
    {
        currentRoom++;
        for (int i = 0; i < roomExceptions.Length; i++)
        {
            IntScene exception = roomExceptions[i];
            if (currentRoom == exception.number)
            {
                SceneManager.LoadScene(exception.scene);
                break;
            }
        }
        SceneManager.LoadScene(scene);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (seed != 0)
            DungeonGenerator.instance.seed = seed + currentRoom - 1;
        exit.killsRequired = DungeonGenerator.instance.Rda.AmountOfEnemiesToSpawn / 2;
        OnNewFloorLoaded?.Invoke();
        exit.gameObject?.SetActive(false);
    }

    public void ResetGame()
    {
        currentRoom = 0;
        Inventory.instance.ClearInventory();
        MoveToNextRoom();
    }
}

[Serializable]
public struct IntScene
{
    public int number;
    public int scene;
}
