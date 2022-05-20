using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SceneLoadReciver : Reciver
{
	public override void OnNotify(Playable origin, INotification notification, object context)
	{
		base.OnNotify(origin, notification, context);

		SceneLoadMarker marker = notification as SceneLoadMarker;
		if (marker != null)
		{
			SceneManager sceneManager = ManagerObject.Instance.GetManager(ManagerType.SceneManager) as SceneManager;
			if(marker.sceneName!="none")
			sceneManager.LoadScene(marker.sceneName);
		}
	}
}
