using TMPro;
using UnityEngine;

/// <summary>
/// Maneja textos de HUD y panel de fin de juego.
/// Requiere TextMeshPro (TMP_Text). Si tu proyecto no lo tiene, reemplazar TMP_Text por UnityEngine.UI.Text.
/// </summary>
public class GameUI : MonoBehaviour
{
    [Header("HUD en partida")]
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private TMP_Text survivalTimeText;
    [SerializeField] private TMP_Text heatText;
    [SerializeField] private TMP_Text massText;
    [SerializeField] private TMP_Text instructionText;

    [Header("Panel de Game Over")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text gameOverTitleText;
    [SerializeField] private TMP_Text finalTimeText;
    [SerializeField, TextArea] private TMP_Text awarenessText;

    [Header("Links climáticos")]
    [SerializeField] private ClimateLinks climateLinks;
    [SerializeField] private TMP_Text[] linkTexts;

    private const string DefaultInstruction = "Hacé click para mantener frío al glaciar.";
    private const string DefaultTitle = "El glaciar se derritió.";
    private const string DefaultAwareness = "El aumento de temperatura afecta a los glaciares, las reservas de agua y los ecosistemas. Informate sobre las proyecciones climáticas y sus impactos en Argentina.";

    private void Awake()
    {
        ValidateReferences();
        SetupStaticTexts();
    }

    public void RefreshPlayingData(float survivalTime, float heat, float mass)
    {
        if (survivalTimeText != null) survivalTimeText.text = $"Tiempo: {survivalTime:0.0}s";
        if (heatText != null) heatText.text = $"Calor: {heat:0.0}";
        if (massText != null) massText.text = $"Masa glaciar: {mass:0.0}%";
    }

    public void ShowGameOver(float survivalTime)
    {
        SetInGameVisible(true); // mantiene HUD + instrucción de contexto, opcional
        SetGameOverVisible(true);

        if (finalTimeText != null)
        {
            finalTimeText.text = $"Sobreviviste: {survivalTime:0.0} segundos.";
        }
    }

    public void SetInGameVisible(bool visible)
    {
        if (inGamePanel != null) inGamePanel.SetActive(visible);
    }

    public void SetGameOverVisible(bool visible)
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(visible);
    }

    private void SetupStaticTexts()
    {
        if (instructionText != null && string.IsNullOrWhiteSpace(instructionText.text))
        {
            instructionText.text = DefaultInstruction;
        }

        if (gameOverTitleText != null && string.IsNullOrWhiteSpace(gameOverTitleText.text))
        {
            gameOverTitleText.text = DefaultTitle;
        }

        if (awarenessText != null && string.IsNullOrWhiteSpace(awarenessText.text))
        {
            awarenessText.text = DefaultAwareness;
        }

        if (climateLinks != null && linkTexts != null)
        {
            ClimateLinkEntry[] entries = climateLinks.Links;
            int count = Mathf.Min(entries.Length, linkTexts.Length);
            for (int i = 0; i < count; i++)
            {
                if (linkTexts[i] != null && !string.IsNullOrWhiteSpace(entries[i].label))
                {
                    linkTexts[i].text = entries[i].label;
                }
            }
        }
    }

    private void ValidateReferences()
    {
        if (inGamePanel == null) Debug.LogWarning("[GameUI] Falta inGamePanel.");
        if (gameOverPanel == null) Debug.LogWarning("[GameUI] Falta gameOverPanel.");
        if (survivalTimeText == null) Debug.LogWarning("[GameUI] Falta survivalTimeText.");
        if (heatText == null) Debug.LogWarning("[GameUI] Falta heatText.");
        if (massText == null) Debug.LogWarning("[GameUI] Falta massText.");
        if (instructionText == null) Debug.LogWarning("[GameUI] Falta instructionText.");
        if (gameOverTitleText == null) Debug.LogWarning("[GameUI] Falta gameOverTitleText.");
        if (finalTimeText == null) Debug.LogWarning("[GameUI] Falta finalTimeText.");
        if (awarenessText == null) Debug.LogWarning("[GameUI] Falta awarenessText.");
        if (climateLinks == null) Debug.LogWarning("[GameUI] Falta climateLinks.");
    }
}
