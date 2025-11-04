using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Text;
using System.Collections.Generic;

[DisallowMultipleComponent]
[RequireComponent(typeof(CanvasGroup))]
public class OverlayController : MonoBehaviour
{
    [Header("Required")]
    [SerializeField] private CanvasGroup canvasGroup;    // CanvasGroup del Panel

    [Header("Header (TMP)")]
    [SerializeField] private TMP_Text buildingNameText;  // Título
    [SerializeField] private TMP_Text confidenceText;    // "63%"

    [Header("Details (TMP)")]
    [SerializeField] private TMP_Text servicesText;      // Multilínea con bullets
    [SerializeField] private TMP_Text hoursText;         // Multilínea con horarios

    [Header("Gallery (optional)")]
    [SerializeField] private Transform galleryParent;    // Contenedor de thumbnails (Layout)
    [SerializeField] private Image galleryItemTemplate;  // Plantilla desactivada (Image)

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

    // ------------ API de panel básico ------------

    /// <summary>Muestra título + % y bloquea hasta cerrar.</summary>
    public void Show(string label, float confidence01)
    {
        // Actualiza cabecera
        if (buildingNameText)
            buildingNameText.text = string.IsNullOrWhiteSpace(label) ? unknownText : label;

        if (confidenceText)
        {
            int pct = Mathf.Clamp(Mathf.RoundToInt(confidence01 * 100f), 0, 100);
            confidenceText.text = $"{pct}%";
        }

        SetRaycast(true);
        FadeTo(1f, fadeDuration);
        IsLocked = true; // 🔒 hasta cerrar
    }

    /// <summary> Oculta el panel (no desbloquea). </summary>
    public void Hide(bool immediate = false)
    {
        if (immediate) SetAlpha(0f);
        else FadeTo(0f, fadeDuration);

        SetRaycast(false);
        // No cambiamos IsLocked aquí (lo hace HideAndUnlock)
        ClearDetails();
    }

    /// <summary> Botón Cerrar → oculta y desbloquea. </summary>
    public void HideAndUnlock()
    {
        Hide(immediate: true);
        IsLocked = false; // 🔓 permite nuevas detecciones
    }

    // ------------ Datos offline (detalles) ------------

    public void ApplyDetails(BuildingInfo info)
    {
        if (info == null) return;

        // Nombre "bonito" por si el label del modelo era raw
        if (buildingNameText && !string.IsNullOrWhiteSpace(info.name))
            buildingNameText.text = info.name;

        if (servicesText)
        {
            if (info.services != null && info.services.Count > 0)
            {
                var sb = new StringBuilder();
                foreach (var s in info.services) sb.AppendLine("• " + s);
                servicesText.text = sb.ToString();
            }
            else servicesText.text = "-";
        }

        if (hoursText)
        {
            if (info.hours != null && info.hours.Count > 0)
            {
                var sb = new StringBuilder();
                foreach (var h in info.hours)
                {
                    var lbl = string.IsNullOrEmpty(h.label) ? "" : (h.label + ": ");
                    sb.AppendLine($"{lbl}{h.open}–{h.close}");
                }
                hoursText.text = sb.ToString();
            }
            else hoursText.text = "-";
        }

        if (galleryParent && galleryItemTemplate)
        {
            ClearGallery();
            List<Sprite> sprites = LocalBuildingStore.LoadGallerySprites(info);
            foreach (var sp in sprites)
            {
                var img = Instantiate(galleryItemTemplate, galleryParent);
                img.gameObject.SetActive(true);
                img.sprite = sp;
                // opcional: preserva proporción
                var fitter = img.GetComponent<AspectRatioFitter>();
                if (fitter) fitter.aspectRatio = sp.rect.width / sp.rect.height;
            }
        }
    }

    // ------------ helpers ------------

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

    private void ClearDetails()
    {
        if (servicesText) servicesText.text = string.Empty;
        if (hoursText) hoursText.text = string.Empty;
        ClearGallery();
    }

    private void ClearGallery()
    {
        if (!galleryParent) return;
        for (int i = galleryParent.childCount - 1; i >= 0; i--)
        {
            var child = galleryParent.GetChild(i).gameObject;
            if (galleryItemTemplate && child == galleryItemTemplate.gameObject) continue; // no borres la plantilla
            Destroy(child);
        }
    }

    public void ApplyTextDetails(string servicios, string horarios)
    {
        if (servicesText)
            servicesText.text = servicios;

        if (hoursText)
            hoursText.text = horarios;
    }

}