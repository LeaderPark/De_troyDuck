using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Manager
{
    private HashSet<Quest> proceedingQuests;
    private HashSet<Quest> completedQuests;
    
    public System.Action OnChanged; 

    public void AddQuest(Quest quest)
    {
        proceedingQuests.Add(quest);
        OnChanged?.Invoke();
    }

    public void RemoveQuest(Quest quest)
    {
        proceedingQuests.Remove(quest);
        OnChanged?.Invoke();
    }

    
}
