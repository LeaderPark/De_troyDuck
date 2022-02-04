using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Playables;
public class ObjectControlReciver : MonoBehaviour, INotificationReceiver
{
	Dialogue dialogue;

	private void Start()
	{
		dialogue = GetComponent<Dialogue>();
	}

	public void OnNotify(Playable origin, INotification notification, object context)
	{
		ObjectControlMarker marker = notification as ObjectControlMarker;
		if (marker != null)
		{
			GameObject gameObject1 = marker.contorolObject.Resolve(origin.GetGraph().GetResolver());
			gameObject1.SetActive(false);
		}
	}
}
