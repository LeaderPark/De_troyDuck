using System.ComponentModel.Design.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTest : MonoBehaviour
{
    public Quest quest;
    public Quest currentQuest;
    void Start()
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
            else
            {
                Debug.Log(quest.clearValue.values[i].boolValue);
            }
        }

        if(quest.prevQuest.clear)
        {
            if(quest.clear)
            {
                Debug.Log("이미 진행한 퀘스트 입니다.");
                //currentQuest = quest.nextQuest;
            }
            else
            {
                Debug.Log(quest.title);
                Debug.Log(quest.context);
                Debug.Log(quest.prevQuest);
                Debug.Log(quest.nextQuest);
            }
        }
        else
        {
            Debug.Log("전 퀘스트 " + quest.prevQuest + "를 깨고 오세요");
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            for(int i = 0; i < quest.clearValue.values.Count; i++)
            {
                if(quest.clearValue.values[i].type == PropertyType.INT)
                {
                    if(quest.clearValue.values[i].intValue > quest.clearValue.values[i].currentIntValue)
                    {
                        quest.clearValue.values[i].currentIntValue++;
                        Debug.Log(quest.clearValue.values[i].currentIntValue);
                    }
                }
            }
        }
        else if(Input.GetKeyDown(KeyCode.R))
        {
            
        }

        for(int i = 0; i < quest.clearValue.values.Count; i++)
        {
            if(quest.clearValue.values[i].type == PropertyType.INT)
            {
                if(quest.clearValue.values[i].intValue <= quest.clearValue.values[i].currentIntValue)
                {
                    Debug.Log("퀘스트 클리어");
                    quest.clear = true;
                }
            }
        }
    }

}
