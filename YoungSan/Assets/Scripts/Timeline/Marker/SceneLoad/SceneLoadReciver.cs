using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SceneLoadReciver : MonoBehaviour, INotificationReceiver
{
	public void OnNotify(Playable origin, INotification notification, object context)
	{
		SceneLoadMarker marker = notification as SceneLoadMarker;
		if (marker != null)
		{
			SceneManager sceneManager = ManagerObject.Instance.GetManager(ManagerType.SceneManager) as SceneManager;
			if(marker.sceneName!="none")
			sceneManager.LoadScene(marker.sceneName);
		}
	}
}
