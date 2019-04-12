using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceData : MonoBehaviour {

    //This class should contain the resoure production data for each plant.
    //Each plant with a 'Plant Data' script should also have one of these.


    Ratios ratios; //reference to the Ratios object
    
    //How much of each resource this plant produces when happy
    public float food = 0;
    public float fuel = 0;
    public float construction = 0;
    public float medicine = 0;
    public float culture = 0;


    void Start () {

        ratios = GameObject.Find("Ratios").GetComponent<Ratios>();
        if (ratios)
        {
            ratios.totalFood += food;
            ratios.totalFuel += fuel;
            ratios.totalConstruction += construction;
            ratios.totalMedicine += medicine;
            ratios.totalCulture += culture;
        }
    }

    void OnDestroy()
    {
        if (ratios)
        {
            ratios.totalFood -= food;
            ratios.totalFuel -= fuel;
            ratios.totalConstruction -= construction;
            ratios.totalMedicine -= medicine;
            ratios.totalCulture -= culture;

        }
    }

}
