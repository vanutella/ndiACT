using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DynamicLineToObject : MonoBehaviour
{
    public string targetObjectName = "PlayerPlaceholder"; // Name des Zielobjekts in der Szene
    private Transform targetTransform;
    private LineRenderer lineRenderer;

    public Material dottedMaterial;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        // Suche das Zielobjekt in der Szene
        GameObject target = GameObject.Find(targetObjectName);
        if (target != null)
        {
            targetTransform = target.transform;
        }
        else
        {
            Debug.LogError("Zielobjekt mit Name '" + targetObjectName + "' nicht gefunden!");
        }

        // LineRenderer Setup (falls noch nicht konfiguriert)
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.material = dottedMaterial;
        lineRenderer.textureMode = LineTextureMode.Tile;
        //lineRenderer.material.mainTextureScale = new Vector2(10f, 1f);
    }

    void Update()
    {
        if (targetTransform != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, targetTransform.position);
        }
    }
}
