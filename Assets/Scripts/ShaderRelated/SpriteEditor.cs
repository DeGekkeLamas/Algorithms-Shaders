using UnityEngine;

public class SpriteEditor
{
    static Color bgColor = new(0.3019608f, 0.3019608f, 0.3019608f);
    static Color otherBgColor = new(0.07450981f, 0.07450981f, 0.07450981f);




    static Texture2D DuplicateTexture(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
    public static Texture2D AddOutline(Texture2D originSprite)
    {
        Color[] colors;
        if (!originSprite.isReadable) colors = DuplicateTexture(originSprite).GetPixels();
        else colors =originSprite.GetPixels();

        Color[] newColors = new Color[colors.Length]; //  (Color[])(colors.Clone());
        Texture2D newSprite = new(originSprite.width, originSprite.height);
        for (int y = 0; y < originSprite.height; y++)
        {
            for (int x = originSprite.width-1; x>=0 ; x--)
            {
                int _index = y * originSprite.width + x;
                Color output = colors[_index];

                if (colors[_index] == bgColor)
                {
                    int offset = 5;
                    bool isEdge = false;
                    int[] _pointsToCheck = new int[8];
                    _pointsToCheck[0] = (y - offset) * originSprite.width + (x - offset);
                    _pointsToCheck[1] = (y - offset) *originSprite.width + x;
                    _pointsToCheck[2] = (y - offset) * originSprite.width + (x + offset);
                    _pointsToCheck[3] = y * originSprite.width + (x - offset);
                    _pointsToCheck[4] = y * originSprite.width + (x + offset);
                    _pointsToCheck[5] = (y + offset) * originSprite.width + (x - offset);
                    _pointsToCheck[6] = (y + offset) * originSprite.width + x;
                    _pointsToCheck[7] = (y + offset) * originSprite.width + (x + offset);

                    foreach (int _point in _pointsToCheck)
                    {
                        if (_point > 0 && _point < newColors.Length && colors[_point] != colors[_index])
                        {
                            isEdge = true;
                            break;
                        }
                    }

                    if (isEdge) output = Color.black;
                }
                newColors[_index] = output;
            }
        }

        newSprite.SetPixels(newColors);
        newSprite.Apply();
        return newSprite;
    }
    public static Texture2D MakeGrayScale(Texture2D originSprite)
    {
        Color[] colors;
        if (!originSprite.isReadable) colors = DuplicateTexture(originSprite).GetPixels();
        else colors = originSprite.GetPixels();

        Color[] newColors = new Color[colors.Length];
        Texture2D newSprite = new(originSprite.width, originSprite.height);

        for(int i = 0; i < colors.Length; i++)
        {
            float average = (colors[i].r + colors[i].g + colors[i].b) / 3;
            newColors[i] = new(average, average, average, 1);
        }

        newSprite.SetPixels(newColors);
        newSprite.Apply();
        return newSprite;
    }
}