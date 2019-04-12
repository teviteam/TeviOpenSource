using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestList : MonoBehaviour {

    public GameObject QuestTextObj;
    QuestManager questManager;

    private void Start()
    {
        if (QuestTextObj)
        {
            questManager = QuestTextObj.GetComponent<QuestManager>();
        }
        UpdateQuestList();
    }


    public void UpdateQuestList()
    {
        if (questManager)
        {
            if (questManager.quests.Count > 0)
            {
                foreach (var quest in questManager.quests)
                {
                    if (quest.active)
                    {
                        GetComponentsInChildren<Text>()[1].text = questManager.questText.text + "\n\nYour reward will be: <b>" + quest.rewardValue + " " + System.Text.RegularExpressions.Regex.Replace(quest.rewardItem, "[A-Z]", " $0") + "</b>";
                    }
                }
                Debug.Log("quest list has been updated on the UI. Holla!");
            }
        }
    }
}
