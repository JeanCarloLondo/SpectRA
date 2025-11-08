using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;
using SpectRA.Auth; // ← (por si luego quieres leer/mostrar el rol actual)

public class PopupController : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text tituloText;
    [SerializeField] private TMP_Text contenidoText;
    [SerializeField] private Image imagen;              // 👈 Nuevo
    [SerializeField] private VideoPlayer videoPlayer;   // 👈 Nuevo

    // 👇 Arrastra aquí en el Inspector el GameObject que tiene ProfilePromptLauncher
    [SerializeField] private ProfilePromptLauncher profileLauncher;

    private bool isVisible = false;

    private void Awake()
    {
        if (!canvasGroup) canvasGroup = GetComponent<CanvasGroup>();
        Hide(true);
    }

    // ----- Método principal -----
    public void Show(string titulo, string contenido)
    {
        if (tituloText)   tituloText.text = titulo;
        if (contenidoText) contenidoText.text = contenido;

        if (imagen)      imagen.gameObject.SetActive(false);
        if (videoPlayer) videoPlayer.gameObject.SetActive(false);

        gameObject.SetActive(true);
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        isVisible = true;
    }

    public void Hide(bool instant = false)
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        gameObject.SetActive(false);
        isVisible = false;
    }

    // ----- Métodos personalizados por botón -----
    public void ShowHorarios()
    {
        Show("Horarios",
            " <b>Piso 1:</b> 6am - 6pm\n" +
            " <b>Piso 2:</b> 6am - 6pm\n" +
            " <b>Piso 3:</b> 6am - 6pm\n" +
            " <b>Piso 4:</b> 6am - 9pm\n" +
            " <b>Piso 5:</b> 6am - 9pm\n" +
            " <b>Piso 6:</b> 6am - 9pm\n" +
            " <b>Piso 7:</b> 6am - 9pm");
    }

    public void ShowDistribucion()
    {
        Show("Distribución",
            " <b>Piso 1:</b> Laboratorio Chevrolett \n" +
            " <b>Piso 2:</b> Laboratorio de Electronica\n" +
            " <b>Piso 3:</b> laboratorios y talleres\n" +
            " <b>Piso 4:</b> Sala de computo, Aula de clase y Oficinas de profesores\n" +
            " <b>Piso 5:</b> Sala de estudio Doctorado y Maestria y Oficinas de profesores\n" +
            " <b>Piso 6:</b> Maestria Mercadeo\n" +
            " <b>Piso 7:</b> Maestrias");
    }

    public void ShowHistoria()
    {
        Show("<b>Historia del Bloque 19</b>",
            "El Bloque 19 fue inaugurado en 2010 como sede de las ingenierías.\n" +
            "Aun que ha recibido renovaciones varias veces.\n" +
            "Aquí se encuentran los programas de  Maestria, Doctorado y los laboratorios principales.");
    }

    public void ShowImagen(Sprite sprite, string titulo = "Vista del Bloque")
    {
        if (tituloText) tituloText.text = titulo;
        if (contenidoText) contenidoText.text = "";

        if (imagen)
        {
            imagen.sprite = sprite;
            imagen.preserveAspect = true;
            imagen.gameObject.SetActive(true);
        }

        if (videoPlayer) videoPlayer.gameObject.SetActive(false);

        gameObject.SetActive(true);
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        isVisible = true;
    }

    public void ShowImagenEjemplo()
    {
        Sprite spriteEjemplo = Resources.Load<Sprite>("adentro19"); // imagen que está en Resources
        ShowImagen(spriteEjemplo, "Vista del Bloque 19");
    }

    // ====== NUEVO: abrir el panel de selección de perfil desde el popup/menú ======
    public void OpenProfile()
    {
        if (profileLauncher != null)
            profileLauncher.OpenProfilePanel();
        else
            Debug.LogWarning("[PopupController] profileLauncher no asignado en el Inspector.");
    }
}