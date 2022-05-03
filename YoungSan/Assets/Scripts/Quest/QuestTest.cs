using System.ComponentModel.Design.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTest : MonoBehaviour
{
    public Quest quest;
    void Start()
    {
        

    }

    void Update()
    {
        QuestManager questManager = ManagerObject.Instance.GetManager(ManagerType.QuestManager) as QuestManager;
        quest = questManager.proceedingQuests[2] as Quest;
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
    }

}
