// Assets/SpectRA/Scripts/AR/ARCameraFrameProvider.cs
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections;
using System;

[RequireComponent(typeof(ARCameraManager))]
public class ARCameraFrameProvider : MonoBehaviour
{
    public event Action<Texture2D> OnFrameReady;

    [Header("Output size for the ML model")]
    [SerializeField] int targetWidth = 224;   // pon el tamaño que espera tu .tflite
    [SerializeField] int targetHeight = 224;
    [SerializeField] int sendEveryNFrames = 2; // 1 = cada frame

    ARCameraManager cam;
    Texture2D targetTex;     // textura reescalada que enviamos al modelo
    Texture2D cpuTexFullRes; // textura tamaño nativo del frame
    int frameCount;

    void Awake()
    {
        cam = GetComponent<ARCameraManager>();
        targetTex = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);
    }

    void OnEnable()
    {
        if (cam != null) cam.frameReceived += OnFrame;
    }

    void OnDisable()
    {
        if (cam != null) cam.frameReceived -= OnFrame;
    }

    void OnDestroy()
    {
        if (cpuTexFullRes != null) Destroy(cpuTexFullRes);
        if (targetTex != null) Destroy(targetTex);
        cpuTexFullRes = null;
        targetTex = null;
    }

    void OnFrame(ARCameraFrameEventArgs _)
    {
        // controlar frecuencia de envío
        frameCount++;
        if (frameCount % Mathf.Max(1, sendEveryNFrames) != 0) return;

        if (!cam.TryAcquireLatestCpuImage(out XRCpuImage cpuImage))
            return;

        using (cpuImage)
        {
            // 1) Definir conversión → RGBA32
            var conv = new XRCpuImage.ConversionParams(cpuImage, TextureFormat.RGBA32)
            {
                inputRect = new RectInt(0, 0, cpuImage.width, cpuImage.height),
                outputDimensions = new Vector2Int(cpuImage.width, cpuImage.height),
                transformation = XRCpuImage.Transformation.None,
            };

            // 2) Reservar buffer y convertir
            int size = cpuImage.GetConvertedDataSize(conv);
            var buffer = new NativeArray<byte>(size, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            try
            {
                cpuImage.Convert(conv, buffer);

                // 3) Actualizar textura full-res (si cambia tamaño, la recreamos)
                if (cpuTexFullRes == null || cpuTexFullRes.width != cpuImage.width || cpuTexFullRes.height != cpuImage.height)
                {
                    if (cpuTexFullRes != null) Destroy(cpuTexFullRes);
                    cpuTexFullRes = new Texture2D(cpuImage.width, cpuImage.height, TextureFormat.RGBA32, false);
                }

                cpuTexFullRes.LoadRawTextureData(buffer);
                cpuTexFullRes.Apply(false, false);

                // 4) Reescalar a targetWidth x targetHeight con Blit
                var rt = RenderTexture.GetTemporary(targetWidth, targetHeight, 0, RenderTextureFormat.ARGB32);
                try
                {
                    Graphics.Blit(cpuTexFullRes, rt);
                    var prev = RenderTexture.active;
                    RenderTexture.active = rt;
                    targetTex.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0, false);
                    targetTex.Apply(false, false);
                    RenderTexture.active = prev;
                }
                finally
                {
                    RenderTexture.ReleaseTemporary(rt);
                }

                // 5) Entregar el frame redimensionado
                OnFrameReady?.Invoke(targetTex);
            }
            finally
            {
                buffer.Dispose();
            }
        }
    }
}