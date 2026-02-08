using DungeonGeneration;
using Entities;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Quests;
using UnityEngine.SceneManagement;

public class QuestDoor : MonoBehaviour, IInteractible
{
    public Quest[] questsRequired;
    int questsDone = 0;
    public int QuestsDone => questsDone;
    [SerializeField] int sceneToLoad;
    bool isOpen;
    public bool IsOpen => isOpen;
    [HideInInspector] public event Action OnStatsUpdated;

    private void Start()
    {
        foreach (Quest quest in questsRequired)
        {
            quest.OnComplete += DoorCheck;
        }
        GameManager.instance.OnNewFloorLoaded += ResetDoor;
        OnStatsUpdated?.Invoke();
    }

    void DoorCheck()
    {
        questsDone++;
        if (questsDone >= questsRequired.Length)
        {
            if (!isOpen) StartCoroutine(OpenDoor());
            isOpen = true;
        }
        OnStatsUpdated?.Invoke();
    }

    IEnumerator OpenDoor()
    {
        Debug.Log("Opened door");
        yield return new();
    }

    /// <summary>
    /// Reset the openstate and killsdone
    /// </summary>
    public void ResetDoor()
    {
        questsDone = 0;
        isOpen = false;
        OnStatsUpdated?.Invoke();
    }

    public void OnInteract()
    {
#if UNITY_EDITOR
        if (DebugCheats.unlockAllDoors)
        {
            StartCoroutine(EnterDoor());
        }
#endif
        if (isOpen)
        {
            StartCoroutine(EnterDoor());
        }
        else Debug.Log("Door closed");
    }

    IEnumerator EnterDoor()
    {
        GameManager.instance.transform.parent = this.transform;
        yield return null;
        SceneManager.LoadScene(sceneToLoad);
    }
}
