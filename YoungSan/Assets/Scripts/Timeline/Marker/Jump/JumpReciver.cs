using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class JumpReciver : MonoBehaviour, INotificationReceiver
{
	TimelineController timelineCon;
	public bool qeustSelect = false;
	private bool loop = false;
	public void OnNotify(Playable origin, INotification notification, object context)
	{
		JumpMarker _marker = notification as JumpMarker;
		if (_marker != null)
		{
			qeustSelect = _marker.qeustSelect;

			if (qeustSelect && !loop)
			{
				loop = true;
				Select(_marker);
			}
			if (timelineCon == null)
			{
				timelineCon = GameObject.Find("CutScenePrefab").GetComponent<TimelineController>();
			}
			if (timelineCon.talkLoop)
			{

				timelineCon.jumpMarker = _marker;
				origin.GetGraph().GetRootPlayable(0).SetTime(_marker.loopMarker.time);

			}
			else
			{
				loop = false;
				timelineCon.talkLoop = true;
			}

		}
	}
	private void Select(JumpMarker marker)
	{
		PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
		QuestManager questManager = ManagerObject.Instance.GetManager(ManagerType.QuestManager) as QuestManager;
		
		SelectButton button = poolManager.GetUIObject("Select").GetComponent<SelectButton>();
		button.gameObject.SetActive(true);
		button.ButtonsSetting(0, "수락", () => { questManager.AddQuest(marker.quest); 
			questManager.AddQuestValue(marker);
		});
		button.ButtonsSetting(1, "거절", () => { Debug.Log("거절한다."); });
	}
}
