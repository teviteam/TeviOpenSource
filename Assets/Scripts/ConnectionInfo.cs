using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionInfo : MonoBehaviour {

    public PlayersData myPlayersData;

    //making the object and its variable stay through loading
    void Awake()
    {
         DontDestroyOnLoad(this.gameObject);            
    }
}
