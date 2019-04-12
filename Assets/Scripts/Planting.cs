using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planting : MonoBehaviour {

    public void PlantingFunction(GameObject mySoil, GameObject seed, float newTime){

        if (mySoil&&seed)
        {
            //get the current seed from the inventory script
            GameObject myPrefab = seed;

            //we plant the current seed of plant selected from inventory into the soil centers that has been touched
            GameObject clonePlante;
            clonePlante = Instantiate(myPrefab, mySoil.transform.position, mySoil.transform.rotation);
            //Debug.Log(myPrefab.name+" has been planted on "+ mySoil.name);

            //growth script is added to the sucker: growth begins!
            Growth myGrowthComponent = clonePlante.AddComponent<Growth>();
            myGrowthComponent.SoilOfThePlant = mySoil;            

            //calls for the update of number of neighbouring planted soils cells
            Soils mySoilsComponent;
            mySoilsComponent = mySoil.GetComponent<Soils>();
            if(mySoilsComponent)
            {
                mySoilsComponent.UpdateAllSurroundingCount();
                //it is a new plant: initialize its timer
                mySoilsComponent.mySoilsData.plant_time = newTime;

                //give the soil a refernce to the plant that's on it.
                if (clonePlante.GetComponent<PlantData>() != null)
                {
                    mySoilsComponent.readOnlyPlantData = clonePlante.GetComponent<PlantData>();
                }


               
            }
            
        }       
    }

}
