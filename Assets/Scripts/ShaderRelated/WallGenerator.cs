using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class WallGenerator : MonoBehaviour
{
    public float height = 2;
    public float width = 4;
    public float depth = 0.2f;
    public bool generateOnAwake;
    public bool useWorldSpace;

    [Header("Sides to brickify")]
    public bool xPlus;
    public bool xMin;
    public bool zPlus;
    public bool zMin;

    Vector3 originalScale;

    private void Awake() { if (generateOnAwake) GenerateWalls(); }

    [ContextMenu("Generate walls")]
    public void GenerateWalls()
    {
        originalScale = transform.localScale;

        List<Mesh> sides = new();
        if (xPlus) sides.Add(AddSide(0, true));
        if (xMin) sides.Add(AddSide(180, true));
        if (zPlus) sides.Add(AddSide(90, false));
        if (zMin) sides.Add(AddSide(-90, false));

        CombineInstance[] combines = new CombineInstance[sides.Count];
        for(int i = 0; i < sides.Count; i++)
        {
            combines[i].mesh = sides[i];
            combines[i].transform = Matrix4x4.identity;
        }
        Mesh wall = new Mesh();
        wall.CombineMeshes(combines);

        GetComponent<MeshFilter>().mesh = wall;

        transform.localScale = Vector3.one;
        this.AddComponent<BoxCollider>();

        //Debug.Log($"Generated wall, from {this}");
    }
    Mesh AddSide(float angle, bool generateOverXAxis)
    {
        MeshBuilder sideBuilder = new();
        Vector3 offset = new(0, 0, (!generateOverXAxis ? originalScale.x : originalScale.z) * 0.25f);
        Vector3 origin = new(
            -(0.5f * (generateOverXAxis ? originalScale.x : originalScale.z)),
            -(0.5f * originalScale.y),
            -(0.5f * (!generateOverXAxis ? originalScale.x : originalScale.z))
            );

        if (useWorldSpace)
        {
            if (generateOverXAxis) offset.x -= Mathf.Abs(this.transform.position.x % width);
            else offset.x -= Mathf.Abs(this.transform.position.z % width);
        }
        Vector3 initialOffset = offset;
        Vector3 brickRatio = new Vector3(width, height, depth) / Mathf.Max(width, height);

        // continue generation vertically
        for (int i = 0; i < Mathf.Ceil(originalScale.y / height); i++)
        {
            // continue generation horizontally
            for (int j = 0; j < Mathf.Ceil((generateOverXAxis ? originalScale.x : originalScale.z) / width) + (i % 2 != 0 ? 1 : 0); j++)
            {
                //side left
                int v0 = sideBuilder.AddVertex(origin + offset + new Vector3(0, 0, 0),
                    new());
                int v1 = sideBuilder.AddVertex(origin + offset + new Vector3(0, height, 0),
                    new(0, brickRatio.y));
                int v2 = sideBuilder.AddVertex(origin + offset + new Vector3(depth, depth, -depth),
                    new(brickRatio.z, brickRatio.z));
                int v3 = sideBuilder.AddVertex(origin + offset + new Vector3(depth, height - depth, -depth), 
                    new(brickRatio.z, brickRatio.y - brickRatio.z));
                // side right
                int v4 = sideBuilder.AddVertex(origin + offset + new Vector3(width, 0, 0),
                    new(brickRatio.x, 0));
                int v5 = sideBuilder.AddVertex(origin + offset + new Vector3(width, height, 0),
                    new(brickRatio.x, brickRatio.y));
                int v6 = sideBuilder.AddVertex(origin + offset + new Vector3(width - depth, depth, -depth), 
                    new(brickRatio.x - brickRatio.z, brickRatio.z));
                int v7 = sideBuilder.AddVertex(origin + offset + new Vector3(width - depth, height - depth, -depth),
                    new(brickRatio.x - brickRatio.z, brickRatio.y - brickRatio.z));
                // bottom
                int v8 = sideBuilder.AddVertex(origin + offset + new Vector3(0, 0, 0),
                    new());
                int v9 = sideBuilder.AddVertex(origin + offset + new Vector3(depth, depth, -depth),
                    new(brickRatio.z, brickRatio.z));
                int v10 = sideBuilder.AddVertex(origin + offset + new Vector3(width, 0, 0),
                    new(brickRatio.x, 0));
                int v11 = sideBuilder.AddVertex(origin + offset + new Vector3(width - depth, depth, -depth), 
                    new(brickRatio.x - brickRatio.z, brickRatio.z));
                // top
                int v12 = sideBuilder.AddVertex(origin + offset + new Vector3(0, height, 0),
                    new(0, brickRatio.y));
                int v13 = sideBuilder.AddVertex(origin + offset + new Vector3(depth, height - depth, -depth),
                    new(brickRatio.z, brickRatio.y - brickRatio.z));
                int v14 = sideBuilder.AddVertex(origin + offset + new Vector3(width, height, 0),
                    new(brickRatio.x, brickRatio.y));
                int v15 = sideBuilder.AddVertex(origin + offset + new Vector3(width - depth, height - depth, -depth),
                    new(brickRatio.x - brickRatio.z, brickRatio.y - brickRatio.z));
                // front
                int v16 = sideBuilder.AddVertex(origin + offset + new Vector3(depth, depth, -depth), 
                    new(brickRatio.z, brickRatio.z));
                int v17 = sideBuilder.AddVertex(origin + offset + new Vector3(depth, height - depth, -depth), 
                    new(brickRatio.z, brickRatio.y - brickRatio.z));
                int v18 = sideBuilder.AddVertex(origin + offset + new Vector3(width - depth, height - depth, -depth), 
                    new(brickRatio.x - brickRatio.z, brickRatio.y - brickRatio.z));
                int v19 = sideBuilder.AddVertex(origin + offset + new Vector3(width - depth, depth, -depth), 
                    new(brickRatio.x - brickRatio.z, brickRatio.z));

                //side left
                sideBuilder.AddTriangle(v1, v2, v0);
                sideBuilder.AddTriangle(v3, v2, v1);
                //side right
                sideBuilder.AddTriangle(v5, v4, v6);
                sideBuilder.AddTriangle(v7, v5, v6);
                // bottom
                sideBuilder.AddTriangle(v8, v9, v11);
                sideBuilder.AddTriangle(v8, v11, v10);
                // top
                sideBuilder.AddTriangle(v13, v12, v14);
                sideBuilder.AddTriangle(v13, v14, v15);
                // front
                sideBuilder.AddTriangle(v16, v17, v18);
                sideBuilder.AddTriangle(v16, v18, v19);

                offset.x += width;
            }
            offset = new((i % 2 != 0) ? initialOffset.x : initialOffset.x + (-0.5f * width), offset.y + height, offset.z);
        }
        sideBuilder.ClampVerticesX(origin.x, origin.x + (generateOverXAxis ? originalScale.x : originalScale.z));
        sideBuilder.RotateAllVertices(angle);

        return sideBuilder.CreateMesh(true);
    }
}
