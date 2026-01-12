using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

/// <summary>
/// Generate a brick wall mesh based off the current objects scale
/// </summary>
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
                AddQuad(sideBuilder,
                    origin + offset + new Vector3(0, 0, 0),
                    origin + offset + new Vector3(0, height, 0),
                    origin + offset + new Vector3(depth, depth, -depth),
                    origin + offset + new Vector3(depth, height - depth, -depth),
                    new(),
                    new(0, brickRatio.y),
                    new(brickRatio.z, brickRatio.z),
                    new(brickRatio.z, brickRatio.y - brickRatio.z));
                // side right
                AddQuad(sideBuilder,
                    origin + offset + new Vector3(width, 0, 0),
                    origin + offset + new Vector3(width - depth, depth, -depth),
                    origin + offset + new Vector3(width, height, 0),
                    origin + offset + new Vector3(width - depth, height - depth, -depth),
                    new(brickRatio.x, 0),
                    new(brickRatio.x - brickRatio.z, brickRatio.z),
                    new(brickRatio.x, brickRatio.y),
                    new(brickRatio.x - brickRatio.z, brickRatio.y - brickRatio.z));
                // bottom
                AddQuad(sideBuilder,
                    origin + offset + new Vector3(0, 0, 0),
                    origin + offset + new Vector3(depth, depth, -depth),
                    origin + offset + new Vector3(width, 0, 0),
                    origin + offset + new Vector3(width - depth, depth, -depth),
                    new(),
                    new(brickRatio.z, brickRatio.z),
                    new(brickRatio.x, 0),
                    new(brickRatio.x - brickRatio.z, brickRatio.z));
                // top
                AddQuad(sideBuilder,
                    origin + offset + new Vector3(0, height, 0),
                    origin + offset + new Vector3(width, height, 0),
                    origin + offset + new Vector3(depth, height - depth, -depth),
                    origin + offset + new Vector3(width - depth, height - depth, -depth),
                    new(0, brickRatio.y),
                    new(brickRatio.x, brickRatio.y),
                    new(brickRatio.z, brickRatio.y - brickRatio.z),
                    new(brickRatio.x - brickRatio.z, brickRatio.y - brickRatio.z));
                // front
                AddQuad(sideBuilder,
                    origin + offset + new Vector3(depth, depth, -depth),
                    origin + offset + new Vector3(depth, height - depth, -depth),
                    origin + offset + new Vector3(width - depth, depth, -depth),
                    origin + offset + new Vector3(width - depth, height - depth, -depth),
                    new(brickRatio.z, brickRatio.z),
                    new(brickRatio.z, brickRatio.y - brickRatio.z),
                    new(brickRatio.x - brickRatio.z, brickRatio.z),
                    new(brickRatio.x - brickRatio.z, brickRatio.y - brickRatio.z));

                offset.x += width;
            }
            offset = new((i % 2 != 0) ? initialOffset.x : initialOffset.x + (-0.5f * width), offset.y + height, offset.z);
        }
        sideBuilder.ClampVerticesX(origin.x, origin.x + (generateOverXAxis ? originalScale.x : originalScale.z));
        sideBuilder.RotateAllVertices(angle);

        return sideBuilder.CreateMesh(true);
    }

    void AddQuad(MeshBuilder builder, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Vector3 vertex4)
    {
        // Verts
        int v0 = builder.AddVertex(vertex1);
        int v1 = builder.AddVertex(vertex2);
        int v2 = builder.AddVertex(vertex3);
        int v3 = builder.AddVertex(vertex4);
        // Tris
        builder.AddTriangle(v1, v2, v0);
        builder.AddTriangle(v3, v2, v1);
    }

    void AddQuad(MeshBuilder builder, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Vector3 vertex4, Vector2 UV1, Vector2 UV2, Vector2 UV3, Vector2 UV4)
    {
        // Verts
        int v0 = builder.AddVertex(vertex1, UV1);
        int v1 = builder.AddVertex(vertex2, UV2);
        int v2 = builder.AddVertex(vertex3, UV3);
        int v3 = builder.AddVertex(vertex4, UV4);
        // Tris
        builder.AddTriangle(v1, v2, v0);
        builder.AddTriangle(v3, v2, v1);
    }
}
