using UnityEngine;

public class WindToMovement : MonoBehaviour
{
    public Transform trackedObject;
    public WindZone windZone;

    [Header("Bewegungseinstellungen")]
    public float threshold = 0.02f;
    public float defaultWind = 0.3f;
    public float maxWind = 1.5f;
    public float sensitivity = 5f;
    public float smoothFactor = 0.9f;

    [Header("Abfrageintervall")]
    public float updateInterval = 2f;

    private Vector3 lastPosition;
    private float smoothedMovement = 0f;
    private float windValue = 0f;
    private float timer = 0f;

    void Start()
    {
        if (trackedObject == null)
            trackedObject = transform;

        lastPosition = trackedObject.position;
        timer = updateInterval;
    }

    void Update()
    {
        // Bewegung aufzeichnen – aber nicht sofort Wind setzen
        Vector3 currentPosition = trackedObject.position;
        float movementAmount = Vector3.Distance(currentPosition, lastPosition);
        lastPosition = currentPosition;

        smoothedMovement = Mathf.Lerp(smoothedMovement, movementAmount, 1 - smoothFactor);

        // Timer runterzählen
        timer -= Time.deltaTime;

        // Nur alle 2 Sekunden Wind berechnen
        if (timer <= 0f)
        {
            windValue = CalculateWind(smoothedMovement);
            windZone.windMain = windValue;
            timer = updateInterval;
            Debug.Log("New Interval starts");
        }
    }

    float CalculateWind(float movement)
    {
        if (movement < threshold)
        {
            Debug.Log("Wenig Bewegung");

            return defaultWind;
        }

        float intensity = (movement - threshold) * sensitivity;
        Debug.Log("Viel/genug Bewegung");
        return Mathf.Clamp(defaultWind + intensity, defaultWind, maxWind);
    }
}