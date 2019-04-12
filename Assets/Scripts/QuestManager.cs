using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public Text questText;

    //public int minVariationObjective = 20;
    //public int maxVariationObjective = 80;

    public GameObject ratiosObj;
    Ratios ratios;

    public GameObject txtBox;

    public GameObject inventoryObj;

    bool newQuest = true;
    bool firstQuestDone = false;
    public float delayFirstQuest = 10;
    private float t = 0;
    bool inspectorUsed = true;
    public bool tutorialFinished = false;
    bool congratsOn = false;

    //TODO does not work
    public int XPperQuest = 50;

    public List<QuestsListElement> quests = new List<QuestsListElement>();
    public List<InventoryData> inventory;
    public List<QuestPlayerData> myQuestPlayerData = new List<QuestPlayerData>();

    //denotes the first tutorial quest (should be 1)
    private int tutI = 1;

    int useItemQuestItem;
    QuestsListElement useItemQuest;
    int getItemQuestItem;
    QuestsListElement getItemQuest;

    bool enoughQuests = false;

    private GameObject ConnectionInfosObject;

    // Use this for initialization
    void Start()
    {

        questText = GetComponent<Text>();
        ratios = ratiosObj.GetComponent<Ratios>();
        ConnectionInfosObject = GameObject.Find("ConnectionInfos");
        t = delayFirstQuest;
        txtBox.GetComponent<Image>().enabled = false;
        InvokeRepeating("CheckCompletionQuests", 1.0f, 1.0f);


        foreach (var quest in quests)
        {
            if (quest.tutorialQuestNumber == 0)
            {
                enoughQuests = true;
            }
        }

        if (myQuestPlayerData.Count != 0)
        {
            delayFirstQuest = 0.1f;
        }

        if (quests.Count < 2) enoughQuests = false;
    }

    // Update is called once per frame
    void Update()
    {
        // delayFirstQuest -= Time.deltaTime;

        //we check we are not loading quest at the moment
        if (ConnectionInfosObject.GetComponent<Connecting>().GameLoadCompleted && newQuest && !congratsOn)
        {
            initiateQuest();
        }
        /*if (!ConnectionInfosObject.GetComponent<Connecting>().PlayerQuestLoadInProgress && !ConnectionInfosObject.GetComponent<Connecting>().PlayerQuestUpdateInProgress && !ConnectionInfosObject.GetComponent<Connecting>().savingInventoryInProcess)
        { 
            if (newQuest)
            {
                initiateQuest();
            }
            if ((delayFirstQuest <= 0 && !firstQuestDone) && newQuest)
            {
                firstQuestDone = true;
                initiateQuest();
            }*/

        if (Input.GetKeyDown(KeyCode.Q))
        {
            initiateQuest();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            ResetTutorial();
        }
    }

    //new version edited by Edwige
    void initiateQuest()
    {
        int questID=0;
        //make quest text box appear
        txtBox.GetComponent<Image>().enabled = true;

        //if this is a not a new player (else questID stays to 0)
        if (myQuestPlayerData.Count == 0)
        {
            questID = 0;
            //Debug.Log("new player, quest id=0");
        }
        else
        {
            bool activeQuest = false;
            //check if there is an active quest
            for (int i = 0; i < myQuestPlayerData.Count; i++)
            {
                if (!myQuestPlayerData[i].quest_completed)
                {
                    questID = myQuestPlayerData[i].id_quest;
                    activeQuest = true;
                    //Debug.Log("we found an active quest, displaying it");
                }
            }

            if (!activeQuest)
            {
                //Debug.Log("no active quest, finding next appropriate one");

                //are we still doing tutorial? if yes, which quest
                bool tutorialFinished = false;
                int currentTuto = 0;
                int tempTuto = 0;

                for (int i = 0; i < myQuestPlayerData.Count; i++)
                {
                    //check if last tutorial quest is finished 
                    //to change if we add tutorial quest
                    if (myQuestPlayerData[i].id_quest == 4 && myQuestPlayerData[i].quest_completed)
                    {
                        tutorialFinished = true;
                        //Debug.Log("tutorialFinished");
                    }
                    else
                    {
                        for (int j = 0; j < quests.Count; j++)
                        {
                            //looking for highest tutorial quest completed
                            if (quests[j].id_quest==myQuestPlayerData[i].id_quest && quests[j].tutorialQuestNumber>currentTuto && myQuestPlayerData[i].quest_completed)
                            {
                                tempTuto = quests[j].id_quest;
                                currentTuto++;
                            }
                        }
                    }
                }

                if (!tutorialFinished)
                {
                    questID = tempTuto+1;
                    //Debug.Log("tutorial not finished, giving quest questID");
                }
                else
                {
                    //gives other quests
                    //# caution: doesn't check the player has the right object in the inventory nor a way to get it
                TryAgain:
                    questID = Random.Range(0, quests.Count);
                    //Debug.Log("giving random later quest");
                    if (quests[questID].tutorialQuestNumber != 0)
                    {
                        //Debug.Log("still tuto, trying again");
                        goto TryAgain;
                    }
                }
            }
        }

        //Ed: add new quest of type questID to the myQuestPlayerData list.
        ConnectionInfosObject = GameObject.Find("ConnectionInfos");
        if (ConnectionInfosObject)
        {
            QuestPlayerData q = new QuestPlayerData();
            q.player_id = ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData.playerId;
            q.id_quest = quests[questID].id_quest;
            q.quest_completed = false;
            q.begin_date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            myQuestPlayerData.Add(q);
            //save to DB
            ConnectionInfosObject.GetComponent<Connecting>().StartAddNewPlayerQuests(q);
        }

        //check what kind of quest is it

        //get item?
        if (quests[questID].kindOfQuest == 0)
        {
            //Debug.Log("get stuff");
            ItemQuest(questID);
        }
        //get to ratio?
        if (quests[questID].kindOfQuest == 1)
        {
            //Debug.Log("get ratio");
            RatioQuest(questID);
        }
        //get to level?
        if (quests[questID].kindOfQuest == 2)
        {
            Debug.LogError("get to level quests not implemented yet");
            LevelQuest(questID);
        }
        //use item?
        if (quests[questID].kindOfQuest == 3)
        {
            if (quests[questID].objectiveItem == "Inspector")
            {
                inspectorUsed = false;
                newQuest = false;
                questText.GetComponent<Text>().enabled = true;
                questText.text = quests[questID].questLore + "\n\nUse <b>" + System.Text.RegularExpressions.Regex.Replace(quests[questID].objectiveItem, "[A-Z]", " $0") + ".</b>";

                quests[questID].active = true;
                //Debug.LogError("You forgot to put reference to function in inspector use function in UIManager... probably.\nMay God have mercy on you, wretched creature.");
                return;
            }
            /*check if there's another type 3 quest active, if there is, everything goes to shit, so it just sends an error ¯\_(ツ)_/¯
            TODO find a way to have multiple type 3 quests active at the same time*/
            foreach (var quest in quests)
            {
                if (quest.active && quest.kindOfQuest == 3)
                {
                    Debug.LogError("EVERYTHING IS BROKEN!!!\n RUUUUUUN!!!\n There are too many (more then one) 'use item' quests active. The time has come to figure out how to have more then one at the same time.");
                }
            }
            //Debug.Log("use items");
            UseItemsQuest(questID);
        }
        newQuest = false;
        //Debug.Log(newQuest);
    }

    void RatioQuest(int questID)
    {
        //TODO generate lore (for all the things)
        //TODO generate objective
        questText.GetComponent<Text>().enabled = true;
        questText.text = quests[questID].questLore + "\n\nGet to <b>" + quests[questID].objectiveValue + " " + System.Text.RegularExpressions.Regex.Replace(quests[questID].objectiveItem, "[A-Z]", " $0") + ".</b>";
        quests[questID].active = true;
    }

    void ItemQuest(int questID)
    {
        //TODO random item?
        //TODO random item value?
        questText.GetComponent<Text>().enabled = true;
        questText.text = quests[questID].questLore + "\n\nGet <b>" + quests[questID].objectiveValue + " " + System.Text.RegularExpressions.Regex.Replace(quests[questID].objectiveItem, "[A-Z]", " $0") + ".</b>";
        quests[questID].active = true;

        getItemQuestItem = quests[questID].objectiveValue;
        getItemQuest = quests[questID];
    }
    void LevelQuest(int questID)
    {
        questText.GetComponent<Text>().enabled = true;
        questText.text = quests[questID].questLore + "\n\nGet to level <b>" + quests[questID].objectiveValue + ".</b>";
        //TODO pupulate

    }

    void UseItemsQuest(int questID)
    {
        questText.GetComponent<Text>().enabled = true;
        questText.text = quests[questID].questLore + "\n\nUse <b>" + Mathf.Abs(quests[questID].objectiveValue) + " " + System.Text.RegularExpressions.Regex.Replace(quests[questID].objectiveItem, "[A-Z]", " $0") + ".</b>";

        quests[questID].active = true;

        //items needed. For readability, one can use negative numbers in the inspector, and this will return its absolute value
        useItemQuestItem = Mathf.Abs(quests[questID].objectiveValue);

        useItemQuest = quests[questID];
    }

    void CheckCompletionQuests()
    {
        foreach (var quest in quests)
        {
            if (quest.active)
            {
                //is it a items quest?
                if (quest.kindOfQuest == 0)
                {
                    //never used
                }
                //is it a ratio quest?
                if (quest.kindOfQuest == 1)
                {
                    //if food => check food
                    if ((quest.objectiveItem == "food") && ((ratios.foodRatio * 100) >= quest.objectiveValue))
                    {
                        QuestCompleted(quest);
                    }
                    //if fuel => check fuel
                    if ((quest.objectiveItem == "fuel") && ((ratios.fuelRatio * 100) >= quest.objectiveValue))
                    {
                        QuestCompleted(quest);
                    }
                    //if construction => check construction
                    if ((quest.objectiveItem == "construction") && ((ratios.constructionRatio * 100) >= quest.objectiveValue))
                    {
                        QuestCompleted(quest);
                    }
                    //if medicine => check medicine
                    if ((quest.objectiveItem == "medicine") && ((ratios.medicineRatio * 100) >= quest.objectiveValue))
                    {
                        QuestCompleted(quest);
                    }
                    //if culture => check culture
                    if ((quest.objectiveItem == "culture") && ((ratios.cultureRatio * 100) >= quest.objectiveValue))
                    {
                        QuestCompleted(quest);
                    }
                }
                //is it a level quest?
                if (quest.kindOfQuest == 2)
                {
                    //TODO populate
                }
                //is it a use item quest?
                if (quest.kindOfQuest == 3)
                {
                    //BAZINGA! I'll never use this! And the code will stay here forevaaaa!! Bhuahahaha!
                    //I see what you do Gio, I see it... :D
                }

            }
        }
    }
       
    void QuestCompleted(QuestsListElement quest)
    {
        Debug.Log("QuestCompleted "+quest.id_quest);
       
        txtBox.GetComponent<Image>().enabled = true;

        //Edwige:updating the result with completion date!    
        foreach(var q in myQuestPlayerData)
        {
            if(quest.id_quest==q.id_quest)
            {
                //update myQuestPlayerData list with completion date 
                q.quest_completed = true;
                q.completion_date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //update in the DB
                ConnectionInfosObject.GetComponent<Connecting>().StartUpdatePlayerQuests(q);
            }
        }        

        //Edwige: check the last quest of tutorial is complete to begin regular shipment
        if (quest.tutorialQuestNumber == 4)
        {
            Debug.Log("tutorial quest 4 finished");
            tutorialFinished = true;
            ConnectionInfosObject.GetComponent<Connecting>().StartSetShipment();
        }
        questText.GetComponent<Text>().enabled = true;
        questText.text = "Congratulations! You have completed the Quest!\nHere's <b>" + quest.rewardValue + " " + System.Text.RegularExpressions.Regex.Replace(quest.rewardItem, "[A-Z]", " $0") + "</b> and " + XPperQuest * quest.multiplierXP + " XP for you.\nThe Mars commune will be forever grateful.";
        quest.active = false;
        congratsOn = true;

        inventoryObj.GetComponent<Inventory>().AddToInventoryInGame(quest.rewardItem, quest.rewardValue);
        //TODO give XP += XPperQuest*quests[questID].multiplierXP;
        ConnectionInfosObject.GetComponent<Connecting>().StartSavingInventory();

        newQuest = true;
    }

    public void DeactivateBox()
    {
        Debug.Log("BEGONE! Quest window!");
        txtBox.GetComponent<Image>().enabled = false;
        questText.GetComponent<Text>().enabled = false;
        //questText.text = "";
        //Debug.Log("This, indeed, works");
        congratsOn = false;
        if (newQuest == true)
        {
            initiateQuest();
            //Debug.Log("Hear ye! Hear Ye! The quest has been initiated by clicking on the boxa after one other quest has been completed");
        }
    }

    //called from the function RemoveFromInventory() in Inventory.cs
    public void useItemQuestCheck(string itemName)
    {
        //check that there is an active quest, that it's a 'use item' quest and that it's asking for itemName item
        foreach (var quest in quests)
        {
            if (quest.active && quest.kindOfQuest == 3 && quest.objectiveItem == itemName)
            {
                useItemQuestItem--;
                if (useItemQuestItem <= 0)
                {                                               
                    QuestCompleted(useItemQuest);
                    useItemQuest = null;
                }
            }
        }
        ConnectionInfosObject.GetComponent<Connecting>().StartSavingInventory();
    }

    public void getItemQuestCheck(string itemName, int numAdded)
    {
        foreach (var quest in quests)
        {
            if (quest.active && quest.kindOfQuest == 0 && quest.objectiveItem == itemName)
            {
                //do stuff

                //Debug.Log("getItemQuestCheck ok");
                getItemQuestItem = getItemQuestItem - numAdded;
                if (getItemQuestItem <= 0)
                {
                    QuestCompleted(getItemQuest);
                    getItemQuest = null;
                }
            }
        }
    }
    public void inspectorQuest()
    {
        if (!inspectorUsed && quests[1].active)
        {
            QuestCompleted(quests[1]);
        }
    }

    public void ResetTutorial()
    {
        tutI = 1;
    }


}