using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//function to generate Soils boxes and get their list
//to make it run again delete all soils and set the list size to 0, deactivate and activate the script, then tadaaa

//it will be executed even outside playmode when awake

[ExecuteInEditMode]
public class GenerateSoil : MonoBehaviour {

    //size of the grid
    public int numberSoilx = 5;
    public int numberSoily = 5;

    //instantiation
    public GameObject SoilPrefab;
    Vector3 tempPosition;
    GameObject myInstantiatedSoil;

    //list to store soils
    public List<Soils> mySoilsList;

    //to generate soils and store them in the dbb
    private GameObject ConnectionInfosObject;
    private PlayersData playersData;

    public void myGenerate() {
        Debug.Log("begin soil generation");

        //gather the number of soil from the user
        ConnectionInfosObject = GameObject.Find("ConnectionInfos");
        if(ConnectionInfosObject)
        {
            playersData = ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData;
        }
        if (playersData!=null)
        {
            numberSoilx = playersData.playerNumbSoilx;
            numberSoily = playersData.playerNumbSoily;

            mySoilsList = new List<Soils>();

            for (int i = 0; i < numberSoilx; i++)
            {
                for (int j = 0; j < numberSoily; j++)
                {
                    //instantiate the soil box at the right position
                    tempPosition = new Vector3(i, 0, j);
                    myInstantiatedSoil = Instantiate(SoilPrefab, tempPosition, Quaternion.identity);
                    //give it a name
                    myInstantiatedSoil.name = "soil_box_" + i + "_" + j;

                    //add the new soil to the soil list
                    Soils mySoils = myInstantiatedSoil.GetComponent<Soils>();
                    if (mySoils)
                    {
                        mySoils.SoilsConstructor(i, j, myInstantiatedSoil);
                        mySoils.mySoilsData.soil_name = myInstantiatedSoil.name;
                        mySoils.mySoilsData.player_id = playersData.playerId;
                        mySoilsList.Add(mySoils);
                    }
                }
            }
            ConnectionInfosObject.GetComponent<Connecting>().StartOnlineGenerateSoils();
        }          
    }

    private void Awake()
    {
        if(SoilPrefab&& mySoilsList.Count==0)
        {
            myGenerate();
        }           
    }
}
