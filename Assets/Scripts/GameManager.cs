using UnityEngine;
using DungeonGeneration;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    int currentRoom = 1;
    public int scene;
    public IntScene[] roomExceptions;
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
}

[Serializable]
public struct IntScene
{
    public int number;
    public int scene;
}
