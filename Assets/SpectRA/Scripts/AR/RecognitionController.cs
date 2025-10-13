// Assets/SpectRA/Scripts/AR/RecognitionController.cs
using UnityEngine;

[DisallowMultipleComponent]
public class RecognitionController : MonoBehaviour
{
    [Header("Sources")]
    public ARCameraFrameProvider frameProvider;   // Arrastra Main Camera (con ARCameraFrameProvider)
    public TextAsset tfliteModel;                 // (si tu recognizer lo usa)
    public TextAsset labels;                      // (si tu recognizer lo usa)

    [Header("UI")]
    public OverlayController overlay;             // Panel (OverlayController)

    [Header("Tuning")]
    [Tooltip("Confianza mínima para considerar 'match'")]
    [Range(0f, 1f)] public float confidenceThreshold = 0.60f;

    [Tooltip("Frames consecutivos sobre el umbral requeridos para disparar el overlay")]
    [Min(1)] public int requiredStableFrames = 3;

    private BuildingRecognizer recognizer;
    private int aboveCount = 0;

    private void Start()
    {
        // Si tu recognizer requiere modelo/labels, valida aquí.
        recognizer = new BuildingRecognizer(tfliteModel, labels);

        if (frameProvider != null) frameProvider.OnFrameReady += OnFrame;
        else Debug.LogWarning("[SpectRA] frameProvider no asignado (no habrá predicciones).");

        if (overlay != null) overlay.Hide(immediate: true);
    }

    private void OnDestroy()
    {
        if (frameProvider != null) frameProvider.OnFrameReady -= OnFrame;
        recognizer?.Dispose();
    }

    private void OnFrame(Texture2D tex)
    {
        if (overlay != null && overlay.IsLocked) return;
        if (tex == null || recognizer == null) return;

        var (label, conf) = recognizer.Predict(tex);

        if (conf >= confidenceThreshold)
        {
            aboveCount++;
            if (aboveCount >= requiredStableFrames && overlay != null)
            {
                // 1) Muestra panel y bloquea hasta cerrar
                overlay.Show(label, conf);

                // 2) Carga detalles offline por etiqueta y puebla UI
                var info = LocalBuildingStore.LoadInfoByLabel(label);
                if (info != null) overlay.ApplyDetails(info);

                // 3) Reinicia contador para el próximo ciclo
                aboveCount = 0;
            }
        }
        else
        {
            // perdió estabilidad
            aboveCount = 0;
        }
    }

    /// <summary>
    /// Conecta este método al botón Cerrar si quieres
    /// llamar desde aquí en vez de usar OverlayController.HideAndUnlock():
    /// </summary>
    public void OnUserClose()
    {
        if (overlay == null) return;
        overlay.HideAndUnlock();
    }
}