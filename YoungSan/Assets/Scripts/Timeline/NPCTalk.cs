using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class NPCTalk : MonoBehaviour
{
	public NpcData[] npcData;
	private QuestManager questManager;
	private TimelineManager timelineManager;
	private void Awake()
	{
		questManager = ManagerObject.Instance.GetManager(ManagerType.QuestManager) as QuestManager;
		timelineManager = ManagerObject.Instance.GetManager(ManagerType.TimelineManager) as TimelineManager;
	}
	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.H))
			for (int i = 0; i < npcData.Length; i++)
			{
				questManager.SetQuestEmptyValue(npcData[i].quest);
			}
	}
	public void Talk()
	{
		for (int i = 0; i < npcData.Length; i++)
		{
			//����Ʈ ������
			if (npcData[i].quest == null)
			{
				timelineManager.StartCutScene(npcData[i].noneQusetTimeline);
				break;
			}
			//npc �����Ͱ� ���ϰ� �ִ� ����Ʈ�� Ŭ���� �� ���°� �ƴ϶��
			if (!npcData[i].quest.clear)
			{
				//���� ����Ʈ�� ���ų� ���ٸ�
				if (npcData[i].quest.prevQuest == null || npcData[i].quest.prevQuest.clear)
				{
					if (questManager.CheckAvailableQuest(npcData[i].quest))
					{
						timelineManager.StartCutScene(npcData[i].timelineList[0]);
					}
					//i��° ����Ʈ�� �����߿� �� �ְ� Ŭ��� ������� �ʴٸ�
					else if (questManager.proceedingQuests.ContainsKey(npcData[i].quest.questId) && !questManager.CheckClearQuest(npcData[i].quest))
					{
						timelineManager.StartCutScene(npcData[i].timelineList[1]);
					}
					//����Ʈ�� Ŭ������
					else if (questManager.CheckClearQuest(npcData[i].quest))
					{
						questManager.ClearQuest(npcData[i].quest);
						timelineManager.StartCutScene(npcData[i].timelineList[2]);
					}
					break;
				}
				//���� ����Ʈ�� �� ���ٸ�
				else
				{
					timelineManager.StartCutScene(npcData[i].noneQusetTimeline);
					break;
				}
			}
			//i��° ����Ʈ�� Ŭ������
			else
			{
				if (i + 1 <= npcData.Length)
				{
					continue;
				}
				timelineManager.StartCutScene(npcData[i].noneQusetTimeline);
				break;
			}
		}
	}
}
