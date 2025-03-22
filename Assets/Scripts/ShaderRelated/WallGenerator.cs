using Unity.VisualScripting;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    public float height = 2;
    public float width = 4;
    public float depth = 0.2f;

    Vector3 originalScale;

    MeshBuilder builder = new();
    private void Awake()
    {
        originalScale = transform.localScale;
        GenerateWalls();
    }
    /**private void OnValidate()
    {
        if (UnityEditor.EditorApplication.isPlaying && didAwake)
            GenerateWalls();
    }**/
    void GenerateWalls()
    {
        bool wallOverZAxis = originalScale.x < originalScale.z;
        originalScale = new(
            (wallOverZAxis) ? originalScale.z : originalScale.x,
            originalScale.y,
            (wallOverZAxis) ? originalScale.x : originalScale.z);

        Vector3 origin = new(
            - (0.5f * originalScale.x),
            - (0.5f * originalScale.y),
            - (0.5f * originalScale.z)
            );

        transform.localScale = Vector3.one;
        Vector3 offset = new(0,0, originalScale.z * 0.25f);

        // generate on both sides
        for(int h = 0; h < 2; h++)
        {
            // continue generation vertically
            for (int i = 0; i < Mathf.Ceil(originalScale.y / height); i++)
            {
                // continue generation horizontally
                for (int j = 0; j < Mathf.Ceil(originalScale.x / width); j++)
                {
                    //side left
                    int v0 = builder.AddVertex(origin + offset + new Vector3(0, 0, 0));
                    int v1 = builder.AddVertex(origin + offset + new Vector3(0, height, 0));
                    int v2 = builder.AddVertex(origin + offset + new Vector3(depth, depth, -depth));
                    int v3 = builder.AddVertex(origin + offset + new Vector3(depth, height - depth, -depth));
                    // side right
                    int v4 = builder.AddVertex(origin + offset + new Vector3(width, 0, 0));
                    int v5 = builder.AddVertex(origin + offset + new Vector3(width, height, 0));
                    int v6 = builder.AddVertex(origin + offset + new Vector3(width - depth, depth, -depth));
                    int v7 = builder.AddVertex(origin + offset + new Vector3(width - depth, height - depth, -depth));
                    // bottom
                    int v8 = builder.AddVertex(origin + offset + new Vector3(0, 0, 0));
                    int v9 = builder.AddVertex(origin + offset + new Vector3(depth, depth, -depth));
                    int v10 = builder.AddVertex(origin + offset + new Vector3(width, 0, 0));
                    int v11 = builder.AddVertex(origin + offset + new Vector3(width - depth, depth, -depth));
                    // top
                    int v12 = builder.AddVertex(origin + offset + new Vector3(0, height, 0));
                    int v13 = builder.AddVertex(origin + offset + new Vector3(depth, height - depth, -depth));
                    int v14 = builder.AddVertex(origin + offset + new Vector3(width, height, 0));
                    int v15 = builder.AddVertex(origin + offset + new Vector3(width - depth, height - depth, -depth));
                    // front
                    int v16 = builder.AddVertex(origin + offset + new Vector3(depth, depth, -depth));
                    int v17 = builder.AddVertex(origin + offset + new Vector3(depth, height - depth, -depth));
                    int v18 = builder.AddVertex(origin + offset + new Vector3(width - depth, height - depth, -depth));
                    int v19 = builder.AddVertex(origin + offset + new Vector3(width - depth, depth, -depth));

                    //side left
                    builder.AddTriangle(v1, v2, v0);
                    builder.AddTriangle(v3, v2, v1);
                    //side right
                    builder.AddTriangle(v5, v4, v6);
                    builder.AddTriangle(v7, v5, v6);
                    // bottom
                    builder.AddTriangle(v8, v9, v11);
                    builder.AddTriangle(v8, v11, v10);
                    // top
                    builder.AddTriangle(v13, v12, v14);
                    builder.AddTriangle(v13, v14, v15);
                    // front
                    builder.AddTriangle(v16, v17, v18);
                    builder.AddTriangle(v16, v18, v19);

                    offset.x += width;
                }
                offset = new((i % 2 != 0) ? 0 : (-0.5f * width), offset.y + height, offset.z);
            }
            offset = new(0, 0, originalScale.z * 0.25f);
            builder.RotateVertices(180);
        }
        if (wallOverZAxis) builder.RotateVertices(90);
        GetComponent<MeshFilter>().mesh = builder.CreateMesh(true);

        this.AddComponent<BoxCollider>();

        Debug.Log($"Generated wall, from {this}");
    }
}
