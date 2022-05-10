using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EventReciver : MonoBehaviour, INotificationReceiver
{
	TimelineController timelineCon;
	private void Awake()
	{
		timelineCon = GetComponent<TimelineController>();
	}
	public void OnNotify(Playable origin, INotification notification, object context)
	{
		EventMarker marker = notification as EventMarker;
		if (marker != null)
		{
			for (int i = 0; i < timelineCon.timelineEvents.Count; i++)
			{
				for (int j = 0; j < marker.evnetName.Count; j++)
				{
					if (marker.evnetName[j] == timelineCon.timelineEvents[i].evnetName)
					{
						timelineCon.timelineEvents[i].events?.Invoke();
					}
				}
			}

		}
	}
}
