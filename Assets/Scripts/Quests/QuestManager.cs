using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine.UI;
using Quests.Presenters;
using Entities;

namespace Quests
{
    /// <summary>
    /// Singleton for managing active quests
    /// </summary>
    public class QuestManager : MonoBehaviour
    {
        List<Quest> activeQuests = new();
        [SerializeField] GameObject questPresenter;
        [SerializeField] HorizontalOrVerticalLayoutGroup layoutGroup;

        [Header("Quests to give")]
        [SerializeField] List<Quest> registeredQuests;
        [SerializeField] int amountToGive = 2;

        public static QuestManager instance;

        private void Awake()
        {
            instance = this;
            for (int i = 0; i < Mathf.Min(registeredQuests.Count, amountToGive); i++)
            {
                int index = Random.Range(0, registeredQuests.Count);
                AddQuest(registeredQuests[index]);
                registeredQuests.RemoveAt(index);
            }
        }

        /// <summary>
        /// Adds a quest to activeQuests. Is it NOT removed from registeredQuests, remove it from the code where this function is called
        /// </summary>
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
            // Wait a frame between setting the quest and activating initilize, so the presenters can initialize first
            StartCoroutine(WaitThenInitialize(toAdd));
        }

        public bool IsActive(Quest toCheck)
        {
            return activeQuests.Contains(toCheck);
        }

        IEnumerator WaitThenInitialize(Quest toAdd)
        {
            yield return null;
            toAdd.Initialize();
        }

        private void OnDestroy()
        {
            foreach(Quest quest in activeQuests)
            {
                quest.Destructor();
            }
        }
    }
}
