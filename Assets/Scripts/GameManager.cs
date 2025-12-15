using DungeonGeneration;
using Entities.Player;
using InventoryStuff;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int currentRoom = 1;
    public int CurrentRoom => currentRoom;
    int seed;
    [SerializeField] int scene;
    [SerializeField] IntScene[] roomExceptions;
    public ExitDoor exit;
    public Transform RecipeUI;

    public static GameManager instance;
    public event Action OnNewFloorLoaded;
    
    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(this.gameObject);
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void Start()
    {
        if (DungeonGenerator.instance != null)
        {
            seed = DungeonGenerator.instance.seed;
        }
    }

    public void MoveToNextRoom()
    {
        currentRoom++;
        // Load exception scene if needed
        for (int i = 0; i < roomExceptions.Length; i++)
        {
            IntScene exception = roomExceptions[i];
            if (currentRoom == exception.number)
            {
                SceneManager.LoadScene(exception.scene);
                return;
            }
        }
        SceneManager.LoadScene(scene);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (DungeonGenerator.instance != null)
        {
            if (seed != 0)
                DungeonGenerator.instance.seed = seed + currentRoom - 1;
            exit.killsRequired = DungeonGenerator.instance.Rda.AmountOfEnemiesToSpawn / 2;
        }
        OnNewFloorLoaded?.Invoke();
        exit.gameObject.SetActive(false);
    }

    public IEnumerator ResetGame()
    {
        yield return new WaitForEndOfFrame();
        currentRoom = 0;
        Inventory.instance.ClearInventory();
        MoveToNextRoom();
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
        instance = null;
    }
}

[Serializable]
public struct IntScene
{
    public int number;
    public int scene;
}
