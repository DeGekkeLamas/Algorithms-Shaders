using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    List<Quest> activeQuests;

    public void AddQuest(Quest toAdd)
    {
        activeQuests.Add(toAdd);
        toAdd.Initialize();
    }
}
