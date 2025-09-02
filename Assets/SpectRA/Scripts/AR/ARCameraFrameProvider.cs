using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System;

[RequireComponent(typeof(ARCameraManager))]
public class ARCameraFrameProvider : MonoBehaviour
{
    // TODO: hook into ARCameraManager.frameReceived and sample frames periodically.
    public event Action<Texture2D> OnFrameReady;

    [SerializeField] int targetWidth = 256;
    [SerializeField] int targetHeight = 256;

    ARCameraManager cam;
    Texture2D buffer;

    void Awake()
    {
        cam = GetComponent<ARCameraManager>();
        buffer = new Texture2D(targetWidth, targetHeight, TextureFormat.RGB24, false);
    }

    void OnEnable() { if (cam != null) cam.frameReceived += OnFrame; }
    void OnDisable() { if (cam != null) cam.frameReceived -= OnFrame; }

    void OnFrame(ARCameraFrameEventArgs args)
    {
        // Minimal stub - you will implement CPU image acquisition here.
        // When ready, invoke:
        // OnFrameReady?.Invoke(buffer);
    }
}
