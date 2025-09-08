using UnityEngine;
using TMPro;            // <- IMPORTANTÍSIMO: usa TMP_Text
using System.Collections;

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

    private Coroutine fadeRoutine;

    void Reset()
    {
        // Intento de autowire rápido cuando añades el script
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        if (buildingNameText == null) buildingNameText = GetComponentInChildren<TMP_Text>();
    }

    void Awake()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        Hide(immediate: true); // arrancar oculto
    }

    /// <summary> Muestra el panel con label + % confianza (0..1). </summary>
    public void Show(string label, float confidence01)
    {
        if (buildingNameText != null)
            buildingNameText.text = string.IsNullOrWhiteSpace(label) ? unknownText : label;

        if (confidenceText != null)
        {
            int pct = Mathf.Clamp(Mathf.RoundToInt(confidence01 * 100f), 0, 100);
            confidenceText.text = $"{pct}%";
        }

        SetRaycast(true);
        FadeTo(1f, fadeDuration);
    }

    /// <summary> Oculta el panel. </summary>
    public void Hide(bool immediate = false)
    {
        SetRaycast(false);
        if (immediate) SetAlpha(0f);
        else FadeTo(0f, fadeDuration);
    }

    // ----------------- helpers -----------------

    private void SetRaycast(bool enabled)
    {
        if (canvasGroup == null) return;
        canvasGroup.interactable = enabled;
        canvasGroup.blocksRaycasts = enabled;
    }

    private void SetAlpha(float a)
    {
        if (canvasGroup == null) return;
        canvasGroup.alpha = a;
    }

    private void FadeTo(float target, float duration)
    {
        if (canvasGroup == null)
            return;

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