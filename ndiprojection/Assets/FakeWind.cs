using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeWind : MonoBehaviour
{
    MeshFilter mf;
    Vector3[] originalVerts, deformedVerts;
    public float speed = 1f;
    public float amount = 4f;
    public float distortionValue = 0.001f;
    void Start()
    {
        mf = GetComponent<MeshFilter>();
        originalVerts = mf.mesh.vertices;
        deformedVerts = new Vector3[originalVerts.Length];
    }

    void Update()
    {
        for (int i = 0; i < originalVerts.Length; i++)
        {
            Vector3 v = originalVerts[i];
            float sway = Mathf.Sin(Time.time * 0.5f + v.y) * distortionValue * v.y; // Je höher, desto mehr Bewegung
            v.x += sway;
            deformedVerts[i] = v;
        }

        Mesh m = mf.mesh;
        m.vertices = deformedVerts;
        m.RecalculateNormals();

        float swaying = Mathf.Sin(Time.time * speed) * amount;
        transform.localRotation = Quaternion.Euler(0, swaying, 0);
    }
}
