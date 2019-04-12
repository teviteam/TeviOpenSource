using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoilsData
{
    public int id_soil;
    public int player_id;
    public string soil_name;
    public int posX;
    public int posY;
    public int water_lvl =35;
    public int nutrient_lvl =35;
    public bool is_planted = false;
    public bool has_babyplant = false;
    public string plant_name;
    public float plant_time;

    public static SoilsData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<SoilsData>(jsonString);
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }
}