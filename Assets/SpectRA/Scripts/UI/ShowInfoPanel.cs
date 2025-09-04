using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class ShowInfoPanel : MonoBehaviour
{
    [Header("AR")]
    public ARTrackedImageManager trackedImageManager;

    [Header("UI Panel")]
    [SerializeField] private GameObject infoPanel;          
    [SerializeField] private TMP_Text tituloText;           
    [SerializeField] private TMP_Text descripcionText;      

    private void OnEnable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnDisable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
        {
            CheckImage(trackedImage);
        }
        foreach (var trackedImage in args.updated)
        {
            CheckImage(trackedImage);
        }
    }

    void CheckImage(ARTrackedImage trackedImage)
    {
        // Verificamos que el nombre de la referencia sea "Edificio19"
        if (trackedImage.referenceImage.name == "Edificio19")
        {
            if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                ShowPanel("Edificio 19", "Aquí va la descripción del edificio 19.");
            }
            else
            {
                HidePanel();
            }
        }
    }

    // ==== NUEVOS MÉTODOS ====
    public void ShowPanel(string titulo, string descripcion)
    {
        if (tituloText != null) tituloText.text = titulo;
        if (descripcionText != null) descripcionText.text = descripcion;
        if (infoPanel != null) infoPanel.SetActive(true);
    }

    public void HidePanel()
    {
        if (infoPanel != null) infoPanel.SetActive(false);
    }
}
