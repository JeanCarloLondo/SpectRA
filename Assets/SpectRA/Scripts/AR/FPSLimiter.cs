using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    [Range(30, 120)] public int targetFps = 30;
    void Start() { Application.targetFrameRate = targetFps; }
}
