using UnityEngine;
using TMPro;
using System.Collections;

public class ThoughtParticleSpawner : MonoBehaviour
{
    public GameObject textPrefab;
    public GameObject lookAtTarget;
    public float minSpawnInterval = 0.5f;
    public float maxSpawnInterval = 2f;
    public float lifetime = 5f;
    public float spawnRadius = 2f;
    public Vector3 initialVelocity = new Vector3(0, 2, 0);

    private GameObject activeText;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            // Warte, bis kein aktives Objekt vorhanden ist
            if (activeText == null)
            {
                SpawnTextParticle();
            }

            // Warte eine zufällige Zeit, bevor du den nächsten Versuch machst
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }

    void SpawnTextParticle()
    {
        if (ThoughtCSVReader.Gedanken.Count == 0) return;

        int index = Random.Range(0, ThoughtCSVReader.Gedanken.Count);
        string text = ThoughtCSVReader.Gedanken[index];

        Vector3 spawnPos = transform.position + Random.insideUnitSphere * spawnRadius;
        activeText = Instantiate(textPrefab, spawnPos, transform.rotation);

        var tmp = activeText.GetComponent<TextMeshPro>();
        if (tmp != null) tmp.text = text;

        var rb = activeText.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = initialVelocity + Random.insideUnitSphere;
        }

        if (lookAtTarget != null)
        {
           activeText.transform.LookAt(lookAtTarget.transform);
            activeText.transform.Rotate(0, 180f, 0); // Leserichtung korrigieren

            Vector3 euler = activeText.transform.eulerAngles;
            euler.x = 0f; // X-Rotation auf 0 setzen
            activeText.transform.eulerAngles = euler;
        }

        Destroy(activeText, lifetime);
        StartCoroutine(ClearReferenceAfterLifetime(lifetime));
    }

    IEnumerator ClearReferenceAfterLifetime(float delay)
    {
        yield return new WaitForSeconds(delay);
        activeText = null;
    }
}
