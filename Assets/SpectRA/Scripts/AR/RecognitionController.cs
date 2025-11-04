using UnityEngine;

[DisallowMultipleComponent]
public class RecognitionController : MonoBehaviour
{
    [Header("Sources")]
    public ARCameraFrameProvider frameProvider; // Cámara
    public TextAsset tfliteModel;
    public TextAsset labels;

    [Header("UI")]
    public OverlayController overlay; // Panel UI

    [Header("Tuning")]
    [Range(0f, 1f)] public float confidenceThreshold = 0.60f;
    [Min(1)] public int requiredStableFrames = 3;

    private BuildingRecognizer recognizer;
    private int aboveCount = 0;

    private void Start()
    {
        recognizer = new BuildingRecognizer(tfliteModel, labels);

        if (frameProvider != null)
            frameProvider.OnFrameReady += OnFrame;
        else
            Debug.LogWarning("[SpectRA] No hay frameProvider.");

        if (overlay != null)
            overlay.Hide(immediate: true);
    }

    private void OnDestroy()
    {
        if (frameProvider != null)
            frameProvider.OnFrameReady -= OnFrame;
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
            if (aboveCount >= requiredStableFrames)
            {
                // Muestra panel
                overlay.Show(label, conf);

                // Texto según el bloque detectado
                string servicios = "";
                string horarios = "";

                if (label == "Bloque19")
                {
                    servicios = "• Laboratorios\n• Salas de cómputo\n• Talleres de ingeniería";
                    horarios = "Lunes a Viernes: 7am – 9pm";
                }
                else
                {
                    servicios = "No se detectó un bloque reconocido.\nApunta hacia el Bloque 19 para ver su información.";
                    horarios = "";
                }

                // Mostrar info
                overlay.ApplyTextDetails(servicios, horarios);

                aboveCount = 0;
            }
        }
        else aboveCount = 0;
    }

    public void OnUserClose()
    {
        overlay?.HideAndUnlock();
    }
}
