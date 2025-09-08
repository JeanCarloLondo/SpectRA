// Assets/SpectRA/Scripts/AR/RecognitionController.cs
using UnityEngine;

[DisallowMultipleComponent]
public class RecognitionController : MonoBehaviour
{
    [Header("Sources")]
    public ARCameraFrameProvider frameProvider;   // Arrastra Main Camera (con ARCameraFrameProvider)
    public TextAsset tfliteModel;                 // Arrastra modelo_bloque19.tflite.bytes
    public TextAsset labels;                      // Arrastra labels.txt

    [Header("UI")]
    public OverlayController overlay;             // Arrastra Panel (con OverlayController)

    [Header("Tuning")]
    [Tooltip("Confianza mínima para considerar 'match'")]
    [Range(0f, 1f)] public float confidenceThreshold = 0.01f;

    [Tooltip("Frames consecutivos sobre el umbral requeridos para disparar el overlay")]
    [Min(1)] public int requiredStableFrames = 3;

    private BuildingRecognizer recognizer;
    private int aboveCount = 0;

    private void Start()
    {
        if (tfliteModel == null)
        {
            Debug.LogError("[SpectRA] tfliteModel no asignado en RecognitionController.");
            enabled = false;
            return;
        }

        recognizer = new BuildingRecognizer(tfliteModel, labels);

        if (frameProvider != null)
        {
            frameProvider.OnFrameReady += OnFrame;
        }
        else
        {
            Debug.LogWarning("[SpectRA] frameProvider no asignado (no habrá predicciones).");
        }

        if (overlay != null) overlay.Hide(immediate: true);
    }

    private void OnDestroy()
    {
        if (frameProvider != null) frameProvider.OnFrameReady -= OnFrame;
        recognizer?.Dispose();
    }

    private void OnFrame(Texture2D tex)
    {
        // Si el panel está bloqueado/visible, no interferimos hasta que el usuario cierre
        if (overlay != null && overlay.IsLocked) return;
        if (tex == null || recognizer == null) return;

        var (label, conf) = recognizer.Predict(tex);
        // Debug opcional: Debug.Log($"[SpectRA] pred: {label}  conf: {conf:0.000}");

        if (conf >= confidenceThreshold)
        {
            aboveCount++;
            if (aboveCount >= requiredStableFrames && overlay != null)
            {
                overlay.Show(label, conf); // 🔒 queda fijo hasta que el usuario pulse "Cerrar"
                aboveCount = 0;            // reset para el siguiente ciclo
            }
        }
        else
        {
            // Pierde estabilidad: resetea el contador (pero NO ocultamos, eso lo hace el botón)
            aboveCount = 0;
        }
    }
}