using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Manager
{
    public Hashtable proceedingQuests = new Hashtable();
    public Hashtable completedQuests = new Hashtable();

    public void AddQuest(Quest quest)
    {
        proceedingQuests.Add(quest.questId, quest);
    }

    public void RemoveQuest(Quest quest)
    {
        proceedingQuests.Remove(quest.questId);
    }

    public void ClearQuest(Quest quest)
    {
        proceedingQuests.Remove(quest.questId);
        completedQuests.Add(quest.questId, quest);
    }

    public void CheckAvailableQuest()
    {

    }

}
