using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mowable : MonoBehaviour
{

    [SerializeField]
    private MeshRenderer grass;
    [SerializeField]
    private MeshRenderer ground;
    [SerializeField]
    private int ChunkSize;
    [SerializeField]
    private int TexSize;

    manager manager;


    private GameObject[] mower;
//    private GameObject[] wheels;

    private BitArray isMowed;
    private Texture2D Mowed;
    private Texture2D MowedPoint;

    void Start()
    {
        //isMowed = new bool[TexSize * TexSize];
        mower = GameObject.FindGameObjectsWithTag("MOWER");
        //wheels = GameObject.FindGameObjectsWithTag("WHEEL");

        manager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<manager>();

        Mowed = new Texture2D(TexSize, TexSize, TextureFormat.ARGB32, false);
        Mowed.filterMode = FilterMode.Bilinear;
        MowedPoint = new Texture2D(TexSize, TexSize, TextureFormat.ARGB32, false);
        MowedPoint.filterMode = FilterMode.Point;

        for (int x = 0; x < TexSize; x++)
        {
            for (int z = 0; z < TexSize; z++)
            {

                MowedPoint.SetPixel(x, z, Color.white);
                Mowed.SetPixel(x, z, Color.white);
            }
        }
        Mowed.Apply();
        MowedPoint.Apply();
        
    }
    
    public void refresh()
    {
        mower = GameObject.FindGameObjectsWithTag("MOWER");
    }

    void Update()
    {
        for (int i = 0; i < mower.Length; i++)
        {
            MowCordinate(mower[i].transform.position.x, mower[i].transform.position.z);
        }

    }

    void MowCordinate(float x, float z)
    {
        if (x > transform.position.x && x < transform.position.x + ChunkSize && z > transform.position.z && z < transform.position.z + ChunkSize)
        {
            Vector2Int Mowpos = WorldToGrassPos(x, z);

            MowPixel(Mowpos);

            if (manager.MowUpgrade)
            {
                MowPixel(new Vector2Int(Mowpos.x - 1, Mowpos.y));
                MowPixel(new Vector2Int(Mowpos.x + 1, Mowpos.y));
                MowPixel(new Vector2Int(Mowpos.x, Mowpos.y - 1));
                MowPixel(new Vector2Int(Mowpos.x, Mowpos.y + 1));
            }
            

            grass.material.SetTexture("_MOWED", MowedPoint);
            ground.material.SetTexture("_MOWED", Mowed);
            
        }
    }

    void MowPixel(Vector2Int pixel)
    {
        if (Mowed.GetPixel(pixel.x, pixel.y) == Color.white)
        {
            if (manager.time > 0)
            {
                manager.Score += 1;
            }

            manager.mowed += 1;
            Mowed.SetPixel(pixel.x, pixel.y, Color.grey);
            Mowed.Apply();

            MowedPoint.SetPixel(pixel.x, pixel.y, Color.grey);
            MowedPoint.Apply();
        }
    }

    Vector2Int WorldToGrassPos(float x, float z)
    {
        //Vector2 Pos = new Vector2(((float)(x - x % (float)ChunkSize) / (float)ChunkSize) * (float)TexSize, ((float)(z - z % (float)ChunkSize) / (float)ChunkSize) * (float)TexSize);
        Vector2 Pos = new Vector2(((/*Mathf.Abs(x)*/ x % (float)ChunkSize) / (float)ChunkSize) * (float)TexSize, ((/*Mathf.Abs(z)*/ z % (float)ChunkSize) / (float)ChunkSize) * (float)TexSize);

        Vector2Int PosFixed = new Vector2Int((int)Pos.x, (int)Pos.y);

        if (x < 0)
        {
            PosFixed.x = TexSize - Mathf.Abs(PosFixed.x) - 1;
        }
        if (z < 0)
        {
            PosFixed.y = TexSize - Mathf.Abs(PosFixed.y) - 1;
        }

        //Debug.Log(((x % (float)ChunkSize) / (float)ChunkSize));
        //return new Vector2Int((int)Pos.x, (int)Pos.y);
        return PosFixed;
    }
}