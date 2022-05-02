using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Manager
{
    public Hashtable proceedingQuests = new Hashtable();
    private Hashtable completedQuests;
    
    public System.Action OnChanged; 

    public void AddQuest(Quest quest)
    {
        proceedingQuests.Add(quest.questId, quest);
        OnChanged?.Invoke();
    }

    public void RemoveQuest(Quest quest)
    {
        proceedingQuests.Remove(quest.questId);
        OnChanged?.Invoke();
    }
}
