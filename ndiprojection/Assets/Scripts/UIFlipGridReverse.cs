using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIFlipGridReverse : MonoBehaviour
{
    public GameObject imageTilePrefab;
    public int rows = 6;
    public int columns = 10;
    public Vector2 spacing = new Vector2(60, 60);
    public float animationDuration = 0.8f;
    public float delayBetweenFlips = 0.03f;

    private List<RectTransform> tiles = new List<RectTransform>();

    void Start()
    {
        CreateGrid();
        StartCoroutine(FlipTilesIn());
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

                // Direkt auf 180° setzen (nur Rückseite sichtbar)
                rt.localRotation = Quaternion.Euler(0, 180, 0);

                // Bild unsichtbar lassen
                Image img = rt.GetComponent<Image>();
                img.enabled = false;

                tiles.Add(rt);
            }
        }
    }

    IEnumerator FlipTilesIn()
    {
        List<RectTransform> shuffledTiles = new List<RectTransform>(tiles);
        Shuffle(shuffledTiles);

        foreach (RectTransform tile in shuffledTiles)
        {
            StartCoroutine(FlipTileToFront(tile));
            yield return new WaitForSeconds(delayBetweenFlips);
        }
    }

    IEnumerator FlipTileToFront(RectTransform tile)
    {
        float t = 0f;
        float duration = animationDuration;
        Quaternion startRot = tile.localRotation;
        Quaternion endRot = Quaternion.identity;
        Image img = tile.GetComponent<Image>();

        // Jetzt zeigen wir das Bild wieder
        img.enabled = true;

        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.SmoothStep(0, 1, t / duration);
            tile.localRotation = Quaternion.Slerp(startRot, endRot, lerp);
            yield return null;
        }
    }

    // Shuffle-Hilfe
    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int r = Random.Range(0, i + 1);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }
}
