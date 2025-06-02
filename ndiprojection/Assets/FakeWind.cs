using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeWind : MonoBehaviour
{
    public WindZone windZone;

    float movementInterval = 2f;
    private Vector3 lastPosition;
    private float timer = 0f;

    public GameObject personObject;
    public float intensityThreshold = 1f;
    public float movementIntensity = 0f;
    private float accumulatedDistance = 0f;

    MeshFilter mf;
    Vector3[] originalVerts, deformedVerts;
    public float speed = 1f;
    public float amount = 4f;
    public float distortionValue = 0.001f;
    void Start()
    {
        lastPosition = personObject.transform.position;

        mf = GetComponent<MeshFilter>();
        originalVerts = mf.mesh.vertices;
        deformedVerts = new Vector3[originalVerts.Length];
    }

    void Update()
    {
        float delta = Vector3.Distance(transform.position, lastPosition);
        accumulatedDistance += delta;

        lastPosition = personObject.transform.position;
        timer += Time.deltaTime;

        if (timer >= movementInterval)
        {
            movementIntensity = accumulatedDistance / movementInterval; // Durchschnittliche Bewegung pro Sekunde
            Debug.Log($" Intensität: {movementIntensity:F2} | Schwelle: {intensityThreshold}");

            if (movementIntensity >= intensityThreshold)
                Debug.Log("Viel Bewegung erkannt.");
            else
                Debug.Log("Wenig Bewegung");

            // Reset
            accumulatedDistance = 0f;
            timer = 0f;
        }

        // move trees
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
