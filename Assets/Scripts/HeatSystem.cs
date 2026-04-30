using UnityEngine;

/// <summary>
/// Gestiona calor, escalado de dificultad e input (mouse + touch).
/// </summary>
public class HeatSystem : MonoBehaviour
{
    [Header("Balance inicial")]
    [SerializeField] private float initialHeat = 0f;
    [SerializeField] private float heatIncreaseRate = 2f;
    [SerializeField] private float difficultyScaling = 0.05f;
    [SerializeField] private float coolingPower = 8f;
    [SerializeField] private float massLossRate = 5f;
    [SerializeField] private float dangerHeatThreshold = 25f;

    public float CurrentHeat { get; private set; }

    public void ResetHeat()
    {
        CurrentHeat = Mathf.Max(0f, initialHeat);
    }

    public void Tick(float deltaTime, float survivalTime, GlacierController glacier)
    {
        if (glacier == null)
        {
            Debug.LogError("[HeatSystem] GlacierController es null en Tick().");
            return;
        }

        // Escalado progresivo: cada segundo hace crecer más rápido el calor.
        float scaledIncrease = heatIncreaseRate * (1f + (difficultyScaling * survivalTime));
        CurrentHeat += scaledIncrease * deltaTime;

        // Input: click izquierdo o tap.
        if (IsCoolingInputPressed())
        {
            CurrentHeat = Mathf.Max(0f, CurrentHeat - coolingPower);
        }

        // Derretimiento cuando supera umbral.
        if (CurrentHeat > dangerHeatThreshold)
        {
            float excessHeatFactor = (CurrentHeat - dangerHeatThreshold) / dangerHeatThreshold;
            float meltAmount = massLossRate * (1f + excessHeatFactor) * deltaTime;
            glacier.ApplyMassLoss(meltAmount);
        }
    }

    private bool IsCoolingInputPressed()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            return touch.phase == TouchPhase.Began;
        }

        return false;
    }
}
