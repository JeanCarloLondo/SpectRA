using UnityEngine;

public class RecognitionController : MonoBehaviour
{
    public ARCameraFrameProvider frameProvider;
    public TextAsset tfliteModel;
    public TextAsset labels;
    public OverlayController overlay;

    BuildingRecognizer recognizer;

    void Start()
    {
        recognizer = new BuildingRecognizer(tfliteModel, labels, true);
        if (frameProvider != null) frameProvider.OnFrameReady += OnFrame;
    }

    void OnDestroy()
    {
        if (frameProvider != null) frameProvider.OnFrameReady -= OnFrame;
        recognizer?.Dispose();
    }

    void OnFrame(Texture2D tex)
    {
        var (label, conf) = recognizer.Predict(tex);
        if (overlay != null)
            overlay.ShowLabel(label, conf);
    }
}
