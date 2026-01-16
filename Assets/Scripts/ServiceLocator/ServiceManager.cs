using Quests;
using UnityEngine;

namespace ServiceLocator
{
    public class ServiceManager : MonoBehaviour
    {
        enum BuildType { Steam, IOS}
        [SerializeField] BuildType buildType;
        [SerializeField] Achievement[] registeredAchievements;


        AchievementService achievementService;
        void Start()
        {
            foreach(var ach in registeredAchievements)
            {
                ach.Initialize();
            }
            if (buildType == BuildType.Steam)
            {
                achievementService = this.GetComponent<AchievementServiceSteam>();
            }
            else
            {
                achievementService = this.GetComponent<AchievementServiceIOS>();
            }
            ServiceLocatorSystem.RegisterService<AchievementService>(achievementService);
        }

        private void OnDestroy()
        {
            ServiceLocatorSystem.UnregisterService<AchievementService>(achievementService);
            foreach (var ach in registeredAchievements)
            {
                ach.Reset();
            }
        }
    }
}
