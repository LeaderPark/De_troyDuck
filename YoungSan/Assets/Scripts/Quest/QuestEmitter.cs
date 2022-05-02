using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEmitter : MonoBehaviour
{
    public Quest[] quests;

    void Test()
    {
        QuestManager questManager = ManagerObject.Instance.GetManager(ManagerType.QuestManager) as QuestManager;
        questManager.AddQuest(quests[1]);
    }
}
