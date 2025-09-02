using UnityEngine;
using UnityEngine.UI;

public class OverlayController : MonoBehaviour
{
    public Text buildingNameText;
    public Text confidenceText;
    public string unknownText = "Building not recognized";

    public void ShowLabel(string name, float conf)
    {
        if (buildingNameText) buildingNameText.text = string.IsNullOrEmpty(name) ? unknownText : name;
        if (confidenceText) confidenceText.text = conf > 0 ? $"Confidence: {(int)(conf*100)}%" : "";
    }

    public void ShowUnknown()
    {
        if (buildingNameText) buildingNameText.text = unknownText;
        if (confidenceText) confidenceText.text = "";
    }
}
