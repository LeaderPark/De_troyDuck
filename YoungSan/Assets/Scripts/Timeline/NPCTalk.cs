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
			//퀘스트 없을때
			if (npcData[i].quest == null)
			{
				timelineManager.StartCutScene(npcData[i].noneQusetTimeline);
				break;
			}
			//npc 데이터가 지니고 있는 퀘스트가 클리어 된 상태가 아니라면
			if (!npcData[i].quest.clear)
			{
				//이전 퀘스트가 없거나 깼다면
				if (npcData[i].quest.prevQuest == null || npcData[i].quest.prevQuest.clear)
				{
					if (questManager.CheckAvailableQuest(npcData[i].quest))
					{
						timelineManager.StartCutScene(npcData[i].timelineList[0]);
					}
					//i번째 퀘스트가 진행중에 들어가 있고 클리어에 들어있지 않다면
					else if (questManager.proceedingQuests.ContainsKey(npcData[i].quest.questId) && !questManager.CheckClearQuest(npcData[i].quest))
					{
						timelineManager.StartCutScene(npcData[i].timelineList[1]);
					}
					//퀘스트가 클리어라면
					else if (questManager.CheckClearQuest(npcData[i].quest))
					{
						questManager.ClearQuest(npcData[i].quest);
						timelineManager.StartCutScene(npcData[i].timelineList[2]);
					}
					break;
				}
				//이전 퀘스트를 덜 깼다면
				else
				{
					timelineManager.StartCutScene(npcData[i].noneQusetTimeline);
					break;
				}
			}
			//i번째 퀘스트가 클리어라면
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
