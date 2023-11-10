using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ocean : MonoBehaviour
{

    public int area = 10;

    public Wave[] waves;
    public float UVSize;

    protected MeshFilter meshFilter;
    protected Mesh mesh;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        mesh.name = gameObject.name;

        mesh.vertices = GenerateVertices();
        mesh.triangles = GenerateTriangles();
        mesh.uv = GenerateUV();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        var vertices = mesh.vertices;
        for (int x = 0; x <= area; x++)
        {
            for (int z = 0; z <= area; z++)
            {
                var y = 0f;
                for (int i = 0; i < waves.Length; i++)
                {
                    if (waves[i].alternate)
                    {
                        var perlin = Mathf.PerlinNoise((x * waves[i].size.x) / area, (z * waves[i].size.y) / area) *
                                     Mathf.PI * 2f;
                        
                        y += Mathf.Cos(perlin + waves[i].velocity.magnitude * Time.time) * waves[i].height;
                    }
                    else
                    {
                        var perlin = Mathf.PerlinNoise((x * waves[i].size.x + Time.time + waves[i].velocity.x) / area,
                            (z * waves[i].size.y + Time.time * waves[i].velocity.y) / area) - 0.5f;

                        y += perlin * waves[i].height;
                    }
                }
                
                vertices[indicies(x, z)] = new Vector3(x, y, z);
            }
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }

    private Vector3[] GenerateVertices()
    {
        var vertices = new Vector3[(area + 1) * (area + 1)];

        for (int x = 0; x <= area; x++)
        {
            for (int z = 0; z <= area; z++)
            {
                vertices[indicies(x, z)] = new Vector3(x, 0, z);
            }
        }

        return vertices;
    }

    private int indicies(float x, float z)
    {
        return (int)( x * (area + 1) + z);
    }

    private int[] GenerateTriangles()
    {
        var triangles = new int [mesh.vertices.Length * 6]; // 2 triangles, 6 points

        for (int x = 0; x < area; x++)
        {
            for (int z = 0; z < area; z++)
            {
                triangles[indicies(x, z) * 6 + 0] = indicies(x, z);
                triangles[indicies(x, z) * 6 + 1] = indicies(x + 1, z + 1); // First triangle
                triangles[indicies(x, z) * 6 + 2] = indicies(x + 1, z);
                
                triangles[indicies(x, z) * 6 + 3] = indicies(x, z);
                triangles[indicies(x, z) * 6 + 4] = indicies(x, z + 1); // Second triangle
                triangles[indicies(x, z) * 6 + 5] = indicies(x + 1, z + 1);
            }
        }

        return triangles;
    }

    private Vector2[] GenerateUV()
    {
        var uv = new Vector2[mesh.vertices.Length];

        for (int x = 0; x <= area; x++)
        {
            for (int z = 0; z <= area; z++)
            {
                var vec = new Vector2((x / UVSize) % 2, (z / UVSize) % 2);
                uv[indicies(x, z)] = new Vector2(vec.x <= 1 ? vec.x : 2 - vec.x, vec.y <= 1 ? vec.y : 2 - vec.y);
            }
        }

        return uv;
    }

    public float CalculateHeight(Vector3 position)
    {
        var scale = new Vector3(1 / transform.lossyScale.x, 0, 1 / transform.lossyScale.z);
        var pos = Vector3.Scale((position - transform.position), scale);

        var p1 = new Vector3(Mathf.Floor(pos.x), 0, Mathf.Floor(pos.z));
        var p2 = new Vector3(Mathf.Floor(pos.x), 0, Mathf.Ceil(pos.z));
        var p3 = new Vector3(Mathf.Ceil(pos.x), 0, Mathf.Floor(pos.z));
        var p4 = new Vector3(Mathf.Ceil(pos.x), 0, Mathf.Ceil(pos.z));

        p1.x = Mathf.Clamp(p1.x, 0, area);
        p1.z = Mathf.Clamp(p1.z, 0, area);
        p2.x = Mathf.Clamp(p2.x, 0, area);
        p2.z = Mathf.Clamp(p2.z, 0, area);
        p3.x = Mathf.Clamp(p3.x, 0, area);    // Clamp makes the values stay within the desired range
        p3.z = Mathf.Clamp(p3.z, 0, area);
        p4.x = Mathf.Clamp(p4.x, 0, area);
        p4.z = Mathf.Clamp(p4.z, 0, area);

        var distMax = Mathf.Max(Vector3.Distance(p1, pos), Vector3.Distance(p2, pos), Vector3.Distance(p3, pos),
            Vector3.Distance(p4, pos) + Mathf.Epsilon);

        var dist = (distMax - Vector3.Distance(p1, pos)) +
                   (distMax - Vector3.Distance(p2, pos)) +
                   (distMax - Vector3.Distance(p3, pos)) +
                   (distMax - Vector3.Distance(p4, pos) + Mathf.Epsilon);

        var heightSum = mesh.vertices[indicies(p1.x, p1.z)].y * (distMax - Vector3.Distance(p1, pos)) +
                        mesh.vertices[indicies(p2.x, p2.z)].y * (distMax - Vector3.Distance(p2, pos)) +
                        mesh.vertices[indicies(p3.x, p3.z)].y * (distMax - Vector3.Distance(p3, pos)) +
                        mesh.vertices[indicies(p4.x, p4.z)].y * (distMax - Vector3.Distance(p4, pos));

        return heightSum * transform.lossyScale.y / dist;
    }

    [Serializable]
    public struct Wave
    {
        public Vector2 velocity;
        public Vector2 size;
        public float height;
        public bool alternate;
    }
}
