using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInCircle : MonoBehaviour
{
    public Transform center;     // Mittelpunkt der Kreisbewegung
    public float radius = 50f;   // Radius des Kreises
    public float speed = 1f;     // Rotationsgeschwindigkeit (Umdrehungen pro Sekunde)

    private float angle = 0f;

    void Update()
    {
        angle += speed * Time.deltaTime * 2 * Mathf.PI; // 2π für eine volle Umdrehung

        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        transform.position = new Vector3(center.position.x + x, transform.position.y, center.position.z + z);
    }
}
