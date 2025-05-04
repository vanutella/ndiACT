using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSphereColor : MonoBehaviour
{
    [SerializeField] private Material sphereMaterial;

    private float colorChangeTime = 20.0f;

    private Color startColor;
    [SerializeField] private Color targetColor;
    private bool reachedTargetColor = true;
    private int indexColor = 0;


    private Color[] fadeColors = new Color[]
    {
        new Color(0, 0, 1f),      // Blue (0, 0, 255)
        new Color(0, 1f, 1f),      // Cyan (0, 255, 255)
        new Color(1f, 1f, 0),      // Yellow (255, 255, 0)
        new Color(1f, 0, 0),      // Red (255, 0, 0)
        new Color(0, 0, 0),      // Black (0, 0, 0)
        new Color(0, 1f, 0),      // Green (0, 255, 0)
    };

    private void Start()
    {
        sphereMaterial.color = new Color(0, 0, 0);

    }
    // Update is called once per frame
    void Update()
    {
        
        if (reachedTargetColor == true)
        {
            
            
            
                Debug.Log("asdf Should start changing color");
                StartCoroutine(ChangeColorOverTime());
                reachedTargetColor = false;
            
        }
    }

    IEnumerator ChangeColorOverTime()
    {
        Debug.Log("asdf Reached coroutine");

        if (indexColor < fadeColors.Length)
        {
            targetColor = fadeColors[indexColor++];
        }
        else if (indexColor >= fadeColors.Length)
        {
            indexColor = 0;
        }
        startColor = sphereMaterial.color;
        float t = 0f;
        while (t < 1f)
        {
            Debug.Log("Stuck in while");
            t += Time.deltaTime / colorChangeTime;
            sphereMaterial.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }
        sphereMaterial.color = targetColor;
        
        reachedTargetColor = true;
    }
}
