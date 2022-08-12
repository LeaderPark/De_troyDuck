using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class OtherPlaceCutSceneReceiver : Receiver
{
	public override void OnNotify(Playable origin, INotification notification, object context)
	{
		base.OnNotify(origin, notification, context);
		OtherPlaceCutSceneMarker marker = notification as OtherPlaceCutSceneMarker;
		if (marker != null)
		{
			 Instantiate(marker.nextCutScenePrefab);
		}
	}
}
