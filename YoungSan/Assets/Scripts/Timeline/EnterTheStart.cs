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
            //triggerQuest�� ���ų� triggerQuest�� ���� �������� ����Ʈ�ϰ�� ����
            //���� ���������� ������ �������� �ʴ´�
            if (triggerQuest == null || questManger.IsProceeding(triggerQuest))
            {
                //isClearTriggerQuest�� ���ų� isClearTriggerQuest�� ���� �ʾҴٸ� ����
                //������ �������� �ʴ´�
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
