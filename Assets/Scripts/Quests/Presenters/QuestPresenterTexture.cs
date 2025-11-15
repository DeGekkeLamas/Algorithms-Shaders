using UnityEngine;
using UnityEngine.UI;

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
