using UnityEngine;
using UnityEngine.UI;

public class QuestPresenterTexture : QuestPresenterOnInitialize
{
    public RawImage image;
    protected override void SetDisplay()
    {
        image.texture = boundQuest.texture;
    }
}
