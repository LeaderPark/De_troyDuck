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
				//퀘스트인지 체크해놓으면 
				loop = true;
				Select();
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
	private void Select()
	{
		PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
		SelectButton button = poolManager.GetUIObject("Select").GetComponent<SelectButton>();
		button.gameObject.SetActive(true);
		button.ButtonsSetting(0, "수락", () => { Debug.Log("수락"); });
		button.ButtonsSetting(1, "거절", () => { Debug.Log("거절"); });
	}
}
