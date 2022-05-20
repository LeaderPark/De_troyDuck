using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class JumpReciver : Reciver
{
	public bool qeustSelect = false;
	private bool loop = false;

	public override void OnNotify(Playable origin, INotification notification, object context)
	{
		JumpMarker _marker = notification as JumpMarker;

		if (_marker != null&& timelineCon.targetMarker==null)
		{
			timelineCon.targetMarker = _marker.loopEndMarker;
		}

		if (timelineCon.targetMarker != null)
		{
			Marker marker = notification as Marker;
			if (timelineCon.targetMarker != marker&&!timelineCon.talkLoop)
			{
				return;
			}
			else if (timelineCon.targetMarker == marker)
			{
				timelineCon.targetMarker = null;
				timelineCon.talkLoop = true;
			}
		}

		if (_marker != null)
		{
			qeustSelect = _marker.qeustSelect;
			timelineCon.jumpMarker = _marker;
			Debug.Log(loop);
			if (_marker.qeustSelect && !loop)
			{
				Select(_marker, origin);
				loop = true;
				origin.GetGraph().GetRootPlayable(0).SetTime(_marker.loopMarker.time);
			}

			if (timelineCon.talkLoop)
			{
				origin.GetGraph().GetRootPlayable(0).SetTime(_marker.loopMarker.time);
			}
			if (!timelineCon.talkLoop)
			{
				origin.GetGraph().GetRootPlayable(0).SetTime(_marker.loopEndMarker.time);
			}
		}
	}
	private void Select(JumpMarker marker, Playable origin)
	{
		Debug.Log("아니 싯팔 퀘스트가 왔다니까");
		PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
		QuestManager questManager = ManagerObject.Instance.GetManager(ManagerType.QuestManager) as QuestManager;

		SelectButton button = poolManager.GetUIObject("Select").GetComponent<SelectButton>();
		button.gameObject.SetActive(true);
		button.ButtonsSetting(0, "수락", () => {

			questManager.AddQuest(marker.quest);
			questManager.AddQuestValue(marker.quest);
			timelineCon.targetMarker = marker.loopEndMarker;
			origin.GetGraph().GetRootPlayable(0).SetTime(marker.loopEndMarker.time);
			timelineCon.talkLoop = false;
			loop = false;
		});
		button.ButtonsSetting(1, "거절", () => {
			Debug.Log("거절한다.");
			timelineCon.targetMarker = marker.questRefuse;
			origin.GetGraph().GetRootPlayable(0).SetTime(marker.questRefuse.time);
			timelineCon.talkLoop = false;
			loop = false;

		});
	}


}
