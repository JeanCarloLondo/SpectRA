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
                overlay.Show(label, conf);

                if (label == "Bloque19")
                {
                    // 🔹 Título y subtítulo principal
                    overlay.ApplyTextDetails(
                        "Edificio de ciencias aplicadas e ingeniería",
                        "Contiene laboratorios de ingeniería civil, mecánica y eléctrica.\n" +
                        "Pisos 1–3: Laboratorios\n" +
                        "Pisos 4–7: Oficinas y salas de innovación.\n" +
                        "Horario: 7:00 a.m. – 9:00 p.m."
                    );
                }
                else
                {
                    overlay.ApplyTextDetails(
                        "Edificio no reconocido",
                        "No se encontró información detallada para este edificio."
                    );
                }

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
