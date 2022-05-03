using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class NPCTalk : MonoBehaviour
{
	public NpcData npcData;
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
		questManager.SetQuestEmptyValue(npcData.quest);
	}
	public void Talk()
	{
		if (npcData.quest == null)
		{
			timelineManager.StartCutScene(npcData.noneQusetTimeline);
		}
		else if (questManager.CheckAvailableQuest(npcData.quest))
		{
			timelineManager.StartCutScene(npcData.timelineList[0]);
		}
		else if (questManager.proceedingQuests.ContainsKey(npcData.quest.questId))
		{
			timelineManager.StartCutScene(npcData.timelineList[1]);
		}
		else if (questManager.CheckClearQuest(npcData.quest))
		{
			questManager.ClearQuest(npcData.quest);
			timelineManager.StartCutScene(npcData.timelineList[2]);
		}
	}
}
