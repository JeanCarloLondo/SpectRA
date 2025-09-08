// Assets/SpectRA/Scripts/AR/BuildingRecognizer.cs
using System;
using System.IO;
using UnityEngine;
using TensorFlowLite; // del plugin asus4

public class BuildingRecognizer : IDisposable
{
    Interpreter interpreter;
    float[,,,] input;   // [1,H,W,3]
    float[,] output;    // [1,1] -> prob de clase "1" (NoBloque19)
    int width = 224, height = 224;

    // labels opcionalmente para mostrar, no para lógica binaria
    readonly string[] labels;

    public BuildingRecognizer(TextAsset tfliteModel, TextAsset labelsTxt, bool useNNAPI = true)
    {
        if (tfliteModel == null) throw new Exception("TFLite model is null");
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
            labels = new[] { "Bloque19", "NoBloque19" }; // por si acaso
    }

    // Preprocess igual que MobileNetV2: [-1,1]
    void FillInputFromTexture(Texture2D tex)
    {
        var pixels = tex.GetPixels32();
        // asumimos tex.width==width y tex.height==height (ya lo reescalas en ARCameraFrameProvider)
        int i = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++, i++)
            {
                var c = pixels[i];
                // float32 en [-1,1]
                input[0, y, x, 0] = (c.r / 127.5f) - 1f;
                input[0, y, x, 1] = (c.g / 127.5f) - 1f;
                input[0, y, x, 2] = (c.b / 127.5f) - 1f;
            }
        }
    }

    // Devuelve (label, confidence)
    public (string label, float confidence) Predict(Texture2D tex)
    {
        if (tex.width != width || tex.height != height)
        {
            // seguridad: reescalar por si acaso
            var rt = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32);
            Graphics.Blit(tex, rt);
            var prev = RenderTexture.active;
            RenderTexture.active = rt;

            var tmp = new Texture2D(width, height, TextureFormat.RGBA32, false);
            tmp.ReadPixels(new Rect(0,0,width,height), 0, 0, false);
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

        float pNoBloque = Mathf.Clamp01(output[0, 0]); // prob de clase "1"
        float pBloque   = 1f - pNoBloque;              // prob de clase "0"

        // Mapea a etiquetas (asumiendo labels[0] = Bloque19, labels[1] = NoBloque19)
        if (pBloque >= pNoBloque)
            return ((labels!=null && labels.Length>0)? labels[0] : "Bloque19", pBloque);
        else
            return ((labels!=null && labels.Length>1)? labels[1] : "NoBloque19", pNoBloque);
    }

    public void Dispose()
    {
        interpreter?.Dispose();
        interpreter = null;
    }
}