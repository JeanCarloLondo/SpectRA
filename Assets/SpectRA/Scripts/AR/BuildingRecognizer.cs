// Assets/SpectRA/Scripts/AR/BuildingRecognizer.cs
using System;
using System.IO;
using UnityEngine;
using TensorFlowLite; // del plugin asus4

public class BuildingRecognizer : IDisposable
{
    private Interpreter interpreter;
    private float[,,,] input;   // [1,H,W,3]
    private float[,] output;    // [1,1] -> prob de clase "1" (NoBloque19)
    private int width = 224, height = 224;

    private readonly string[] labels;

    // 🔹 Referencia al OverlayController para mostrar la info bonita
    private OverlayController overlayController;

    public BuildingRecognizer(TextAsset tfliteModel, TextAsset labelsTxt, OverlayController overlay, bool useNNAPI = true)
    {
        if (tfliteModel == null) throw new Exception("TFLite model is null");

        overlayController = overlay;

        var options = new InterpreterOptions();
#if UNITY_ANDROID && !UNITY_EDITOR
        options.threads = 2;
        if (useNNAPI) options.useNNAPI = true;
#endif
        interpreter = new Interpreter(tfliteModel.bytes, options);
        interpreter.AllocateTensors();

        // Ajusta tamaños a los del modelo
        var inInfo = interpreter.GetInputTensorInfo(0);
        height = inInfo.shape[1];
        width  = inInfo.shape[2];

        input  = new float[1, height, width, 3];
        output = new float[1, 1];

        // labels
        if (labelsTxt != null)
            labels = labelsTxt.text.Replace("\r","").Split('\n');
        else
            labels = new[] { "Bloque19", "NoBloque19" }; // fallback
    }

    // === Procesamiento ===
    private void FillInputFromTexture(Texture2D tex)
    {
        var pixels = tex.GetPixels32();
        int i = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++, i++)
            {
                var c = pixels[i];
                input[0, y, x, 0] = (c.r / 127.5f) - 1f;
                input[0, y, x, 1] = (c.g / 127.5f) - 1f;
                input[0, y, x, 2] = (c.b / 127.5f) - 1f;
            }
        }
    }

    // === Predicción principal ===
    public (string label, float confidence) Predict(Texture2D tex)
    {
        if (tex.width != width || tex.height != height)
        {
            var rt = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32);
            Graphics.Blit(tex, rt);
            var prev = RenderTexture.active;
            RenderTexture.active = rt;

            var tmp = new Texture2D(width, height, TextureFormat.RGBA32, false);
            tmp.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);
            tmp.Apply(false, false);
            RenderTexture.active = prev;
            RenderTexture.ReleaseTemporary(rt);

            FillInputFromTexture(tmp);
            UnityEngine.Object.Destroy(tmp);
        }
        else
        {
            FillInputFromTexture(tex);
        }

        interpreter.SetInputTensorData(0, input);
        interpreter.Invoke();
        interpreter.GetOutputTensorData(0, output);

        float pNoBloque = Mathf.Clamp01(output[0, 0]);
        float pBloque = 1f - pNoBloque;

        string label = (pBloque >= pNoBloque)
            ? (labels != null && labels.Length > 0 ? labels[0] : "Bloque19")
            : (labels != null && labels.Length > 1 ? labels[1] : "NoBloque19");

        float confidence = (pBloque >= pNoBloque) ? pBloque : pNoBloque;

        // 🔹 Mostrar en pantalla (solo si tenemos overlay asignado)
        if (overlayController != null)
            ShowOverlay(label, confidence);

        return (label, confidence);
    }

    // === Muestra la interfaz bonita ===
    private void ShowOverlay(string label, float confidence)
    {
        overlayController.Show(label, confidence);

        if (label == "Bloque19")
        {
            overlayController.ApplyTextDetails(
                "Edificio de ciencias aplicadas e ingeniería",
                "Contiene laboratorios de ingeniería civil, mecánica y eléctrica.\n" +
                "Pisos 1–3: Laboratorios\n" +
                "Pisos 4–7: Oficinas y salas de innovación.\n" +
                "Horario: 7:00 a.m. – 9:00 p.m."
            );
        }
        else
        {
            overlayController.ApplyTextDetails(
                "Edificio no reconocido",
                "No se encontró información detallada para este edificio."
            );
        }
    }

    public void Dispose()
    {
        interpreter?.Dispose();
        interpreter = null;
    }
}
