using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Manager
{
    public Hashtable proceedingQuests = new Hashtable();
    public Hashtable completedQuests = new Hashtable();
    public Hashtable allQuests = new Hashtable();

    void Awake()
    {
        LoadQuests();
    }

    private void LoadQuests()
    {
        Quest[] quests = Resources.LoadAll<Quest>("ScriptableObjects/Quest");
        foreach (Quest item in quests)
        {
            allQuests.Add(item.questId, item);
        }
    }
    public void developerResetQuest()
    {
        foreach (Quest item in allQuests.Values)
        {
            ResetQuest(item.questId);
        }
    }

    public void ResetQuests()
    {
        foreach (Quest item in proceedingQuests.Values)
        {
            if (item != null && item.resetPrevQuest)
                ResetQuest(item.questId);
        }
        proceedingQuests.Clear();
        completedQuests.Clear();
    }

    public Quest GetQuest(int id)
    {
        Debug.Assert(allQuests.ContainsKey(id), "Quest is unavailable.");
        return allQuests[id] as Quest;
    }

    public bool IsProceeding(int id)
    {
        return proceedingQuests.ContainsKey(id);
    }
    public bool IsComplete(int id)
    {
        return completedQuests.ContainsKey(id);
    }

    private void ResetQuest(int id)
    {
        Quest resetQuest = GetQuest(id);
        do
        {
            RemoveQuestComplete(id);

            if (resetQuest.resetPrevQuest && resetQuest.prevQuest != null)
            {
                resetQuest = resetQuest.prevQuest;
            }
            else resetQuest = null;

        } while (resetQuest != null);
    }

    public void SetQuestProceeding(int id)
    {
        Quest quest = GetQuest(id);
        if (!IsProceeding(id))
        {
            proceedingQuests.Add(id, quest);
            UIManager uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
            uIManager.SetQuestUI(quest);
        }
    }

    public void RemoveQuestProceeding(int id)
    {
        proceedingQuests.Remove(id);
    }

    public void SetQuestComplete(int id)
    {
        Quest quest = GetQuest(id);
        if (!IsComplete(id))
        {
            completedQuests.Add(id, quest);
            UIManager uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
            uIManager.SetQuestUI(quest);
        }
    }

    public void RemoveQuestComplete(int id)
    {
        completedQuests.Remove(id);
    }

    public void ClearQuest(int id)
    {
        RemoveQuestProceeding(id);
        SetQuestComplete(id);
        // UIManager uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        // uIManager.SetQuestUI(GetQuest(id));
    }

    public bool CheckAvailableQuest(int id)
    {
        bool check = false;
        Quest quest = GetQuest(id);
        if (quest.prevQuest == null || IsComplete(quest.prevQuest.questId))
        {
            if (!proceedingQuests.ContainsKey(quest.questId))
            {
                if (!IsComplete(quest.questId))
                {
                    check = true;
                }
                else
                {
                    Debug.Log("이미 진행한 퀘스트 입니다.");
                    check = false;
                }
            }
            else
            {
                Debug.Log("이미 진행하고 있는 퀘스트 입니다.");
                check = false;
            }
        }
        else
        {
            Debug.Log("전 퀘스트 " + quest.prevQuest + "를 클리어해야 합니다.");
            check = false;
        }
        return check;
    }
}
