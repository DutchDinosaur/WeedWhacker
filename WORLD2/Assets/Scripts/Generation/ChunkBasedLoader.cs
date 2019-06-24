using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkBasedLoader : MonoBehaviour
{

    [SerializeField]
    private Transform trackPosition;

    [SerializeField]
    private GameObject ObjectPrefab;
    public int RenderDistance = 3;
    public int DeleteRange = 10;

    [SerializeField]
    private int chunkSize = 32;

    [SerializeField]
    private int updateTime = 30;
    private int terrainLoadCounter = 0;
    private int chunkCount;

    private Vector2 tempMidChunkPos;

    private void Start()
    {
        chunkCount = (RenderDistance * 2) * (RenderDistance * 2);
        tempMidChunkPos = Vector2.zero;

        for (int i = 0; i < chunkCount; i++)
        {
            Instantiate(ObjectPrefab, transform);
        }

        for (int i = 0, x = 0; x < RenderDistance * 2; x++)
        {
            for (int z = 0; z < RenderDistance * 2; z++, i++)
            {
                transform.GetChild(i).position = new Vector3((GetMidChunkPos().x - RenderDistance * chunkSize) + x * chunkSize, 0, (GetMidChunkPos().y - RenderDistance * chunkSize) + z * chunkSize);
            }
        }
    }

    private void FixedUpdate()
    {
        if (terrainLoadCounter <= -updateTime)
        {
            terrainLoadCounter = 0;

            if (tempMidChunkPos.x == GetMidChunkPos().x && tempMidChunkPos.y == GetMidChunkPos().y)
            {
                return;
            }

            tempMidChunkPos = GetMidChunkPos();
        }
        else
        {
            terrainLoadCounter--;
            return;
        }


        List<Vector2> chunkPositions = new List<Vector2>();
        Vector2 MidChunkPos = GetMidChunkPos();

        for (int i = 0,x = 0; x < RenderDistance * 2; x++)
        {
            for (int z = 0; z < RenderDistance * 2; z++, i++)
            {
                chunkPositions.Add (new Vector2((MidChunkPos.x - RenderDistance * chunkSize) + x * chunkSize, (MidChunkPos.y - RenderDistance * chunkSize) + z * chunkSize));
            }
        }

        for (int a = 0; a < transform.childCount; a++)
        {
            Vector2 chunkPos = new Vector2(transform.GetChild(a).transform.position.x, transform.GetChild(a).transform.position.z);
            for (int b = 0; b < chunkPositions.Count; b++)
            {
                if (chunkPositions[b].x == chunkPos.x && chunkPositions[b].y == chunkPos.y)
                {
                    chunkPositions.RemoveAt(b);
                    //break;
                }
                if (chunkPos.x > DeleteRange * chunkSize + MidChunkPos.x || chunkPos.x < -DeleteRange * chunkSize + MidChunkPos.x || chunkPos.y > DeleteRange * chunkSize + MidChunkPos.y || chunkPos.y < -DeleteRange * chunkSize + MidChunkPos.y)
                {
                    //transform.GetChild(a).SetParent(objectsPool); //very laggy
                    Destroy( transform.GetChild(a).gameObject);
                }
            }
        }

        Debug.Log("LOADED: "+chunkPositions.Count+ "CHUNKS!");

        for (int i = 0; i < chunkPositions.Count; i++)
        {
            Vector3 Pos = new Vector3(chunkPositions[i].x, 0, chunkPositions[i].y);

            Instantiate(ObjectPrefab, Pos, Quaternion.identity, transform);

            //Transform chunkToSpawn = objectsPool.GetChild(0); //very laggy
            //chunkToSpawn.position = Pos;
            //chunkToSpawn.GetComponent<MeshGeneratorStartWithSubdivide>().CreateShape();
            //chunkToSpawn.GetComponent<MeshGeneratorStartWithSubdivide>().UpdateMesh();
        }
    }

    Vector2 GetMidChunkPos()
    {
        return new Vector2(trackPosition.position.x - trackPosition.position.x % chunkSize, trackPosition.position.z - trackPosition.position.z % chunkSize);
    }
}