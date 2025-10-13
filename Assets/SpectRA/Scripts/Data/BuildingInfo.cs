using System;
using System.Collections.Generic;

[Serializable]
public class BuildingInfo
{
    public string id;                    // "bloque19"
    public string name;                  // "Bloque 19"
    public List<string> services;        // ["Salas de estudio","Laboratorios",...]
    public List<BuildingHour> hours;     // ver clase abajo
    public List<string> gallery;         // ["bloque19_1","bloque19_2"] (Sprites en Resources/Images)
}

[Serializable]
public class BuildingHour
{
    public string label; // "Lun–Vie", "Sáb", etc.
    public string open;  // "07:00"
    public string close; // "21:00"
}