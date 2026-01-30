using NaughtyAttributes;
using UnityEngine;

public class ColorTextureGenerator : MonoBehaviour
{
    [SerializeField] Color[] colorsToUse;

    [Button]
    void CreateTexture()
    {
        Texture2D tex = new(colorsToUse.Length, 1);
        tex.SetPixels(colorsToUse);
        tex.Apply();
        TextureExporter.ExportTexture(tex, $"ColorTexture{colorsToUse.Length}");
    }
}
