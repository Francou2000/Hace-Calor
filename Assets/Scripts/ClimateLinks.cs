using System;
using UnityEngine;

[Serializable]
public class ClimateLinkEntry
{
    public string label;
    public string url;
}

/// <summary>
/// Guarda links de concientización y permite abrirlos desde UI (botones OnClick).
/// </summary>
public class ClimateLinks : MonoBehaviour
{
    [SerializeField] private ClimateLinkEntry[] links = new ClimateLinkEntry[4];

    public ClimateLinkEntry[] Links => links;

    public void OpenLinkByIndex(int index)
    {
        if (links == null || index < 0 || index >= links.Length)
        {
            Debug.LogWarning($"[ClimateLinks] Índice fuera de rango: {index}");
            return;
        }

        string url = links[index].url;
        if (string.IsNullOrWhiteSpace(url))
        {
            Debug.LogWarning($"[ClimateLinks] URL vacía para índice {index}.");
            return;
        }

        Application.OpenURL(url);
    }
}
