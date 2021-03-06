﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSegment : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Vector2[] uv;

    public int xSize = 20;
    public int ySize = 20;

    [Range(1, 5)]
    public float amplitude = 1f;
    [Range(1, 5)]
    public float frequenzy = 1f;
    [Range(1, 5)]
    public float depth = 1f;

    // Start is called before the first frame update
    void Start()
    {
        int[] wallArray = transform.parent.GetComponent<Tile>().wallArray;
        float tileWidth = transform.parent.GetComponent<Tile>().tileWidth;

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        transform.localScale = new Vector3(1 / (float)xSize, transform.localScale.y / (float)ySize, 1 / (float)xSize);

        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        uv = new Vector2[vertices.Length];

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float z = Mathf.PerlinNoise(x * frequenzy, y * frequenzy) * depth;
                vertices[i] = new Vector3(x, y, z);
                uv[i] = new Vector2(x, y);
                i++;
            }
        }

        triangles = new int[xSize * ySize * 6];

        int vert = 0;
        int tris = 0;

        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;

                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }

            vert++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        mesh.RecalculateNormals();
    }
}
