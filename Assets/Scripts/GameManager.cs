using System;
using UnityEngine;

/// <summary>
/// Estados globales del juego.
/// </summary>
public enum GameState
{
    Playing,
    GameOver
}

/// <summary>
/// Orquesta el loop principal: arranca automáticamente, cuenta tiempo y finaliza la partida.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GlacierController glacierController;
    [SerializeField] private HeatSystem heatSystem;
    [SerializeField] private GameUI gameUI;

    public GameState CurrentState { get; private set; } = GameState.Playing;
    public float SurvivalTime { get; private set; }

    public event Action<float> OnSurvivalTimeChanged;
    public event Action<float> OnGameOver;

    private void Awake()
    {
        if (glacierController == null)
        {
            Debug.LogError("[GameManager] Falta asignar GlacierController en el Inspector.");
        }

        if (heatSystem == null)
        {
            Debug.LogError("[GameManager] Falta asignar HeatSystem en el Inspector.");
        }

        if (gameUI == null)
        {
            Debug.LogError("[GameManager] Falta asignar GameUI en el Inspector.");
        }
    }

    private void OnEnable()
    {
        if (glacierController != null)
        {
            glacierController.OnFullyMelted += HandleGlacierFullyMelted;
        }
    }

    private void OnDisable()
    {
        if (glacierController != null)
        {
            glacierController.OnFullyMelted -= HandleGlacierFullyMelted;
        }
    }

    private void Start()
    {
        // El juego inicia automáticamente al cargar la escena.
        CurrentState = GameState.Playing;
        SurvivalTime = 0f;

        if (glacierController != null)
        {
            glacierController.ResetGlacier();
        }

        if (heatSystem != null)
        {
            heatSystem.ResetHeat();
        }

        if (gameUI != null)
        {
            gameUI.SetInGameVisible(true);
            gameUI.SetGameOverVisible(false);
            gameUI.RefreshPlayingData(SurvivalTime, heatSystem != null ? heatSystem.CurrentHeat : 0f, glacierController != null ? glacierController.CurrentMass : 0f);
        }
    }

    private void Update()
    {
        if (CurrentState != GameState.Playing)
        {
            return;
        }

        SurvivalTime += Time.deltaTime;
        OnSurvivalTimeChanged?.Invoke(SurvivalTime);

        if (heatSystem != null && glacierController != null)
        {
            heatSystem.Tick(Time.deltaTime, SurvivalTime, glacierController);
        }

        if (gameUI != null && heatSystem != null && glacierController != null)
        {
            gameUI.RefreshPlayingData(SurvivalTime, heatSystem.CurrentHeat, glacierController.CurrentMass);
        }
    }

    private void HandleGlacierFullyMelted()
    {
        if (CurrentState == GameState.GameOver)
        {
            return;
        }

        CurrentState = GameState.GameOver;
        OnGameOver?.Invoke(SurvivalTime);

        if (gameUI != null)
        {
            gameUI.ShowGameOver(SurvivalTime);
        }
    }
}
