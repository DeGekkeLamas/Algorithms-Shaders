using UnityEngine;
using UnityEngine.UI;

namespace Quests.Presenters
{
    public class QuestPresenterTexture : QuestPresenterOnInitialize
    {
        RawImage image;
        private void Awake()
        {
            image = this.GetComponent<RawImage>();
        }
        protected override void UpdateDisplay()
        {
            image.texture = boundQuest.texture;
        }
    }
}
