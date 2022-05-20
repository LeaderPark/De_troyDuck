using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EventReciver : Receiver
{

	public override void OnNotify(Playable origin, INotification notification, object context)
	{
		base.OnNotify(origin, notification, context);
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
