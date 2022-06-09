using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Receiver : MonoBehaviour, INotificationReceiver
{
	protected TimelineController timelineCon;

	protected virtual void Awake()
	{
		timelineCon = GameObject.Find("CutScenePrefab").GetComponent<TimelineController>();
	}
	public virtual void OnNotify(Playable origin, INotification notification, object context)
	{
		JumpMarker jumpMarker = notification as JumpMarker;
		//지금 실행하는 마커가 점프마커이고 스킵이 눌린 상태면 넘어간다
		if (jumpMarker!=null&&timelineCon.skip)
		{
			return;
		}
		if (timelineCon.targetMarker != null)
		{
			return;
		}
	}
}
