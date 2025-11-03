using System;
using UnityEngine;

namespace SpectRA.Auth
{
    [Serializable]
    public class UserProfile
    {
        public string displayName;
        public UserRole role = UserRole.Visitante; // ← Visitante por defecto
    }

    public static class UserProfileStorage
    {
        const string KEY = "spectra.userprofile.v1";

        public static void Save(UserProfile p)
        {
            PlayerPrefs.SetString(KEY, JsonUtility.ToJson(p));
            PlayerPrefs.Save();
        }

        public static UserProfile LoadOrDefault()
        {
            if (!PlayerPrefs.HasKey(KEY))
                return new UserProfile { displayName = "", role = UserRole.Visitante };
            return JsonUtility.FromJson<UserProfile>(PlayerPrefs.GetString(KEY));
        }

        public static bool Exists() => PlayerPrefs.HasKey(KEY);
        public static void Clear() { PlayerPrefs.DeleteKey(KEY); PlayerPrefs.Save(); }
    }
}