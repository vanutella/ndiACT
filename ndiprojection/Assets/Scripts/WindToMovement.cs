using UnityEngine;
using LightBuzz.Kinect4Azure;

public class WindToMovement : MonoBehaviour
{
    public WindZone windZone;


    [Header("Wind Variables")]
    public float defaultWind = 0.3f;
    public float maxWind = 1.5f;
    public float sensitivity = 5f;

    [Header("Movement")]
    public float distanceThreshold = 0.2f;
    public float updateInterval = 2f;

    public Transform trackedObj;
    private Vector3 lastPosition;
    private float accumulatedDistance = 0f;
    private float timer = 0f;
    private bool trackingValid = false;

    void Start()
    {
        timer = updateInterval;
        FindStickman();
    }

    void Update()
    {
       

        // Try to find stickman
        if (trackedObj == null){
           FindStickman();
            Debug.Log("No Stickman found");
            return;
        }

        // Calculate distance
        Vector3 currentPosition = trackedObj.position;

        float delta = Vector3.Distance(currentPosition, lastPosition);
        accumulatedDistance += delta;
        lastPosition = currentPosition;

        Debug.Log("Distance travelled: " + accumulatedDistance);

       

        // Timer runterzählen
        timer -= Time.deltaTime;

        // Nur alle 2 Sekunden Wind berechnen
        if (timer <= 0f)
        {
            float wind = CalculateWind(accumulatedDistance);
            windZone.windMain = wind;

            accumulatedDistance = 0f;
            timer = updateInterval;
            Debug.Log("New Interval starts");
        }
    }

    void FindStickman(){
        GameObject obj = GameObject.Find("Point (2)");
        if (obj != null){
            Debug.Log("Found Obj");
            trackedObj = obj.transform;
            lastPosition = trackedObj.position;
        }
        else{
            trackedObj = null;
        }
    }

    float CalculateWind(float distance)
    {
        if(distance < distanceThreshold){
            return defaultWind;
        }

        float excess = distance - distanceThreshold;
        float extraWind = excess * sensitivity;
        Debug.Log("Viel/genug Bewegung");
        return Mathf.Clamp(defaultWind + extraWind, defaultWind, maxWind);
    }
}