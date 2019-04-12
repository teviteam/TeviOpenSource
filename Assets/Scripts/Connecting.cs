using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Connecting : MonoBehaviour
{
    [Header("General variables")]
    public string website = "http://localhost/";
    //http://localhost/
    
    private GameObject ConnectionInfosObject;
    public GameObject RegisterUIGroup;
    public GameObject LoginUIGroup;

    public bool GameLoadCompleted = false;

    public string hash = ""; //change your secret code, and remember to change into the PHP file too

    [Header("Login variables")]
    private string loginUrl = "kreskanta/login_kreskanta.php";
    public InputField myMailLogin;
    public InputField myPassword;
    public Text errorMessages;

    [Header("GetPlayerData variables")]
    private string getPlayerDataUrl = "kreskanta/get_players_info.php";
    private string getPlayerText;

    [Header("Registering variables")]
    private string registerUrl = "kreskanta/register_kreskanta.php";
    public InputField myUsernameRegister;
    public InputField myMailRegister;
    public InputField myPasswordRegister1;
    public InputField myPasswordRegister2;
    public Text errorMessagesRegister;

    [Header("Soil and Inventory Saving variables")]
    private string generateSoilUrl = "kreskanta/generate_soil.php";
    private string saveSoilUrl = "kreskanta/save_soil.php";
    private float timerSave;
    private bool savingSoilInProcess = false;
    private float currentSaving = 5.0f;
    private GameObject SoilGenerationObject;
    private string generateInventoryUrl = "kreskanta/generate_inventory.php";
    private string saveInventoryUrl = "kreskanta/save_inventory.php";
    private GameObject InventoryObject;
    public bool savingInventoryInProcess = false;

    private string deleteAccountUrl = "kreskanta/delete_account.php";

    private string setShipmentUrl = "kreskanta/set_shipment.php";

    public bool newPlayer = true;

    public GameObject QuestTextObj;
    private string loadQuestTemplateUrl = "kreskanta/quest_load_template.php";
    private string loadPlayerQuestsUrl = "kreskanta/quest_load_player.php";
    private string addPlayerQuestUrl = "kreskanta/quest_player_addnew.php";
    private string updatePlayerQuestUrl = "kreskanta/quest_player_update_complete.php";
    public bool PlayerQuestLoadInProgress = false;
    public bool PlayerQuestUpdateInProgress = false;
    private string getPlayerQuestId = "kreskanta/quest_get_id.php";

    private string loadPlantnetResultsUrl = "kreskanta/loadplantnetresults.php";


    void Start()
    {
        website = HIDDENdata.website;
        hash = HIDDENdata.hash;

        ConnectionInfosObject = GameObject.Find("ConnectionInfos");
        if (errorMessages)
        {
            //errorMessages.text = "errors will be displayed here";
            errorMessages.text = "";
        }
    }

    private void Update()
    {
        if (GameLoadCompleted == true && savingSoilInProcess==false && savingInventoryInProcess == false && Time.time> (timerSave+currentSaving))
        {
            //Debug.Log("saving begins");
            savingInventoryInProcess = true;
            savingSoilInProcess = true;
            currentSaving += 5.0f;
            StartSavingSoils();
            StartSavingInventory();
        }
    }

    public void LoginButton()
    {
        Debug.Log("LoginButton clicked");
        StartCoroutine(OnlineLogin());
    }

    private IEnumerator OnlineLogin()
    {
        if (ConnectionInfosObject)
        {
            if (myMailLogin.text != "")
            {
                if (myPassword.text != "")
                {
                    ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData.playerEmail = myMailLogin.text;
                    ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData.playerPassword = myPassword.text;
                    Debug.Log("We have a login and a password, trying to connect, login=" + myMailLogin.text + " pass=" + myPassword.text);

                    WWWForm form = new WWWForm();
                    form.AddField("myform_hash", hash); //add your hash code to the field myform_hash, check that this variable name is the same as in PHP file
                    form.AddField("myform_email", myMailLogin.text);
                    form.AddField("myform_pass", myPassword.text);

                    using (WWW w = new WWW(website+ loginUrl, form))
                    {
                        yield return w;
                        if (w.error != null)
                        {
                            errorMessages.text = w.error;
                            Debug.Log(w.error); //if there is an error, tell us
                        }
                        else
                        {
                            Debug.Log("www ok");
                            errorMessages.text = "Loading...";
                            Debug.Log("php: " + w.text); //here we return the data our PHP told us
                            if (w.text == "yipiiie")
                            {
                                Debug.Log("We have found the player in the online database and its password matches.");
                                newPlayer = false;
                                StartCoroutine(GetPlayersData()); 
                            }
                            else if (w.text == "Data invalid - cant find name.")
                            {
                                Debug.Log("Email not found");
                                errorMessages.text = "We haven't find your email in our database!";
                            }

                            else if (w.text == "email or password is wrong.")
                            {
                                Debug.Log("The password is not correct or you didn't put your email as login");
                                errorMessages.text = "The password is not correct or you didn't put your email as login!";
                            }
                        }
                    }
                }
                else
                {
                    errorMessages.text = "no password";
                    Debug.Log(errorMessages);
                }
            }
            else
            {
                errorMessages.text = "no login email";
                Debug.Log(errorMessages);

            }
        }
    }

    private IEnumerator GetPlayersData()
    {
        Debug.Log("We are connected, now getting players data");

        WWWForm form2 = new WWWForm();
        form2.AddField("myform_hash", hash); //add your hash code to the field myform_hash, check that this variable name is the same as in PHP file
        form2.AddField("myform_email", ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData.playerEmail);
        using (WWW w = new WWW(website+getPlayerDataUrl, form2))
        {
            yield return w;
            if (w.error != null)
            {
                Debug.Log(w.error); //if there is an error, tell us
            }
            else
            {
                Debug.Log("getplayersdata function is ok");
                getPlayerText = w.text;
                Debug.Log("php: " + getPlayerText); 
                PlayersData loadedData = JsonUtility.FromJson<PlayersData>(getPlayerText);
                ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData = loadedData;
                w.Dispose(); //clear our form in game
                SceneManager.LoadScene("main_scene", LoadSceneMode.Single);
            }
        }
    }

    public void DisplayRegisterUIGroup()
    {
        if(RegisterUIGroup&&LoginUIGroup)
        {
            LoginUIGroup.SetActive(false);
            RegisterUIGroup.SetActive(true);
        }
    }

    public void RegisterButton()
    {
        Debug.Log("real RegisterButton clicked");
        StartCoroutine(OnlineRegister());
    }

    private IEnumerator OnlineRegister()
    {
        if (ConnectionInfosObject)
        {
            if (myUsernameRegister.text != "" && myMailRegister.text != "" && myPasswordRegister1.text != "" && myPasswordRegister2.text != "" )
            {
                if (myPasswordRegister1.text == myPasswordRegister2.text)
                {
                    ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData.playerUsername = myUsernameRegister.text;
                    ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData.playerEmail = myMailRegister.text;
                    ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData.playerPassword = myPasswordRegister1.text;
                    ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData.playerCorp = 1;
                    ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData.playerXP = 8;
                    Debug.Log("System.DateTime.Now.ToString(): " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData.playerAccountCreation = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData.playerLastConnexion = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData.playerLastShipment = "0000-00-00 00:00:00";
                    //ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData.playerLastShipment = ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData.playerAccountCreation;

                    Debug.Log("We have the data we need, trying to register, email=" + myMailLogin.text);

                    string playerDataToJson = ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData.SaveToString();

                    Debug.Log("playerDataToJson " + playerDataToJson);
                    WWWForm form = new WWWForm();
                    
                    form.AddField("myform_hash", hash);
                    form.AddField("myform_jsonregister", playerDataToJson);

                    using (WWW w = new WWW(website + registerUrl, form))
                    {
                        yield return w;
                        if (w.error != null)
                        {
                            errorMessagesRegister.text = w.error;
                            Debug.Log(w.error); //if there is an error, tell us
                        }
                        else
                        {
                            Debug.Log("www register ok");
                            //errorMessagesRegister.text = w.text;
                            Debug.Log("php: " + w.text); //here we return the data our PHP told us
                            if (w.text == "yipiiie register")
                            {
                                errorMessagesRegister.text = "Loading a new game" ;
                                Debug.Log("We managed to register, launch the game");
                                newPlayer = true;
                                StartCoroutine(GetPlayersData());
                                //SceneManager.LoadScene("main_scene", LoadSceneMode.Single);
                            }
                            else
                            {
                                errorMessagesRegister.text = w.text;
                            }
                        }
                    }
                }
                else
                {
                    errorMessagesRegister.text = "Please type two identical passwords. Thanks.";
                    Debug.Log(errorMessagesRegister);
                }

            }
            else
            {
                errorMessagesRegister.text = "Please fill all the fields. Thanks.";
                Debug.Log(errorMessagesRegister);
            }
        }
    }

    public void StartOnlineGenerateSoils()
    {
        Debug.Log("trying to launch soil generation with the database");
        StartCoroutine(OnlineGenerateSoils());
    }

    //function to generate the players soils when registering the player for the first time and getting their id + at each game load
    private IEnumerator OnlineGenerateSoils()
    {
        SoilGenerationObject = GameObject.Find("SoilsListAndGeneration");
        if (SoilGenerationObject)
        {
            List<Soils> currentSoilList = SoilGenerationObject.GetComponent<GenerateSoil>().mySoilsList;
            if (currentSoilList.Count != 0  )
            {
                Debug.Log("We have a soil generation object");

                SoilsData[] soilsArray = new SoilsData[currentSoilList.Count];

                for (int i = 0; i < currentSoilList.Count; i++)
                {
                    soilsArray[i] = currentSoilList[i].mySoilsData;
                }

                string soilToJason = JsonHelper.ToJson(soilsArray, true);
                //Debug.Log(soilToJason);

                WWWForm form = new WWWForm();

                form.AddField("myform_hash", hash);
                form.AddField("myform_soils", soilToJason);

                using (WWW w = new WWW(website + generateSoilUrl, form))
                {
                    yield return w;
                    if (w.error != null)
                    {
                        Debug.Log(w.error); //if there is an error, tell us
                    }
                    else
                    {
                        Debug.Log("www soils ok");
                       // Debug.Log("php: " + w.text); //here we return the data our PHP told us

                        //--- loading soils from DBB ----
                        

                        string jsonString = fixJson(w.text);
                        Debug.Log(jsonString);
                        
                        SoilsData[] newSoilArray = JsonHelper.FromJson<SoilsData>(jsonString);

                        for (int i = 0; i < currentSoilList.Count; i++)
                        {
                           currentSoilList[i].mySoilsData = newSoilArray[i];
                           currentSoilList[i].LoadSoil();
                        }                                       

                        Debug.Log("we got our garden database id");

                        StartOnlineGenerateInventory();
                    }
                }
            }
            else
            {                
                Debug.Log("no soils in the list");
            }
        }
        else
        {
            Debug.Log("no soil generation object found");
        }
    }

    public void StartSavingSoils()
    {
        //Debug.Log("trying to launch saving soil in the database");
        StartCoroutine(OnlineSaveSoils());
    }

    //function to save the players soils
    private IEnumerator OnlineSaveSoils()
    {
        SoilGenerationObject = GameObject.Find("SoilsListAndGeneration");
        if (SoilGenerationObject)
        {
            List<Soils> currentSoilList = SoilGenerationObject.GetComponent<GenerateSoil>().mySoilsList;
            if (currentSoilList.Count != 0)
            {
                //Debug.Log("We have a soil generation object");

                SoilsData[] soilsArray = new SoilsData[currentSoilList.Count];

                for (int i = 0; i < currentSoilList.Count; i++)
                {
                    soilsArray[i] = currentSoilList[i].mySoilsData;
                }

                string soilToJason = JsonHelper.ToJson(soilsArray, true);
                //Debug.Log(soilToJason);

                WWWForm form = new WWWForm();

                form.AddField("myform_hash", hash);
                form.AddField("myform_soils", soilToJason);

                using (WWW w = new WWW(website + saveSoilUrl, form))
                {
                    yield return w;
                    if (w.error != null)
                    {
                        Debug.Log(w.error); //if there is an error, tell us

                        savingSoilInProcess = false;
                    }
                    else
                    {
                        Debug.Log("www saving soils ok");
                        Debug.Log("php: " + w.text); //here we return the data our PHP told us

                        savingSoilInProcess = false;
                    }
                }
            }
            else
            {
                Debug.Log("no soils in the list");
            }
        }
        else
        {
            Debug.Log("no soil generation object found");
        }
    }

    public void StartOnlineGenerateInventory()
    {
        Debug.Log("trying to launch inventory generation with the database");
        StartCoroutine(OnlineGenerateInventory());
    }

    //function to generate and load the players inventory.
    // when registering the player for the first time it gets  their id 
    private IEnumerator OnlineGenerateInventory()
    {
        
        InventoryObject = GameObject.Find("InventoryObject");
        if (InventoryObject)
        {
            int myPlayerId = ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData.playerId;

            WWWForm form = new WWWForm();

            //at first, the inventory is always empty
            //if the database is empty then we will generate basic inventory from php
            //if the database has already inventory items, then we will load from it
            form.AddField("myform_hash", hash);
            form.AddField("myform_playerid", myPlayerId);

            using (WWW w = new WWW(website + generateInventoryUrl, form))
            {
                yield return w;
                if (w.error != null)
                {
                    Debug.Log(w.error); //if there is an error, tell us
                }
                else
                {
                    Debug.Log("www inventory ok");
                    Debug.Log("json inventory from php"+w.text); 

                    //--- loading inventory from DB ----

                    string jsonString = fixJson(w.text);
                    Debug.Log(jsonString);
                    InventoryData[] newInventoryArray = JsonHelper.FromJson<InventoryData>(jsonString);

                    Debug.Log("newInventoryArray.Length " + newInventoryArray.Length);

                    for (int i = 0; i < newInventoryArray.Length; i++)
                    {
                        //Debug.Log("we add inventory item "+i);
                        InventoryObject.GetComponent<Inventory>().inventoryDatas.Add(newInventoryArray[i]);
                    }
                    InventoryObject.GetComponent<Inventory>().LoadInventory();
                    InventoryObject.GetComponent<Inventory>().BeginShipmentCheck();

                    Debug.Log("we got our inventory database id");

                    //move on to Loading quests
                    StartOnlineLoadQuestsTemplates();
                }
            }
        }
        else
        {
            Debug.Log("no inventory object found");
        }       
    }

    public void StartSavingInventory()
    {
        //Debug.Log("trying to launch saving inventory in the database");
        InventoryObject = GameObject.Find("InventoryObject");
        if (InventoryObject)
        {
            InventoryObject.GetComponent<Inventory>().inventoryPlayersData = ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData;
        }
        StartCoroutine(OnlineSaveInventory());
    }

    //function to save the players inventory
    private IEnumerator OnlineSaveInventory()
    {
        if (InventoryObject)
        {
            List<InventoryData> currentInventory = InventoryObject.GetComponent<Inventory>().inventoryDatas;
            if (currentInventory.Count != 0)
            {
                //Debug.Log("We have an inventory object");

                InventoryData[] inventoryArray = new InventoryData[currentInventory.Count];

                for (int i = 0; i < currentInventory.Count; i++)
                {
                    inventoryArray[i] = currentInventory[i];
                }

                string inventoryToJason = JsonHelper.ToJson(inventoryArray, true);
                //Debug.Log(inventoryToJason);

                WWWForm form = new WWWForm();

                form.AddField("myform_hash", hash);
                form.AddField("myform_inventory", inventoryToJason);

                using (WWW w = new WWW(website + saveInventoryUrl, form))
                {
                    yield return w;
                    if (w.error != null)
                    {
                        Debug.Log(w.error); //if there is an error, tell us
                    }
                    else
                    {
                        //Debug.Log("www saving inventory ok");
                        //Debug.Log("php: " + w.text); //here we return the data our PHP told us
                        savingInventoryInProcess = false;
                    }
                }
            }
            else
            {
                Debug.Log("no inventory data in the list");
            }
        }
        else
        {
            Debug.Log("no inventory object found");
        }
    }

    public void StartDeletingAccount()
    {
        StartCoroutine(DeleteAccount());
    }

    private IEnumerator DeleteAccount()
    {
        int myPlayerId = ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData.playerId;

        WWWForm form = new WWWForm();

        form.AddField("myform_hash", hash);
        form.AddField("myform_playerid", myPlayerId);

        using (WWW w = new WWW(website + deleteAccountUrl, form))
        {
            yield return w;
            if (w.error != null)
            {
                Debug.Log(w.error); //if there is an error, tell us
            }
            else
            {
                //Debug.Log("www deleting account ok");
                //Debug.Log("php: " + w.text); //here we return the data our PHP told us
                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    Application.Quit();
                }
                else {
                    SceneManager.LoadScene("main_menu", LoadSceneMode.Single);
                }
            }
        }            
    }

    public void StartSetShipment()
    {
        StartCoroutine(SetShipment());
    }
    //function that set to current time the playerLastShipment
    private IEnumerator SetShipment()
    {
        Debug.Log("SetShipment begins ");
        int myPlayerId = ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData.playerId;
        string myplayerLastShipment = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Debug.Log("myplayerLastShipment "+myplayerLastShipment);

        WWWForm form = new WWWForm();

        form.AddField("myform_hash", hash);
        form.AddField("myform_playerid", myPlayerId);
        form.AddField("myform_playerLastShipment", myplayerLastShipment);

        using (WWW w = new WWW(website + setShipmentUrl, form))
        {
            yield return w;
            if (w.error != null)
            {
                Debug.Log(w.error); //if there is an error, tell us
            }
            else
            {
                Debug.Log("www updating shipment time ok");
                Debug.Log("php: " + w.text); //here we return the data our PHP told us
                ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData.playerLastShipment = myplayerLastShipment;
                InventoryObject.GetComponent<Inventory>().BeginShipmentCheck();
                InventoryObject.GetComponent<Inventory>().isShipmentInProcess = false;
            }
        }
    }

    public void StartOnlineLoadQuestsTemplates()
    {
        Debug.Log("trying to launch quest templates loading with the database");
        StartCoroutine(OnlineLoadQuestsTemplates());
    }

    //function to load the quest template at each game loading
    private IEnumerator OnlineLoadQuestsTemplates()
    {
        InventoryObject = GameObject.Find("InventoryObject");
        QuestTextObj = GameObject.Find("QuestTextObj");
       
        if (QuestTextObj&& InventoryObject)
        {
            int myPlayerId = ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData.playerId;
            //

            WWWForm form = new WWWForm();

            form.AddField("myform_hash", hash);
            form.AddField("myform_playerid", myPlayerId);

            using (WWW w = new WWW(website + loadQuestTemplateUrl, form))
            {
                yield return w;
                if (w.error != null)
                {
                    Debug.Log(w.error); //if there is an error, tell us
                }
                else
                {
                    //Debug.Log("www load quest template ok");
                    //Debug.Log("json quest template from php" + w.text);

                    //empty the list
                    QuestTextObj.GetComponent<QuestManager>().quests.Clear();

                    //--- loading quests templates from DB ----

                    string jsonString = fixJson(w.text);
                    //Debug.Log(jsonString);
                    QuestsListElement[] newQuestTemplateList = JsonHelper.FromJson<QuestsListElement>(jsonString);

                    //Debug.Log("newInventoryArray.Length " + newQuestTemplateList.Length);

                    for (int i = 0; i < newQuestTemplateList.Length; i++)
                    {
                        //Debug.Log("we add quest template item " + i);
                        QuestTextObj.GetComponent<QuestManager>().quests.Add(newQuestTemplateList[i]);
                    }
                    //Debug.Log("we got our quest templates !!");

                    //on to loading player quests
                    StartOnlineLoadPlayerQuests();
                }
            }
        }
        else
        {
            Debug.Log("no inventory object found");
        }        
    }

    public void StartOnlineLoadPlayerQuests()
    {
        Debug.Log("trying to launch player quest loading with the database");
        StartCoroutine(OnlineLoadPlayerQuests());
    }

    //function to load the quest of the player at each game loading (the one he either began or has completed)
    private IEnumerator OnlineLoadPlayerQuests()
    {
        PlayerQuestLoadInProgress = true;
        InventoryObject = GameObject.Find("InventoryObject");
        QuestTextObj = GameObject.Find("QuestTextObj");

        if (QuestTextObj && InventoryObject)
        {
            int myPlayerId = ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData.playerId;

            WWWForm form = new WWWForm();

            form.AddField("myform_hash", hash);
            form.AddField("myform_playerid", myPlayerId);

            using (WWW w = new WWW(website + loadPlayerQuestsUrl, form))
            {
                yield return w;
                if (w.error != null)
                {
                    Debug.Log(w.error); //if there is an error, tell us
                }
                else
                {
                    //Debug.Log("www load quest template ok");

                    //empty the list
                    QuestTextObj.GetComponent<QuestManager>().myQuestPlayerData.Clear();

                    //Debug.Log("json player quest from php" + w.text);
                    
                    //if there is no quest done or began by our player in the db, php will return "vide", else, we load them
                    if (w.text != "vide")
                    {

                        //--- loading players quests from DB ----

                        string jsonString = fixJson(w.text);

                        //Debug.Log(jsonString);
                        QuestPlayerData[] newPlayerQuestList = JsonHelper.FromJson<QuestPlayerData>(jsonString);

                        //Debug.Log("newInventoryArray.Length " + newPlayerQuestList.Length);

                        for (int i = 0; i < newPlayerQuestList.Length; i++)
                        {
                            //Debug.Log("we add player quest item " + i);
                            QuestTextObj.GetComponent<QuestManager>().myQuestPlayerData.Add(newPlayerQuestList[i]);
                        }
                    }

                    yield return OnlineLoadPlantnetResults();

                    //only then is the game properly launched
                    GameLoadCompleted = true; //#to change => all the coroutines for startup should be called in one function
                    timerSave = Time.time;

                    //Debug.Log("we got our player quest !!");
                }
            }
        }
        else
        {
            Debug.Log("no inventory object found");
        }

        PlayerQuestLoadInProgress = false;
    }

    public void StartAddNewPlayerQuests(QuestPlayerData q)
    {
        Debug.Log("StartAddNewPlayerQuests began");
        StartCoroutine(AddNewPlayerQuests(q));
    }
    
    private IEnumerator AddNewPlayerQuests(QuestPlayerData q)
    {
        string playerQuestToJson = q.SaveToString();
        //Debug.Log(playerQuestToJson);

        WWWForm form = new WWWForm();

        form.AddField("myform_hash", hash);
        form.AddField("myform_playerquest", playerQuestToJson);

        using (WWW w = new WWW(website + addPlayerQuestUrl, form))
        {
            yield return w;
            if (w.error != null)
            {
                Debug.Log(w.error); //if there is an error, tell us
            }
            else
            {
                Debug.Log("www player quest addition in the database ok");
                Debug.Log("php: " + w.text); //here we return the data our PHP told us

                //reload players quests in order to get proper id.
                StartGettingPlayerQuestId(q);
            }
        }            
    }

    public void StartUpdatePlayerQuests(QuestPlayerData q)
    {
        Debug.Log("StartUpdatePlayerQuests began");
        StartCoroutine(UpdatePlayerQuests(q));
    }

    //update player quest when they are completed
    private IEnumerator UpdatePlayerQuests(QuestPlayerData q)
    {
        PlayerQuestUpdateInProgress = true;
        string playerQuestToJson = q.SaveToString();
        //Debug.Log(playerQuestToJson);

        WWWForm form = new WWWForm();

        form.AddField("myform_hash", hash);
        form.AddField("myform_playerquest", playerQuestToJson);

        using (WWW w = new WWW(website + updatePlayerQuestUrl, form))
        {
            yield return w;
            if (w.error != null)
            {
                Debug.Log(w.error); //if there is an error, tell us
            }
            else
            {
                //Debug.Log("www player quest update ok");
                //Debug.Log("php: " + w.text); //here we return the data our PHP told us
            }
        }
        PlayerQuestUpdateInProgress = false;
    }


    public void StartGettingPlayerQuestId(QuestPlayerData q)
    {
        Debug.Log("StartGettingPlayerQuestId began");
        StartCoroutine(GettingPlayerQuestId(q));
    }

    //update player quest when they are completed
    private IEnumerator GettingPlayerQuestId(QuestPlayerData q)
    {

        WWWForm form = new WWWForm();

        form.AddField("myform_hash", hash);
        form.AddField("myform_playerid", q.player_id);
        form.AddField("myform_idquest", q.id_quest);

        using (WWW w = new WWW(website + getPlayerQuestId, form))
        {
            yield return w;
            if (w.error != null)
            {
                Debug.Log(w.error); //if there is an error, tell us
            }
            else
            {
                Debug.Log("www get quest id ok");
                Debug.Log("php: " + w.text); //here we return the data our PHP told us
                q.id_quest_player = int.Parse(w.text);
            }
        }
    }

    public void StartOnlineLoadPlantnetResults()
    {
        Debug.Log("trying to launch loading plantnetresults");
        StartCoroutine(OnlineLoadPlantnetResults());
    }

    //function called at startup and once plantnet has recognized a new picture
    public IEnumerator OnlineLoadPlantnetResults()
    {
        int myPlayerId = ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData.playerId;

        WWWForm form = new WWWForm();
        form.AddField("myform_hash", hash);
        form.AddField("myform_playerid", myPlayerId);

        using (WWW w = new WWW(website + loadPlantnetResultsUrl, form))
        {
            yield return w;
            if (w.error != null)
            {
                Debug.Log(w.error); //if there is an error, tell us
            }
            else
            {
                Debug.Log("www for ok loading plantnet results");
                Debug.Log("plantnet result json: " + w.text);

                if (w.text != "No plantnet results")
                {
                    //--- loading plantnet results from DB ----

                    string jsonString = fixJson(w.text);
                    Debug.Log(jsonString);
                    PlantnetResultData[] newPlantnetResultsArray = JsonHelper.FromJson<PlantnetResultData>(jsonString);

                    Debug.Log("newPlantnetResultsArray.Length " + newPlantnetResultsArray.Length);

                    for (int i = 0; i < newPlantnetResultsArray.Length; i++)
                    {
                        //Debug.Log("we add plantnet item "+i);
                        if (InventoryObject)
                        {
                            InventoryObject.GetComponent<Inventory>().plantnetResultDatas.Add(newPlantnetResultsArray[i]);
                        }
                    }

                    Debug.Log("we got our plantnet results");
                }
                else
                {
                    Debug.Log("no plantnet results for this user yet");
                }
            }
        }
    }

    string fixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }
}
