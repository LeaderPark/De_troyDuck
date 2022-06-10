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
    private QuestManager questManger;
	private void Awake()
	{
        questManger = ManagerObject.Instance.GetManager(ManagerType.QuestManager) as QuestManager;
	}
	private void OnTriggerEnter(Collider col) 
    {
        if(col.CompareTag("Player"))
        {
            //triggerQuest가 없거나 triggerQuest가 현재 진행중인 퀘스트일경우 실행
            //현재 진행중이지 않으면 실행하지 않는다
            if (triggerQuest == null || questManger.IsProceeding(triggerQuest))
            {
                //isClearTriggerQuest가 없거나 isClearTriggerQuest를 깨지 않았다면 실행
                //깼으면 실행하지 않는다
                //if (isClearTriggerQuest == null || !isClearTriggerQuest.clear)
                //{
                    if (timeLineName != null)
                    {
                        TimelineManager timelineManager = ManagerObject.Instance.GetManager(ManagerType.TimelineManager) as TimelineManager;
                        timelineManager.StartCutScene(timeLineName);
                    }
                    for (int i = 0; i < objs.Length; i++)
                    {
                        objs[i].SetActive(true);
                    }
                    gameObject.SetActive(false);
               // }
            }
        }
    }
}
