using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryData
{
    public int id_inventory = 0;
    public int player_id = 0;
    public string itemName = "BLANK NAME";
    public int itemCount;

    public InventoryData(int playerid, string itemName, int itemCount)
    {
        this.itemName = itemName;
        this.itemCount = itemCount;
        this.player_id = playerid;
        this.id_inventory = 0;
    }

    public static InventoryData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<InventoryData>(jsonString);
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

}
