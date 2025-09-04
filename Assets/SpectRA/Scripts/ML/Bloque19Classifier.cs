using UnityEngine;
using TensorFlowLite;
using System.IO;

public class Bloque19Classifier : MonoBehaviour
{
    [Header("Modelo IA")]
    [SerializeField] private string modelName = "modelo_bloque19.tflite";

    [Header("UI Panel")]
    [SerializeField] private ShowInfoPanel infoPanel;

    private Interpreter interpreter;
    private float[,,,] input = new float[1, 224, 224, 3];
    private float[,,] output = new float[1, 1, 1];

    void Start()
    {
        // Ruta del modelo en StreamingAssets
        string modelPath = Path.Combine(Application.streamingAssetsPath, modelName);

        // Cargar modelo
        var options = new InterpreterOptions() { threads = 2 };
        interpreter = new Interpreter(File.ReadAllBytes(modelPath), options);
        interpreter.AllocateTensors();
    }

    public void PredictAndShow(Texture2D image)
    {
        float prediction = Predict(image);

        if (prediction < 0.5f) // Ajusta el umbral
        {
            infoPanel.ShowPanel(
                "Bloque 19",
                "Este es el Bloque 19. Aquí estará la descripción del edificio."
            );
        }
    }

    private float Predict(Texture2D image)
    {
        // Redimensionar imagen
        Texture2D resized = new Texture2D(224, 224);
        Graphics.ConvertTexture(image, resized);

        // Convertir a input
        Color32[] pixels = resized.GetPixels32();
        for (int y = 0; y < 224; y++)
        {
            for (int x = 0; x < 224; x++)
            {
                Color32 pixel = pixels[y * 224 + x];
                input[0, y, x, 0] = pixel.r / 255f;
                input[0, y, x, 1] = pixel.g / 255f;
                input[0, y, x, 2] = pixel.b / 255f;
            }
        }

        // Ejecutar inferencia
        interpreter.SetInputTensorData(0, input);
        interpreter.Invoke();
        interpreter.GetOutputTensorData(0, output);

        return output[0, 0, 0]; 
    }

    void OnDestroy()
    {
        interpreter?.Dispose();
    }
}
