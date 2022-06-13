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
        //Test();
    }

    private void Update()
    {
        DebugQuest();
    }
    private void Test()
    {
        proceedingQuests.Add(1, allQuests[1]);
        proceedingQuests.Add(2, allQuests[2]);
        proceedingQuests.Add(3, allQuests[3]);

        completedQuests.Add(4, allQuests[4]);
        completedQuests.Add(5, allQuests[5]);
        completedQuests.Add(6, allQuests[6]);
    }

    private void LoadQuests()
    {
        Quest[] quests = Resources.LoadAll<Quest>("ScriptableObjects/Quest");
        foreach (Quest item in quests)
        {
            allQuests.Add(item.questId, item);
        }
    }

    public void AllResetQuests()
    {
        foreach (Quest item in proceedingQuests.Values)
        {
            ResetQuest(item);
        }
        foreach (Quest item in completedQuests.Values)
        {
            ResetQuest(item);
        }
        proceedingQuests.Clear();
        completedQuests.Clear();
    }

    public void ResetQuests()
    {
        foreach (Quest item in proceedingQuests.Values)
        {
            ResetQuest(item);
        }
        proceedingQuests.Clear();

    }
    public bool IsProceeding(Quest quest)
    {
        return proceedingQuests.ContainsKey(quest);
    }
    public bool IsComplete(Quest quest)
    {
        return completedQuests.ContainsKey(quest);
    }
    private void ResetQuest(Quest quest)
    {
        quest.clear = false;
        if (quest.resetPrevQuest && quest.prevQuest != null)
        {
            quest.prevQuest.clear = false;
            if (completedQuests.ContainsKey(quest.prevQuest.questId))
                completedQuests.Remove(quest.prevQuest.questId);

            Debug.Log(quest.prevQuest + " : " + quest.prevQuest.clear);
            if (quest.prevQuest.resetPrevQuest)
            {
                ResetQuest(quest.prevQuest);
            }
        }
    }

    public void AddQuest(Quest quest)
    {
        proceedingQuests.Add(quest.questId, quest);
        UIManager uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        uIManager.questUI.SetQuestUIText(quest);
    }

    public void RemoveQuest(Quest quest)
    {
        proceedingQuests.Remove(quest.questId);
    }

    public void ClearQuest(Quest quest)
    {
        proceedingQuests.Remove(quest.questId);
        quest.clear = true;
        completedQuests.Add(quest.questId, quest);
        UIManager uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        uIManager.questUI.SetQuestUIText(quest);
    }

    public bool CheckAvailableQuest(Quest quest)
    {
        bool check = false;
        if (quest.prevQuest == null || quest.prevQuest.clear)
        {
            if (!proceedingQuests.ContainsKey(quest.questId))
            {
                if (!quest.clear)
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

    public bool CheckClearQuest(Quest quest)
    {
        bool check = false;
        for (int i = 0; i < quest.clearValue.values.Count; i++)
        {
            if (quest.clearValue.values[i].type == PropertyType.INT)
            {
                if (quest.clearValue.values[i].intValue <= quest.clearValue.values[i].currentIntValue)
                {
                    Debug.Log("퀘스트 클리어");
                    check = true;
                }
            }
            else if (quest.clearValue.values[i].type == PropertyType.BOOL)
            {
                if (quest.clearValue.values[i].boolValue)
                {
                    Debug.Log("퀘스트 클리어");
                    check = true;
                }
            }

            if (!check)
            {
                break;
            }
        }
        return check;
    }

    public void SetQuestEmptyValue(Quest quest) //테스트용 초기화
    {
        for (int i = 0; i < quest.clearValue.values.Count; i++)
        {
            if (quest.clearValue.values[i].type == PropertyType.INT)
            {
                quest.clearValue.values[i].currentIntValue = 0;
                quest.clear = false;
                Debug.Log(quest.clearValue.values[i].intValue);
                Debug.Log(quest.clearValue.values[i].currentIntValue);
                Debug.Log(quest.clear);
            }
            else if (quest.clearValue.values[i].type == PropertyType.BOOL)
            {
                quest.clearValue.values[i].boolValue = false;
                quest.clear = false;
                Debug.Log(quest.clearValue.values[i].boolValue);
            }
        }
    }

    public void AddQuestValue(Quest quest)
    {
        EventManager eventManager = ManagerObject.Instance.GetManager(ManagerType.EventManager) as EventManager;
        eventManager.GetEventTrigger(typeof(DieEventTrigger)).Add(new GlobalEventTrigger.DieEvent((hitEntity, attackEntity) =>
        {
            if (hitEntity.CompareTag("Enemy"))
            {
                for (int i = 0; i < quest.clearValue.values.Count; i++)
                {
                    if (hitEntity.entityData == quest.clearValue.values[i].entityData)
                    {
                        if (quest.clearValue.values[i].type == PropertyType.INT)
                        {
                            if (quest.clearValue.values[i].intValue > quest.clearValue.values[i].currentIntValue)
                            {
                                quest.clearValue.values[i].currentIntValue++;
                                Debug.Log(quest.clearValue.values[i].currentIntValue);
                            }
                        }
                    }
                }
            }
        }));
    }

    public void DebugQuest()
    {
        foreach (var item in proceedingQuests.Values)
        {
            //Debug.Log("proceedingQuests.Values : " + item);
        }
        foreach (var item in proceedingQuests.Keys)
        {
            //Debug.Log("proceedingQuests.Keys : " + item);
        }
        foreach (var item in completedQuests.Values)
        {
            //Debug.Log("completedQuests.Values : " + item);
        }
        foreach (var item in completedQuests.Keys)
        {
            Debug.Log("completedQuests.Keys : " + item);
        }
        // foreach (var item in allQuests.Values)
        // {
        //     Debug.Log(item);
        // }
        // foreach (var item in allQuests.Keys)
        // {
        //     Debug.Log(item);
        // }
    }
}
