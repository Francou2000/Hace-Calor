using System;
using UnityEngine;

/// <summary>
/// Gestiona la masa del glaciar y su estado visual (5 sprites).
/// </summary>
public class GlacierController : MonoBehaviour
{
    [Header("Masa")]
    [SerializeField, Range(0f, 100f)] private float initialMass = 100f;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite state1Healthy;
    [SerializeField] private Sprite state2SlightlyMelted;
    [SerializeField] private Sprite state3Melted;
    [SerializeField] private Sprite state4AlmostGone;
    [SerializeField] private Sprite state5FullyMelted;

    public float CurrentMass { get; private set; }

    public event Action<float> OnMassChanged;
    public event Action OnFullyMelted;

    private bool notifiedFullyMelted;

    private void Awake()
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("[GlacierController] Falta asignar SpriteRenderer en el Inspector.");
        }
    }

    public void ResetGlacier()
    {
        CurrentMass = Mathf.Clamp(initialMass, 0f, 100f);
        notifiedFullyMelted = false;
        UpdateSprite();
        OnMassChanged?.Invoke(CurrentMass);
    }

    public void ApplyMassLoss(float amount)
    {
        if (CurrentMass <= 0f)
        {
            return;
        }

        CurrentMass = Mathf.Max(0f, CurrentMass - Mathf.Max(0f, amount));
        UpdateSprite();
        OnMassChanged?.Invoke(CurrentMass);

        if (!notifiedFullyMelted && CurrentMass <= 0f)
        {
            notifiedFullyMelted = true;
            OnFullyMelted?.Invoke();
        }
    }

    private void UpdateSprite()
    {
        if (spriteRenderer == null)
        {
            return;
        }

        // Rangos pedidos:
        // 100-76: estado 1 | 75-51: estado 2 | 50-26: estado 3 | 25-1: estado 4 | 0: estado 5
        if (CurrentMass >= 76f)
        {
            spriteRenderer.sprite = state1Healthy;
        }
        else if (CurrentMass >= 51f)
        {
            spriteRenderer.sprite = state2SlightlyMelted;
        }
        else if (CurrentMass >= 26f)
        {
            spriteRenderer.sprite = state3Melted;
        }
        else if (CurrentMass >= 1f)
        {
            spriteRenderer.sprite = state4AlmostGone;
        }
        else
        {
            spriteRenderer.sprite = state5FullyMelted;
        }
    }
}
