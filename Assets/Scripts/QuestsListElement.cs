using System.Collections;
using UnityEngine;

[System.Serializable]

public class QuestsListElement {

    public int id_quest;

    public string questLore = "ass: absent stint story";

    /* get N X items = 0
           includes: items, tools, seeds, fruit, etc.
      get N or more y ratios = 1
      get to N level = 2
      use N X items = 3 (like: plant x seeds, or water x times)    
     
     */
    public int kindOfQuest = 0;

    //either items or ratios, for ratios:
    /*food
      fuel
      construction
      medicine
      culture
        */
    public string objectiveItem = "item name of no better definition";

    public int objectiveValue = 0;

    public string rewardItem = "reward item name of no better definition";

    public int rewardValue = 0;

    public int multiplierXP = 1;

    public bool active = false;

    //if 0: not a tutorial quest, and is called randomly
    public int tutorialQuestNumber = 0;

    public QuestsListElement(string newQuestLore, 
                                int newKindOfQuest, 
                                string newObjectiveItem,
                                int newObjectiveValue,
                                string newRewardItem,
                                int newRewardValue,
                                bool newActive)
    {
        this.questLore = newQuestLore;
        this.kindOfQuest = newKindOfQuest;
        this.objectiveItem = newObjectiveItem;
        this.objectiveValue = newObjectiveValue;
        this.rewardItem = newRewardItem;
        this.rewardValue = newRewardValue;
        this.active = newActive;
    }

    //TODO add int questsSinceCompleted, to make sure quests are not repeated
    //TODO? quest difficulties

}
