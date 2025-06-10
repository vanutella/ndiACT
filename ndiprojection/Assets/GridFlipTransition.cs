using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIFlipGrid : MonoBehaviour
{
    public GameObject imageTilePrefab; // Dein UI-Image Prefab
    public int rows = 6;
    public int columns = 10;
    public Vector2 spacing = new Vector2(60, 60); // Abstand in px
    public float animationDuration = 0.8f;
    public float delayBetweenFlips = 0.03f;

    private List<RectTransform> tiles = new List<RectTransform>();

    void Start()
    {
        CreateGrid();
        StartCoroutine(FlipTiles());
    }

    void CreateGrid()
    {
        RectTransform parent = GetComponent<RectTransform>();
        Vector2 startPos = new Vector2(
            -((columns - 1) * spacing.x) / 2,
            ((rows - 1) * spacing.y) / 2
        );

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector2 pos = startPos + new Vector2(x * spacing.x, -y * spacing.y);
                GameObject tile = Instantiate(imageTilePrefab, parent);
                RectTransform rt = tile.GetComponent<RectTransform>();
                rt.anchoredPosition = pos;
                tiles.Add(rt);
            }
        }
    }

    IEnumerator FlipTiles()
    {
        List<RectTransform> shuffledTiles = new List<RectTransform>(tiles);
        Shuffle(shuffledTiles);

        foreach (RectTransform tile in shuffledTiles)
        {
            StartCoroutine(FlipTile(tile));
            yield return new WaitForSeconds(delayBetweenFlips);
        }
    }

    IEnumerator FlipTile(RectTransform tile)
    {
        float t = 0f;
        float duration = animationDuration;
        Quaternion startRot = Quaternion.identity;
        Quaternion endRot = Quaternion.Euler(0, 180, 0);
        Image img = tile.GetComponent<Image>();

        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.SmoothStep(0, 1, t / duration);
            tile.localRotation = Quaternion.Slerp(startRot, endRot, lerp);
            yield return null;
        }

        // Deaktiviere oder blende aus nach 180°
        img.enabled = false;
    }

    // Fisher-Yates Shuffle
    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int r = Random.Range(0, i + 1);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }
}
