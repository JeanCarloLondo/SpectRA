// Assets/SpectRA/Scripts/UI/OverlayController.cs
using UnityEngine;
using TMPro;
using System.Collections;

[DisallowMultipleComponent]
public class OverlayController : MonoBehaviour
{
    [Header("Required")]
    [SerializeField] private CanvasGroup canvasGroup;   // CanvasGroup del Panel

    [Header("Texts (TMP)")]
    [SerializeField] private TMP_Text buildingNameText; // TitleText
    [SerializeField] private TMP_Text confidenceText;   // Description (o un segundo TMP para %)

    [Header("Config")]
    [SerializeField] private string unknownText = "Building not recognized";
    [SerializeField] private float fadeDuration = 0.15f;

    public bool IsLocked { get; private set; } = false;

    private Coroutine fadeRoutine;

    void Reset()
    {
        if (!canvasGroup) canvasGroup = GetComponent<CanvasGroup>();
        if (!buildingNameText) buildingNameText = GetComponentInChildren<TMP_Text>();
    }

    void Awake()
    {
        if (!canvasGroup) canvasGroup = GetComponent<CanvasGroup>();
        Hide(immediate: true); // arrancar oculto
        IsLocked = false;
    }

    /// <summary>
    /// Muestra el panel con label + % confianza (0..1) y LO BLOQUEA
    /// hasta que el usuario pulse "Cerrar".
    /// </summary>
    public void Show(string label, float confidence01)
    {
        // Actualiza contenido
        if (buildingNameText)
            buildingNameText.text = string.IsNullOrWhiteSpace(label) ? unknownText : label;

        if (confidenceText)
        {
            int pct = Mathf.Clamp(Mathf.RoundToInt(confidence01 * 100f), 0, 100);
            confidenceText.text = $"{pct}%";
        }

        // Habilita interacción y muestra con fade
        SetRaycast(true);
        FadeTo(1f, fadeDuration);

        // Queda fijo hasta cerrar
        IsLocked = true;
    }

    /// <summary> Oculta el panel (no desbloquea). </summary>
    public void Hide(bool immediate = false)
    {
        SetRaycast(false);
        if (immediate) SetAlpha(0f);
        else FadeTo(0f, fadeDuration);
    }

    /// <summary>
    /// Llamar desde el botón "Cerrar".
    /// Oculta inmediatamente y DESBLOQUEA para permitir nuevos reconocimientos.
    /// </summary>
    public void HideAndUnlock()
    {
        Hide(immediate: true);
        IsLocked = false;
    }

    // ----------------- helpers -----------------

    private void SetRaycast(bool enabled)
    {
        if (!canvasGroup) return;
        canvasGroup.interactable = enabled;
        canvasGroup.blocksRaycasts = enabled;
    }

    private void SetAlpha(float a)
    {
        if (!canvasGroup) return;
        canvasGroup.alpha = a;
    }

    private void FadeTo(float target, float duration)
    {
        if (!canvasGroup) return;

        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeRoutine(target, duration));
    }

    private IEnumerator FadeRoutine(float target, float duration)
    {
        float start = canvasGroup.alpha;
        if (Mathf.Approximately(start, target) || duration <= 0f)
        {
            SetAlpha(target);
            yield break;
        }

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / duration);
            SetAlpha(Mathf.Lerp(start, target, k));
            yield return null;
        }
        SetAlpha(target);
    }
}