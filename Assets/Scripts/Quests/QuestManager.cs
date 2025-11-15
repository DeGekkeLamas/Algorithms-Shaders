using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    List<Quest> activeQuests = new();
    public GameObject questPresenter;
    public HorizontalOrVerticalLayoutGroup layoutGroup;

    public void AddQuest(Quest toAdd)
    {
        activeQuests.Add(toAdd);
        // Set presenters
        GameObject spawned = Instantiate(questPresenter, layoutGroup.transform);
        QuestPresenter[] presenters = spawned.GetComponentsInChildren<QuestPresenter>();
        for(int i = 0; i < presenters.Length; i++)
        {
            presenters[i].boundQuest = toAdd;
        }
        // Wait a frame between setting the quest and activating initilize, so the presenters can initialize
        StartCoroutine(WaitThenInitialize());
    }

    IEnumerator WaitThenInitialize()
    {
        yield return null;
        toAdd.Initialize();
    }

    public Quest toAdd;
    [Button("Test quest adder", EButtonEnableMode.Playmode)]
    void TestAddQuest() => AddQuest(toAdd);
}
