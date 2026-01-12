using System.Collections;
using UnityEngine;
using Entities;
using System;
using DungeonGeneration;

/// <summary>
/// Door to next level that requires a certain amount of kills to "open"
/// </summary>
public class ExitDoor : MonoBehaviour, IInteractible
{
    public int killsRequired;
    int killsDone = 0;
    public int KillsDone => killsDone;
    bool isOpen;
    public bool IsOpen => isOpen;
    [HideInInspector] public event Action OnStatsUpdated;

    private void Start()
    {
        Entity.OnAnyDeath += DoorCheck;
        GameManager.instance.OnNewFloorLoaded += ResetDoor;
        OnStatsUpdated?.Invoke();
    }
    private void OnEnable()
    {
        if (!didStart) return;
        Entity.OnAnyDeath += DoorCheck;
        GameManager.instance.OnNewFloorLoaded += ResetDoor;
    }
    private void OnDisable()
    {
        Entity.OnAnyDeath -= DoorCheck;
        if (GameManager.instance != null)
            GameManager.instance.OnNewFloorLoaded -= ResetDoor;
    }

    void DoorCheck(Entity _)
    {
        killsDone++;
        if (killsDone >= Mathf.Min(killsRequired, DungeonGenerator.instance.enemiesSpawned))
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
        killsDone = 0;
        isOpen = false;
        OnStatsUpdated?.Invoke();
    }

    public void OnInteract()
    {
#if UNITY_EDITOR
        if (DebugCheats.unlockAllDoors) GameManager.instance.MoveToNextRoom();
#endif
        if (isOpen) GameManager.instance.MoveToNextRoom();
    }
}
