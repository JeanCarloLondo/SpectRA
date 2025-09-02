using UnityEngine;

public class TelemetryLogger : MonoBehaviour
{
    public void LogRecognition(string label, float conf, float latencyMs, bool success)
    {
        Debug.Log($"[Telemetry] label={label} conf={conf:F2} latency_ms={latencyMs:F0} success={success}");
    }
}
