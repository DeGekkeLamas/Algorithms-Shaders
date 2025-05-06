using System.Collections;
using System.Text;
using UnityEngine;

[RequireComponent (typeof(MeshRenderer), typeof(MeshFilter))]
public class DrawTilemap : MonoBehaviour
{
    new MeshRenderer renderer;
    public void DrawMap(int[,] tilemap)
    {
        // Get colors from tilemap
        int lengthY = tilemap.GetLength(0);
        int lengthX = tilemap.GetLength(1);
        Texture2D drawnMap = new(lengthY, lengthX);
        Color[] cols = new Color[lengthY * lengthX];

        for(int y = 0; y < lengthY; y++)
        {
            for(int x = 0; x < lengthX; x++)
            {
                cols[y*lengthY + x] = Color.Lerp(Color.white, Color.black, tilemap[y, x]);
            }
        }

        // Create texture
        drawnMap.SetPixels(cols);
        drawnMap.Apply();
        drawnMap.filterMode = FilterMode.Point;

        // Apply texture to object
        renderer = this.GetComponent<MeshRenderer>();
        renderer.sharedMaterial.mainTexture = drawnMap;

        Debug.Log("Drew tilemap");
    }

    [ContextMenu("Export tilemap")]
    void Export()
    {
        Texture2D texture = (Texture2D)renderer.sharedMaterial.mainTexture;
        TextureExporter.ExportTexture(texture);
    }
}
