using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Helper para que cada botón de link se auto-configure desde Inspector.
/// </summary>
[RequireComponent(typeof(Button))]
public class ClimateLinkButton : MonoBehaviour
{
    [SerializeField] private ClimateLinks climateLinks;
    [SerializeField] private int linkIndex;
    [SerializeField] private TMP_Text labelText;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        if (climateLinks == null)
        {
            Debug.LogWarning($"[ClimateLinkButton] Falta ClimateLinks en {name}.");
            return;
        }

        button.onClick.RemoveListener(HandleClick);
        button.onClick.AddListener(HandleClick);

        ApplyLabel();
    }

    private void HandleClick()
    {
        if (climateLinks == null)
        {
            return;
        }

        climateLinks.OpenLinkByIndex(linkIndex);
    }

    private void ApplyLabel()
    {
        if (labelText == null || climateLinks.Links == null)
        {
            return;
        }

        if (linkIndex < 0 || linkIndex >= climateLinks.Links.Length)
        {
            Debug.LogWarning($"[ClimateLinkButton] linkIndex fuera de rango en {name}: {linkIndex}");
            return;
        }

        ClimateLinkEntry entry = climateLinks.Links[linkIndex];
        if (!string.IsNullOrWhiteSpace(entry.label))
        {
            labelText.text = entry.label;
        }
    }
}
