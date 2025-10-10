using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;

public class PopupController : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text tituloText;
    [SerializeField] private TMP_Text contenidoText;
    [SerializeField] private Image imagen;              // 👈 Nuevo
    [SerializeField] private VideoPlayer videoPlayer;   // 👈 Nuevo

    private bool isVisible = false;

    private void Awake()
    {
        if (!canvasGroup) canvasGroup = GetComponent<CanvasGroup>();
        Hide(true);
    }

    // ----- Método principal -----
    public void Show(string titulo, string contenido)
    {
        tituloText.text = titulo;
        contenidoText.text = contenido;

        if (imagen) imagen.gameObject.SetActive(false);
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
            "🕓 Piso 1: 7am – 5pm\n" +
            "🕔 Piso 2: 8am – 6pm\n" +
            "🕕 Piso 3: 9am – 7pm");
    }

    public void ShowDistribucion()
    {
        Show("Distribución",
            "📚 Piso 1: Laboratorio Chevrolett\n" +
            "💻 Piso 2: Laboratorio de Electronica\n" +
            "💻 Piso 3: Laboratorio de Electronica\n" +
            "💻 Piso 4: Sala de computo, Aula de clase y Oficinas de profesores\n" +
            "💻 Piso 5: Sala de estudio Doctorado y Maestria y Oficinas de profesores\n" +
            "💻 Piso 6: Mercadeo\n" +
            "🏢 Piso 7: Maestria mercadeo");
    }

    public void ShowHistoria()
    {
        Show("Historia del Bloque 19",
            "El Bloque 19 fue inaugurado en 2010 como sede de las ingenierías.\n" +
            "Aquí se encuentran los programas de Doctorado y los laboratorios principales.");
    }


    public void ShowImagen(Sprite sprite, string titulo = "Vista del Bloque")
    {
        tituloText.text = titulo;
        contenidoText.text = "";

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


    public void ShowImagenEjemplo(){

        Sprite spriteEjemplo = Resources.Load<Sprite>("adentro19"); //imagen que esta en resources
        ShowImagen(spriteEjemplo, "Vista del Bloque 19");
    }

}
