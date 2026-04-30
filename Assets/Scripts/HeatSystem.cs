using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
#endif

/// <summary>
/// Gestiona calor, escalado de dificultad e input (mouse + touch).
/// Compatible con Input System y con Input Manager clásico.
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

#if ENABLE_INPUT_SYSTEM
    private bool enhancedTouchEnabled;
#endif

    private void OnEnable()
    {
#if ENABLE_INPUT_SYSTEM
        if (!enhancedTouchEnabled)
        {
            EnhancedTouchSupport.Enable();
            enhancedTouchEnabled = true;
        }
#endif
    }

    private void OnDisable()
    {
#if ENABLE_INPUT_SYSTEM
        if (enhancedTouchEnabled)
        {
            EnhancedTouchSupport.Disable();
            enhancedTouchEnabled = false;
        }
#endif
    }

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

        float scaledIncrease = heatIncreaseRate * (1f + (difficultyScaling * survivalTime));
        CurrentHeat += scaledIncrease * deltaTime;

        if (IsCoolingInputPressed())
        {
            CurrentHeat = Mathf.Max(0f, CurrentHeat - coolingPower);
        }

        if (CurrentHeat > dangerHeatThreshold)
        {
            float excessHeatFactor = (CurrentHeat - dangerHeatThreshold) / dangerHeatThreshold;
            float meltAmount = massLossRate * (1f + excessHeatFactor) * deltaTime;
            glacier.ApplyMassLoss(meltAmount);
        }
    }

    private bool IsCoolingInputPressed()
    {
#if ENABLE_INPUT_SYSTEM
        // Mouse (left click)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            return true;
        }

        // Touch (tap)
        var activeTouches = Touch.activeTouches;
        for (int i = 0; i < activeTouches.Count; i++)
        {
            if (activeTouches[i].phase == TouchPhase.Began)
            {
                return true;
            }
        }

        return false;
#else
        // Fallback al sistema clásico.
        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            return touch.phase == UnityEngine.TouchPhase.Began;
        }

        return false;
#endif
    }
}
