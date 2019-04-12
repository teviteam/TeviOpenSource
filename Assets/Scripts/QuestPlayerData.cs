using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestPlayerData
{
    public int id_quest_player;
    public int player_id;
    public int id_quest; //the quest template to call
    public bool quest_completed;
    public string begin_date;
    public string completion_date;

    public static QuestPlayerData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<QuestPlayerData>(jsonString);
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }
}