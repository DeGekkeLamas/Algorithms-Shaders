using UnityEngine;

public class SpriteEditor : MonoBehaviour
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
        Color[] colors = DuplicateTexture(originSprite).GetPixels();
        Color[] newColors = colors;
        Texture2D newSprite = new(originSprite.width, originSprite.height);
        for (int i = 0; i < newColors.Length; i++)
        {
            Color output = newColors[i];
            float x = (float)i % originSprite.width / (originSprite.width - 1);
            float y = (float)i % originSprite.height / (originSprite.height - 1);

            if (colors[i] == bgColor)
            {
                output = bgColor;
                if (
                    colors[(int)Mathf.Clamp(x + y * originSprite.width - 10, 0, colors.Length - 1)] != colors[i] ||
                    colors[(int)Mathf.Clamp(x + y * originSprite.width + 10, 0, colors.Length - 1)] != colors[i] ||
                    colors[(int)Mathf.Clamp(x + y * originSprite.width - (10 * originSprite.width), 0, colors.Length - 1)] != colors[i] ||
                    colors[(int)Mathf.Clamp(x + y * originSprite.width + (10 * originSprite.width), 0, colors.Length - 1)] != colors[i]
                    ) output = Color.magenta;
            }
            newColors[i] = output;
        }

        newSprite.SetPixels(colors);
        newSprite.Apply();
        return newSprite;
    }
}
