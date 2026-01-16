using UnityEngine;

namespace ServiceLocator
{
    public class AchievementServiceSteam : AchievementService
    {
        public override void OnAchievementCompleted()
        {
            Debug.LogWarning("Completed achievement on Steam");
        }
    }
}
