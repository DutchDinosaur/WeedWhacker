using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGeneratorWithSubdivide : MonoBehaviour{

    Mesh mesh;

    Vector3[] vertecies;
    int[] triangles;

    public int xSize = 32;
    public int zSize = 32;

    [SerializeField]
    private int subDivides = 2;
    private float subDivideMultiplier;

    private int terrainLoadCounter;
    private Vector2 lastCamPos;

    void Start()
    {
        mesh = new Mesh();
        subDivideMultiplier = 1 / subDivides;

        GetComponent<MeshFilter>().mesh = mesh;
        if (GetComponent<MeshCollider>() != null)
        {
            GetComponent<MeshCollider>().sharedMesh = mesh;
        }
        else { Debug.LogWarning("There is no collider on this object!"); }

        CreateShape();
        UpdateMesh();
    }

    public void CreateShape()
    {

        int ver = (xSize + 1) * (zSize + 1);
        vertecies = new Vector3[(int)Mathf.Pow(ver, subDivides)];
        
        for (int i = 0, z = 0; z <= zSize * subDivides; z++)
        {
            for (int x = 0; x <= xSize * subDivides; x++)
            {
                vertecies[i] = new Vector3((float)x / subDivides, 0, (float)z / subDivides);
                vertecies[i].y = TerrainNoise(((float)x / subDivides + transform.position.x), ((float)z / subDivides + transform.position.z));
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6 * subDivides * subDivides];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize * subDivides; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                for (int i = 0; i < subDivides; i++)
                {
                    triangles[tris + 0] = vert + 0;
                    triangles[tris + 1] = vert + xSize * subDivides + 1;
                    triangles[tris + 2] = vert + 1;
                    triangles[tris + 3] = vert + 1;
                    triangles[tris + 4] = vert + xSize * subDivides + 1;
                    triangles[tris + 5] = vert + xSize * subDivides + 2;

                    vert++;
                    tris += 6;
                }
            }
            vert++;
        }
    }

    public void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertecies;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    float TerrainNoise(float x, float z)
    {
        float y = Mathf.PerlinNoise(x * .7f, z * .7f) * .5f;
        y += Mathf.PerlinNoise(x * .08f, z * .08f) * 2.5f;
        return y;
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
    //    }
    //}
}