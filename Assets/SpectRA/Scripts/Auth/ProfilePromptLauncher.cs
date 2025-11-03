using UnityEngine;

public class ProfilePromptLauncher : MonoBehaviour
{
    [Tooltip("Asigna el prefab RoleSelectionPanel (desactivado por defecto)")]
    public GameObject roleSelectionPanel;

    [Tooltip("Si es true, se muestra invitación al inicio (siempre se puede Saltar)")]
    public bool showNonBlockingInvite = false;

    void Start()
    {
        if (showNonBlockingInvite && roleSelectionPanel != null)
            roleSelectionPanel.SetActive(true);
    }

    // Llama esto desde tu botón “Perfil”
    public void OpenProfilePanel()
    {
        if (roleSelectionPanel != null)
            roleSelectionPanel.SetActive(true);
    }
}