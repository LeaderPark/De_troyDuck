using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class JumpReciver : MonoBehaviour, INotificationReceiver
{
	TimelineController timelineCon;
	public void OnNotify(Playable origin, INotification notification, object context)
	{
		JumpMarker _marker = notification as JumpMarker;
		if (_marker != null)
		{
			if (timelineCon == null)
			{
				timelineCon = GameObject.Find("CutScenePrefab").GetComponent<TimelineController>();
			}
			if (timelineCon.talkLoop)
			{
				timelineCon.jumpMarker = _marker;
				origin.GetGraph().GetRootPlayable(0).SetTime(_marker.test.time);
			}
			else
			{
				timelineCon.talkLoop = true;
			}
		}
	}
}
