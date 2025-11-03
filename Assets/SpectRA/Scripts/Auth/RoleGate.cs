using System.Linq;
using UnityEngine;
using SpectRA.Auth;

[DefaultExecutionOrder(-10)]
public class RoleGate : MonoBehaviour
{
    [Tooltip("Vacío = visible para todos")]
    public UserRole[] allowedRoles;

    void OnEnable()
    {
        var pm = ProfileManager.Instance;
        if (pm != null) Apply(pm.Current.role);
        if (pm != null) pm.OnRoleChanged += Apply;
    }

    void OnDisable()
    {
        var pm = ProfileManager.Instance;
        if (pm != null) pm.OnRoleChanged -= Apply;
    }

    void Apply(UserRole role)
    {
        if (allowedRoles == null || allowedRoles.Length == 0) { gameObject.SetActive(true); return; }
        gameObject.SetActive(allowedRoles.Contains(role));
    }
}