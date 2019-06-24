using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour{

    Mesh mesh;

    Vector3[] vertecies;
    int[] triangles;

    [SerializeField]
    private int xSize = 32;
    [SerializeField]
    private int zSize = 32;
    [SerializeField]
    private int updateTime = 30;
    [SerializeField]
    private Transform followPosition;
    [SerializeField]
    private Vector2 terrainOffset;

    [SerializeField]
    private int Scale = 1;

    private int terrainLoadCounter;
    private Vector2 lastCamPos;

    void Start()
    {
        mesh = new Mesh();

        GetComponent<MeshFilter>().mesh = mesh;
        if (GetComponent<MeshCollider>() != null)
        {
            GetComponent<MeshCollider>().sharedMesh = mesh;
        }
        else { Debug.LogWarning("There is no collider on this object!"); }
    }

    private void FixedUpdate()
    {
        if (followPosition != null)
        {
            TerrainFollow();
        }
    }

    void CreateShape()
    {
        vertecies = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = TerrainNoise(((float)x / Scale + (float)transform.position.x), ((float)z / Scale + (float)transform.position.z));
                vertecies[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
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

        mesh.vertices = vertecies;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    float TerrainNoise(float x, float z)
    {
        float y = Mathf.PerlinNoise(x * .7f, z * .7f) * .5f;
        y += Mathf.PerlinNoise(x * .08f, z * .08f) * 2.5f * 2;
        return y;
    }

    void TerrainFollow()
    {
        if (terrainLoadCounter <= -updateTime && lastCamPos != new Vector2((int)followPosition.position.x, (int)followPosition.position.z))
        {
            terrainLoadCounter = 0;
            lastCamPos = new Vector2((int)followPosition.position.x, (int)followPosition.position.z);

            //Debug.Log("Updated Terrain!");

        }
        else
        {
            terrainLoadCounter--;
            return;
        }

        transform.position = new Vector3(
            (int)followPosition.position.x - (xSize * (1 / Scale)) + terrainOffset.x,
            0,
            (int)followPosition.position.z - (zSize * (1/ Scale) + terrainOffset.y)
            );

        CreateShape();
        UpdateMesh();
    }

    //private void OnDrawGizmos()
    //{
    //    if (vertecies == null)
    //    {
    //        return;
    //    }

    //    for (int i = 0; i < vertecies.Length; i++)
    //    {
    //        Gizmos.DrawSphere(vertecies[i], .05f);
    //    } camera
    //}
}