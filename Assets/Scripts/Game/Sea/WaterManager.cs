using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent (typeof(MeshRenderer))]
public class WaterManager : MonoBehaviour
{
    private MeshFilter mFilter;
    private void Awake()
    {
        mFilter = GetComponent<MeshFilter>();
    }

    private void Update()
    {
        Vector3[] vertices = mFilter.mesh.vertices;
        for(int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y=WaveManager.instance.GetWaveHeight(transform.position.x + vertices[i].x);
        }

        mFilter.mesh.vertices = vertices;
        mFilter.mesh.RecalculateNormals();
    }
}
