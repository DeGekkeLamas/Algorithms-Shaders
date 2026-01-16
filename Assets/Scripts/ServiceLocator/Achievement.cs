using Quests;
using UnityEngine;

namespace ServiceLocator
{
    [CreateAssetMenu(fileName = "Achievement", menuName = "ScriptableObjects/Achievement")]
    public class Achievement : ScriptableObject
    {
        public Quest boundQuest;

        public void Initialize()
        {
            if (!QuestManager.instance.IsActive(boundQuest)) boundQuest.Initialize();

            boundQuest.OnComplete += Complete;
        }
        public void Reset()
        {
            boundQuest.OnComplete -= Complete;
            boundQuest.Destructor();
        }

        void Complete()
        {
            ServiceLocatorSystem.GetService<AchievementService>().OnAchievementCompleted();
        }
    }
}
