using UnityEngine;
using System;

// Placeholder wrapper for TFLite inference.
// Replace with real TensorFlow Lite interpreter usage.
public class BuildingRecognizer : IDisposable
{
    public BuildingRecognizer(TextAsset modelAsset, TextAsset labelsAsset, bool useUint8Model = true)
    {
        // Load model bytes and labels here.
    }

    public (string label, float confidence) Predict(Texture2D tex)
    {
        // Return a fake prediction for now.
        return ("Unknown", 0.0f);
    }

    public void Dispose() { }
}
