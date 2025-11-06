using System.Collections;
using UnityEngine;

public class ExitDoor : MonoBehaviour, IInteractible
{
    public int killsRequired;
    int killsDone = 0;
    bool isOpen;
    private void OnEnable()
    {
        Entity.OnAnyDeath += DoorCheck;
    }
    private void OnDisable()
    {
        Entity.OnAnyDeath -= DoorCheck;
    }

    void DoorCheck()
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
        yield return new();
    }

    public void OnInteract()
    {

    }
}
