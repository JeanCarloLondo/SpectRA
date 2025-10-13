using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Carga datos 100% offline desde Resources:
///  - JSON:   Resources/Buildings/<key>.json
///  - Fotos:  Resources/Images/<spriteName>
/// </summary>
public static class LocalBuildingStore
{
    /// <summary>
    /// Normaliza un label del modelo ("Bloque 19 - Engineering") → "bloque19"
    /// </summary>
    public static string NormalizeKey(string label)
    {
        if (string.IsNullOrWhiteSpace(label)) return string.Empty;

        string s = label.Trim().ToLowerInvariant();
        // quita signos y espacios
        s = s.Replace(" ", "").Replace("_", "").Replace("-", "");
        // acentos y eñes
        s = s.Replace("á","a").Replace("é","e").Replace("í","i")
             .Replace("ó","o").Replace("ú","u").Replace("ñ","n");
        // si quieres, recorta sufijos
        // (ej. "bloque19engineering" -> "bloque19" al encontrar primer dígito)
        int firstDigit = -1;
        for (int i = 0; i < s.Length; i++)
        {
            if (char.IsDigit(s[i])) { firstDigit = i; break; }
        }
        if (firstDigit >= 0)
        {
            // mantenemos "bloque" + dígitos consecutivos (bloque19)
            int j = firstDigit;
            while (j < s.Length && char.IsDigit(s[j])) j++;
            string prefix = s.Substring(0, firstDigit);      // "bloque"
            string digits = s.Substring(firstDigit, j-firstDigit); // "19"
            return prefix + digits;
        }
        return s;
    }

    public static BuildingInfo LoadInfoByLabel(string label)
    {
        string key = NormalizeKey(label);
        if (string.IsNullOrEmpty(key)) return null;

        TextAsset ta = Resources.Load<TextAsset>($"Buildings/{key}");
        if (ta == null)
        {
            Debug.LogWarning($"[LocalBuildingStore] No se encontró JSON: Resources/Buildings/{key}.json");
            return null;
        }

        try
        {
            return JsonUtility.FromJson<BuildingInfo>(ta.text);
        }
        catch (System.SystemException e)
        {
            Debug.LogError($"[LocalBuildingStore] Error parseando JSON {key}: {e}");
            return null;
        }
    }

    public static List<Sprite> LoadGallerySprites(BuildingInfo info)
    {
        var list = new List<Sprite>();
        if (info?.gallery == null) return list;

        foreach (var name in info.gallery)
        {
            if (string.IsNullOrWhiteSpace(name)) continue;
            var sp = Resources.Load<Sprite>($"Images/{name}");
            if (sp != null) list.Add(sp);
            else Debug.LogWarning($"[LocalBuildingStore] Sprite no encontrado: Resources/Images/{name}");
        }
        return list;
    }
}