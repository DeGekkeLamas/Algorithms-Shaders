using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine.UI;
using Quests.Presenters;

namespace Quests
{
    /// <summary>
    /// Singleton for managing active quests
    /// </summary>
    public class QuestManager : MonoBehaviour
    {
        List<Quest> activeQuests = new();
        public GameObject questPresenter;
        public HorizontalOrVerticalLayoutGroup layoutGroup;

        [Header("Quests to give")]
        public List<Quest> registeredQuests;
        public int amountToGive = 2;

        private void Awake()
        {
            for(int i = 0; i < Mathf.Min(registeredQuests.Count, amountToGive); i++)
            {
                int index = Random.Range(0, registeredQuests.Count);
                AddQuest(registeredQuests[index]);
                registeredQuests.RemoveAt(i);
            }
        }
        public void AddQuest(Quest toAdd)
        {
            activeQuests.Add(toAdd);
            // Set presenters
            GameObject spawned = Instantiate(questPresenter, layoutGroup.transform);
            QuestPresenter[] presenters = spawned.GetComponentsInChildren<QuestPresenter>();
            for (int i = 0; i < presenters.Length; i++)
            {
                presenters[i].boundQuest = toAdd;
            }
            // Wait a frame between setting the quest and activating initilize, so the presenters can initialize
            StartCoroutine(WaitThenInitialize(toAdd));
        }

        IEnumerator WaitThenInitialize(Quest toAdd)
        {
            yield return null;
            toAdd.Initialize();
        }
    }
}
