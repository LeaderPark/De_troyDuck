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

    public bool CheckAvailableQuest(Quest quest)
    {
        bool check = false;
        if(quest.prevQuest == null || quest.prevQuest.clear)
        {
            if(!proceedingQuests.ContainsKey(quest.questId))
            {
                if(!quest.clear)
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
            Debug.Log("전 퀘스트 " + quest.prevQuest + "를 깨고 오세요");
            check = false;
        }
        return check;
    }

    public bool CheckClearQuest(Quest quest) 
    {
        bool check = false;
        for(int i = 0; i < quest.clearValue.values.Count; i++)
        {
            if(quest.clearValue.values[i].type == PropertyType.INT)
            {
                if(quest.clearValue.values[i].intValue <= quest.clearValue.values[i].currentIntValue)
                {
                    Debug.Log("퀘스트 클리어");
                    check = true;
                }
            }
            else if(quest.clearValue.values[i].type == PropertyType.BOOL)
            {
                if(quest.clearValue.values[i].boolValue)
                {
                    Debug.Log("퀘스트 클리어");
                    check = true;
                }
            }
        }
        return check;
    }

    public void SetQuestEmptyValue(Quest quest)
    {
        for(int i = 0; i < quest.clearValue.values.Count; i++)
        {
            if(quest.clearValue.values[i].type == PropertyType.INT)
            {
                quest.clearValue.values[i].currentIntValue = 0; //테스트용 초기화
                quest.clear = false; //테스트용 초기화
                Debug.Log(quest.clearValue.values[i].intValue);
                Debug.Log(quest.clearValue.values[i].currentIntValue);
                Debug.Log(quest.clear);
            }
            else if(quest.clearValue.values[i].type == PropertyType.BOOL)
            {
                quest.clearValue.values[i].boolValue = false;
                quest.clear = false; 
                Debug.Log(quest.clearValue.values[i].boolValue);
            }
        }
    }
}
