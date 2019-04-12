using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlantnetResultData {

    public int id_plant;
    public int player_id;
    public string image_name;
    public string plant0;
    public string common0;
    public float score0;
    public string plant1;
    public string common1;
    public float score1;
    public string plant2;
    public string common2;
    public float score2;
    public string date;

    public static PlantnetResultData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<PlantnetResultData>(jsonString);
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }
}
