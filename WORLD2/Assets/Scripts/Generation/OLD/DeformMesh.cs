using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GrassChunk))]
public class DeformMesh : MonoBehaviour
{
    [SerializeField]
    private float maxDepression;
    [SerializeField]
    private Vector3[] inputVertecies;
    [SerializeField]
    private Vector3[] deformedVertecies;

    private GrassChunk grassGen;

    void Start()
    {
        grassGen = GetComponent<GrassChunk>();
    }


}
