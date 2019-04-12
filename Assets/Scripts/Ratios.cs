using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ratios : MonoBehaviour {

    public int soilCount; //the number of soil squares (at the start of the game, if this changes this number will need to be updated)
    
    public float totalFood = 0;
    public float totalFuel = 0;
    public float totalConstruction = 0;
    public float totalMedicine = 0;
    public float totalCulture = 0;

    public float foodRatio = 0;
    public float fuelRatio = 0;
    public float constructionRatio = 0;
    public float medicineRatio = 0;
    public float cultureRatio = 0;

    Slider[] bars;
    Slider foodBar;
    Slider fuelBar;
    Slider constructionBar;
    Slider medicineBar;
    Slider cultureBar;

    Text[] texts;
    Text foodText;
    Text fuelText;
    Text constructionText;
    Text medicineText;
    Text cultureText;
    
    public void UpdateRatios()
    {
        foodRatio = (totalFood/soilCount);
        fuelRatio = (totalFuel / soilCount);
        constructionRatio = (totalConstruction / soilCount);
        medicineRatio = (totalMedicine / soilCount);
        cultureRatio = (totalCulture / soilCount);
    
        foodBar.value = foodRatio;
        fuelBar.value = fuelRatio;
        constructionBar.value = constructionRatio;
        medicineBar.value = medicineRatio;
        cultureBar.value = cultureRatio;

        foodText.text = "Food: " + (int)(foodRatio * 100) + "%";
        fuelText.text = "Fuel: " + (int)(fuelRatio * 100) + "%";
        constructionText.text = "Construction: " + (int)(constructionRatio * 100) + "%";
        medicineText.text = "Medicine: " + (int)(medicineRatio * 100) + "%";
        cultureText.text = "Culture: " + (int)(cultureRatio * 100) + "%";
    }

    // Use this for initialization
    void Start () {

        bars = GetComponentsInChildren<Slider>();
        foodBar = bars[0];
        fuelBar = bars[1];
        constructionBar = bars[2];
        medicineBar = bars[3];
        cultureBar = bars[4];

        texts = GetComponentsInChildren<Text>();
        foodText = texts[1];
        fuelText = texts[2];
        constructionText = texts[3];
        medicineText = texts[4];
        cultureText = texts[5];

        soilCount = GameObject.Find("SoilsListAndGeneration").GetComponent<GenerateSoil>().mySoilsList.Count;
		
	}
	
	// Update is called once per frame
	void Update () {

        UpdateRatios();
		
	}
}
