using UnityEngine;

namespace SpectRA.Auth
{
    public class ProfileManager : MonoBehaviour
    {
        public static ProfileManager Instance { get; private set; }
        public UserProfile Current { get; private set; }

        public delegate void RoleChanged(UserRole role);
        public event RoleChanged OnRoleChanged;

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Current = UserProfileStorage.LoadOrDefault(); // Visitante si no existe
        }

        public void SetRole(UserRole role)
        {
            if (Current.role == role) return;
            Current.role = role;
            UserProfileStorage.Save(Current);
            OnRoleChanged?.Invoke(role);
        }

        public void SetDisplayName(string name)
        {
            Current.displayName = name ?? "";
            UserProfileStorage.Save(Current);
        }
    }
}