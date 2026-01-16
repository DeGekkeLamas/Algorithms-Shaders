using UnityEngine;

namespace ServiceLocator
{
    public class AchievementServiceIOS : AchievementService
    {
        public override void OnAchievementCompleted()
        {
            Debug.LogWarning("Completed achievement on IOS");
        }
    }
}
