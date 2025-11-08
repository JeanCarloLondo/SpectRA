using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SpectRA.Auth;

public class RoleSelectionUI : MonoBehaviour
{
    [Header("Referencias")]
    public Button estudianteBtn;
    public Button profesorBtn;
    public Button administrativoBtn;
    public Button continueBtn;
    public Button skipBtn;
    public TMP_InputField nameInput;
    public Toggle rememberToggle;
    public GameObject root;
    public GameObject profilePanel; // ← NUEVO

    private UserRole? selected;

    void Start()
    {
        continueBtn.interactable = false;

        estudianteBtn.onClick.AddListener(() => Pick(UserRole.Estudiante));
        profesorBtn.onClick.AddListener(() => Pick(UserRole.Profesor));
        administrativoBtn.onClick.AddListener(() => Pick(UserRole.Administrativo));

        continueBtn.onClick.AddListener(Confirm);
        skipBtn.onClick.AddListener(Skip);
    }

    void Pick(UserRole r)
    {
        selected = r;
        continueBtn.interactable = true;
    }

    void Confirm()
    {
        if (selected == null) return;

        var pm = ProfileManager.Instance;
        if (!string.IsNullOrWhiteSpace(nameInput?.text))
            pm.SetDisplayName(nameInput.text.Trim());
        pm.SetRole(selected.Value);

        // Desactiva el ProfilePanel completo
        if (profilePanel != null)
            profilePanel.SetActive(false);
        else
            root.SetActive(false);
    }

    void Skip()
    {
        // No cambia nada, sigue como Visitante
        if (profilePanel != null)
            profilePanel.SetActive(false);
        else
            root.SetActive(false);
    }
}