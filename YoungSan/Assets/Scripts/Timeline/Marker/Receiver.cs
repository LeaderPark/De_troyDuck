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

		if (timelineCon.targetMarker != null)
		{
			return;
		}
	}
}
