using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayersData
{
    public int playerId;
    public string playerUsername;
    public string playerEmail;
    public string playerPassword;
    public int playerLvl;
    public int playerCorp;
    public int playerNumbSoilx = 5;
    public int playerNumbSoily = 5;
    public int playerXP = 0;
    public string playerAccountCreation;
    public string playerLastConnexion;
    public string playerLastShipment;

    public static PlayersData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<PlayersData>(jsonString);
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }
}
