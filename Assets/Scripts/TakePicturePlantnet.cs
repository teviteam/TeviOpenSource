using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TakePicturePlantnet : MonoBehaviour
{
    string website = "http://localhost/";
    private string initialUploadUrl = "kreskanta/upload.php";
    private string analysePlantnetUrl = "kreskanta/parsejsonplantnet.php";

    private int player_id;
    private string tempImageName;

    private WebCamTexture webCamTexture;
    //public Text myText; //for debug in android/ios
    //public Image myImage;//for debug in android/ios
    public Texture2D photo;

    public GameObject connexionObj;

    public PlantnetResultData[] myPlantnetDataArr;

    public GameObject victoryUI;
    public Text victoryText;
    public Text commonName;
    public Text match;
    private string wikiLink;
    public Text link;
    public Text reward;

    public int rewardAmount = 0;

    private Inventory InventoryReference;
    [HideInInspector]
    public bool pictureInProcess = false;

    //ui for plantnet
    public GameObject cameraButton;
    public GameObject quitPlantnetButton;
    public GameObject loadingObj;

    private string apikey = "";


    void Start()
    {
        connexionObj = GameObject.Find("ConnectionInfos");
        if (connexionObj)
        {
            website = connexionObj.GetComponent<Connecting>().website;
            player_id = connexionObj.GetComponent<ConnectionInfo>().myPlayersData.playerId;
        }
        else
        {
            website = "http://localhost/";
            player_id = 0;
        }

        InventoryReference = GameObject.Find("InventoryObject").GetComponent<Inventory>();

        webCamTexture = new WebCamTexture(WebCamTexture.devices[0].name);
        this.GetComponent<Renderer>().material.mainTexture = webCamTexture;
        webCamTexture.Play();

        apikey = HIDDENdata.apikey;//plantnet won't work without an API Key, you can request a test api key from them!
    }
    
    public void TakePhoto()
    {
        if (!pictureInProcess)//to avoid taking two pictures at once
        {
            pictureInProcess = true;
            Debug.Log("tots taking a photo");
            loadingObj.SetActive(true);
            cameraButton.SetActive(false);

            //myText.text = "TakePhoto asks coroutine";
            StartCoroutine("CoroutineTakePhoto");
        }
    }

    private IEnumerator CoroutineTakePhoto()
    {

        Debug.Log("Begin CoroutineTakePhoto");

        photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        //Encode to a PNG
        byte[] bytes = photo.EncodeToPNG();

        /*
        //if you want to see the image taken before upload, for debug purposes
        Rect rec = new Rect(0, 0, photo.width, photo.height);
        Sprite mySprite;
        mySprite=Sprite.Create(photo, rec, new Vector2(0.5f, 0.5f), 100.0f);
        */

        //unique name for the image
        float tempsTimef = Time.deltaTime * 100000;
        int tempsTime = (int)tempsTimef;
        tempImageName = player_id.ToString()+"_"+ tempsTime.ToString();
 
        // Create a Web Form
        string textTest = "Upload Image";
        WWWForm form = new WWWForm();
        form.AddField("submit", textTest);
        form.AddField("image_name", tempImageName);
        form.AddBinaryData("fileToUpload", bytes); 

        // Upload to a cgi script
        WWW w = new WWW(website+initialUploadUrl, form); 
        yield return w;

        if (w.error != null)
        {
            Debug.Log(w.error);
            //myText.text = w.error;
        }
        else
        {
            Debug.Log("Finished Uploading Screenshot : " + w.text);
            //myText.text = w.text;

            StartCoroutine("ReadPlantnetAnswer");
        }
    }

    private IEnumerator ReadPlantnetAnswer()
    {

        // #to replace by the id of the image or the datetime+this
        string urlPlantnet = "https://my-api.plantnet.org/v1/identify/all?images="+ website + "kreskanta/images_upload/"+ tempImageName 
            + ".png&organs=leaf&lang=en&api-key="+apikey;

        Debug.Log("urlPlantnet: "+urlPlantnet);
        WWW w1 = new WWW(urlPlantnet); 
        yield return w1;

        WWWForm form = new WWWForm();
        form.AddField("myform_urltoparse", urlPlantnet);
        form.AddField("myform_playerid", player_id);
        form.AddField("myform_tempImageName", tempImageName);

        using (WWW w = new WWW(website + analysePlantnetUrl, form))
        {
            yield return w;
            if (w.error != null)
            {
                Debug.Log(w.error); //if there is an error, tell us
            }
            else
            {
                Debug.Log("www ok");
                Debug.Log("php: " + w.text); //here we return the data our PHP told us
                if (w.text == "not a plant") //if plantnet return 404 error
                {
                    Debug.Log("not a plant at all");
                    if (victoryUI)
                    {
                        victoryUI.SetActive(true);
                        commonName.text = "";
                        victoryText.text = "";
                        match.text = "Our scientists don't think this is a plant :-/";
                        wikiLink = "https://en.wikipedia.org/wiki/plant";
                        link.text = wikiLink;
                        reward.text = "";
                        this.transform.parent.gameObject.SetActive(false);
                    }
                }
                else if (w.text == "not sure") //if plantnet gives a result <5
                {
                    Debug.Log("not clearly a plant");
                    if (victoryUI)
                    {
                        victoryUI.SetActive(true);
                        commonName.text = "";
                        victoryText.text = "";
                        match.text = "Our scientists don't think this is a plant :-/";
                        wikiLink = "https://en.wikipedia.org/wiki/plant";
                        link.text = wikiLink;
                        reward.text = "";
                        this.transform.parent.gameObject.SetActive(false);
                    }
                }
                else //we found a plant, whoot
                {
                    string jsonString = fixJson(w.text);
                    //Debug.Log(jsonString);
                    myPlantnetDataArr = JsonHelper.FromJson<PlantnetResultData>(jsonString);
                    //myText.text = "best : " + myPlantnetDataArr[0].plant0 + " score : " + myPlantnetDataArr[0].score0;
                    if(victoryUI)
                    {
                        
                        victoryUI.SetActive(true);
                        commonName.text = myPlantnetDataArr[0].common0;  //common name                    
                        victoryText.text = myPlantnetDataArr[0].plant0;   //scientific name
                        match.text = Mathf.RoundToInt(myPlantnetDataArr[0].score0) + "% Match";
                        wikiLink = "https://en.wikipedia.org/wiki/" + System.Text.RegularExpressions.Regex.Replace(myPlantnetDataArr[0].plant0, " ", "_");
                        link.text = wikiLink;

                        //load or reload all plantnet results for the user
                        yield return StartCoroutine(connexionObj.GetComponent<Connecting>().OnlineLoadPlantnetResults());

                        //check how many identical plants exists for last recognized plant
                        int numberOfIdenticalPlants = CheckIdenticalPlants(myPlantnetDataArr[0].plant0);

                        RewardPlantnet(numberOfIdenticalPlants);                         
                    }
                }             
            }
            loadingObj.SetActive(false);
            cameraButton.SetActive(true);
        }
    }

    string fixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }

    public void CloseVictoryWindowAndRelaunchPlantnet(GameObject myObject)
    {
        myObject.SetActive(false);
        pictureInProcess = false;
    }

    public void exchangeCameraButtons()
    {
        if (cameraButton.activeSelf)
            {
                cameraButton.SetActive(false);
            quitPlantnetButton.SetActive(true);
        }
            if (!cameraButton.activeSelf)
            {
            cameraButton.SetActive(true);
            quitPlantnetButton.SetActive(false);
        }
    }

    public void GoToURL()
    {
        Application.OpenURL(wikiLink);
    }

    public void RegisterButton()
    {
        Debug.Log("real RegisterButton clicked");        
    }    

    //checks how many plants have been recognized as the same as the last player's picture
    private int CheckIdenticalPlants(string scientificName)
    {
        //Debug.Log("scientificName : "+scientificName);
        int numberOfIdenticalPlants = 0;

        if (InventoryReference)
        {
            foreach (PlantnetResultData result in InventoryReference.plantnetResultDatas)
            {
                //Debug.Log("result.plant0 : " + result.plant0);
                if (result.plant0 == scientificName)
                {
                    numberOfIdenticalPlants++;
                }
            }
        }
        Debug.Log("We found "+ numberOfIdenticalPlants + " identical plant to last one (1 means it is unique)");
        return numberOfIdenticalPlants;
    }

    //reward the player after taking a picture recognized as a plant, according to the numbers of plants that have already been taken
    private void RewardPlantnet(int numberOfIdenticalPlants)
    {
        //reward amount according to the number of identical plants, only for water and fertilizer
        if (numberOfIdenticalPlants == 1)
        {
            rewardAmount = 10;
        }
        else if (numberOfIdenticalPlants < 5)
        {
            rewardAmount = 4;
        }
        else
        {
            rewardAmount = 2;
        }

        if (InventoryReference)
        { 
            //if the player doesn't have water we gave some to her
            if (InventoryReference.inventory[1].itemCount == 0)
            {
                Debug.Log("the player doesn't have water. We give her" + rewardAmount);
                if (numberOfIdenticalPlants == 1)
                {
                    reward.text = "Wow, a new plant! Here's " + rewardAmount + " Watering Cans for your efforts.";
                }
                else
                {
                    reward.text = "We have seen that plant already, don't we? But thanks anyway. Here's " + rewardAmount + " Watering Cans.";
                }

                InventoryReference.AddToInventoryInGame("WateringCan", rewardAmount);
            }
            //if the player doesn't have fertilizer we gave some to her
            else if (InventoryReference.inventory[2].itemCount == 0)
            {
                Debug.Log("the player doesn't have fertilizer. We give her" + rewardAmount);
                if (numberOfIdenticalPlants == 1)
                {
                    reward.text = "Wow, a new plant! Here's " + rewardAmount + " Fertilizers for your efforts.";
                }
                else
                {
                    reward.text = "We have seen that plant already, don't we? But thanks anyway. Here's " + rewardAmount + " Fertilizers.";
                }
                InventoryReference.AddToInventoryInGame("Fertilizer", rewardAmount);
            }
            //else we give a random seed if there it is not a very well known plant. Else, the player gets nothing
            else
            {
                if (numberOfIdenticalPlants > 3)
                {
                    reward.text = "We have seen too many pictures of this plant already. We want to see different plants.";
                }
                else
                {
                    string[] seeds = { "BananaSeed", "BreadfruitSeed", "ShampooGingerSeed", "SugarcaneSeed", "WildindigoSeed" };
                    int rewardItem = Random.Range(0, seeds.Length);
                    reward.text = "Here's one " + System.Text.RegularExpressions.Regex.Replace(seeds[rewardItem], "[A-Z]", " $0") + " for your efforts.";
                    InventoryReference.AddToInventoryInGame(seeds[rewardItem], 1);//we never give more than 1 seed 
                }
            }
        }
        this.transform.parent.gameObject.SetActive(false);
    }

}