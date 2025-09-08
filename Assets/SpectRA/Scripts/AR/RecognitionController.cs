using UnityEngine;

public class RecognitionController : MonoBehaviour
{
    [Header("Inputs")]
    public ARCameraFrameProvider frameProvider; // arrástralo desde tu escena
    public TextAsset tfliteModel;               // Assets/SpectRA/Models/TFLite/...
    public TextAsset labels;                    // Assets/SpectRA/Models/TFLite/labels.txt

    [Header("UI")]
    public OverlayController overlay;           // arrastra el Panel con OverlayController

    [Header("Threshold")]
    [Range(0f, 1f)]
    public float confidenceThreshold = 0.01f;   // editable en el Inspector

    private BuildingRecognizer recognizer;

    void Start()
    {
        // Crea el recognizer (usaUint8=true si tu modelo es INT8)
        recognizer = new BuildingRecognizer(tfliteModel, labels);

        if (frameProvider != null)
        {
            frameProvider.OnFrameReady += OnFrame;
        }

        // Asegura que el panel arranca oculto
        if (overlay != null) overlay.Hide(immediate: true);
    }

    void OnDestroy()
    {
        if (frameProvider != null) frameProvider.OnFrameReady -= OnFrame;
        recognizer?.Dispose();
    }

    private void OnFrame(Texture2D tex)
    {
        if (tex == null || recognizer == null) return;

        var (label, conf) = recognizer.Predict(tex);

        if (overlay == null) return;

        if (conf >= confidenceThreshold)
        {
            overlay.Show(label, conf);
        }
        else
        {
            overlay.Hide(); // por debajo del umbral, oculta
        }

    }
}