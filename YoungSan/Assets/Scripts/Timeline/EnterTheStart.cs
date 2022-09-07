using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class EnterTheStart : MonoBehaviour
{
    public TimelineAsset timeLineName;
    public GameObject[] objs;
    [SerializeField] private Quest triggerQuest;
    [SerializeField] private Quest isClearTriggerQuest;
    [SerializeField] private bool loop = false;

    private void OnTriggerEnter(Collider col)
    {
        QuestManager questManager = ManagerObject.Instance.GetManager(ManagerType.QuestManager) as QuestManager;
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        if (col.CompareTag("Player"))
        {
            if (triggerQuest == null || questManager.IsProceeding(triggerQuest.questId))
            {
                if (isClearTriggerQuest == null || !questManager.IsComplete(isClearTriggerQuest.questId))
                {
                    if (timeLineName != null)
                    {

                        TimelineManager timelineManager = ManagerObject.Instance.GetManager(ManagerType.TimelineManager) as TimelineManager;
                        timelineManager.StartCutScene(timeLineName);
                    }
                    for (int i = 0; i < objs.Length; i++)
                    {
                        objs[i].SetActive(true);
                    }
                    if(!loop)
                    gameObject.SetActive(false);
                    //uiManager.FadeInOut(false, true);
                    // });
                }
            }
        }
    }
}
