using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class ThoughtCSVReader : MonoBehaviour
{
    public static List<string> Gedanken = new List<string>();

    void Awake()
    {
        LoadThoughts();
    }

    void LoadThoughts()
    {
        TextAsset file = Resources.Load<TextAsset>("Thought1");
        if (file == null)
        {
            Debug.LogError("Thought_extra.csv nicht gefunden!");
            return;
        }

        string[] lines = file.text.Split('\n');
        var quotePattern = new Regex("\"([^\"]+)\"|“([^”]+)”");

        foreach (string line in lines)
        {
            var match = quotePattern.Match(line);
            if (match.Success)
            {
                string text = match.Groups[1].Value;
                if (string.IsNullOrEmpty(text)) text = match.Groups[2].Value;
                if (!string.IsNullOrEmpty(text))
                {
                    Gedanken.Add(text.Trim());
                }
            }
        }
    }
}
