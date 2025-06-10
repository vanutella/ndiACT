using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIFlipGridController : MonoBehaviour
{
    public GameObject imageTilePrefab;
    public int rows = 6;
    public int columns = 10;
    public Vector2 spacing = new Vector2(60, 60);
    public float animationDuration = 0.8f;
    public float delayBetweenFlips = 0.03f;
    public float destroyDelayAfterAnimation = 2f;

    private List<GameObject> createdTiles = new List<GameObject>();
    private bool isTransitioning = false;

    public void OpenGrid()
    {
        if (isTransitioning) return;
        StartCoroutine(FlipRoutine(opening: true));
        Debug.Log("Opening...");
    }

    public void CloseGrid()
    {
        if (isTransitioning) return;
        StartCoroutine(FlipRoutine(opening: false));
    }

    IEnumerator FlipRoutine(bool opening)
    {
        isTransitioning = true;

        CreateGrid(opening);

        // Liste zum Mischen
        List<RectTransform> tiles = new List<RectTransform>();
        foreach (var tileObj in createdTiles)
        {
            tiles.Add(tileObj.GetComponent<RectTransform>());
        }
        Shuffle(tiles);

        // Flip nacheinander
        foreach (RectTransform tile in tiles)
        {
            StartCoroutine(FlipTile(tile, opening));
            yield return new WaitForSeconds(delayBetweenFlips);
        }

        float totalDuration = animationDuration + delayBetweenFlips * tiles.Count;
        yield return new WaitForSeconds(totalDuration + destroyDelayAfterAnimation);

        // Aufräumen
        foreach (GameObject obj in createdTiles)
        {
            Destroy(obj);
        }

        createdTiles.Clear();
        isTransitioning = false;
    }

    void CreateGrid(bool openingState)
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

                // Wenn geschlossen wird → Start auf 180°, sonst auf 0°
                rt.localRotation = Quaternion.Euler(0, openingState ? 0 : 180, 0);

                Image img = tile.GetComponent<Image>();
                img.enabled = openingState; // Nur beim Öffnen sichtbar

                createdTiles.Add(tile);
            }
        }
    }

    IEnumerator FlipTile(RectTransform tile, bool opening)
    {
        float t = 0f;
        Quaternion startRot = tile.localRotation;
        Quaternion endRot = Quaternion.Euler(0, opening ? 180 : 0, 0);
        Image img = tile.GetComponent<Image>();

        if (!opening)
            img.enabled = true; // Beim Schließen aktivieren

        while (t < animationDuration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.SmoothStep(0, 1, t / animationDuration);
            tile.localRotation = Quaternion.Slerp(startRot, endRot, lerp);
            yield return null;
        }

        if (opening)
            img.enabled = false; // Nach Öffnung ausblenden
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int r = Random.Range(0, i + 1);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }
}
