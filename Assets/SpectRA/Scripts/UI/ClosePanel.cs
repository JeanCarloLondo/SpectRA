using UnityEngine;

public class ClosePanel : MonoBehaviour
{
    public GameObject panel; // referencia al Panel de Info

    public void Close()
    {
        panel.SetActive(false);
    }
}
