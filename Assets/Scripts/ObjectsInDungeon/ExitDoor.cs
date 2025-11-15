using System.Collections;
using UnityEngine;

public class ExitDoor : MonoBehaviour, IInteractible
{
    public int killsRequired;
    int killsDone = 0;
    bool isOpen;
    private void Start()
    {
        Entity.OnAnyDeath += DoorCheck;
        GameManager.instance.OnNewFloorLoaded += ResetDoor;
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
        GameManager.instance.OnNewFloorLoaded -= ResetDoor;
    }

    void DoorCheck(Entity _)
    {
        killsDone++;
        if (killsDone >= killsRequired)
        {
            if (!isOpen) StartCoroutine(OpenDoor());
            isOpen = true;
        }
    }

    IEnumerator OpenDoor()
    {
        Debug.Log("Opened door");
        yield return new();
    }

    public void ResetDoor()
    {
        killsDone = 0;
        isOpen = false;
    }

    public void OnInteract()
    {
        if (isOpen) GameManager.instance.MoveToNextRoom();
    }
}
