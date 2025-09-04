using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARCameraFeed : MonoBehaviour
{
    [SerializeField] private ARCameraManager cameraManager;
    [SerializeField] private Bloque19Classifier classifier;

    private Texture2D cameraTexture;

    void Update()
    {
        if (cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image)) // 👈 tipo explícito
        {
            // Crear textura si es necesario
            if (cameraTexture == null ||
                cameraTexture.width != image.width ||
                cameraTexture.height != image.height)
            {
                cameraTexture = new Texture2D(image.width, image.height, TextureFormat.RGBA32, false);
            }

            // Parámetros de conversión
            var conversionParams = new XRCpuImage.ConversionParams
            {
                inputRect = new RectInt(0, 0, image.width, image.height),
                outputDimensions = new Vector2Int(image.width, image.height),
                outputFormat = TextureFormat.RGBA32,
                transformation = XRCpuImage.Transformation.MirrorY
            };

            // Copiar datos crudos
            var rawTextureData = cameraTexture.GetRawTextureData<byte>();
            image.Convert(conversionParams, rawTextureData);
            cameraTexture.Apply();

            // Liberar memoria del frame
            image.Dispose();

            // Pasar al clasificador
            classifier.PredictAndShow(cameraTexture);
        }
    }
}
