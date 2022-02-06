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
			for (int i = 0; i < marker.animationDatas.Length; i++)
			{
				GameObject obj = marker.animationDatas[i].contorolObject.Resolve(origin.GetGraph().GetResolver());
				AnimationClip clip = marker.animationDatas[i].animation.Resolve(origin.GetGraph().GetResolver());

				Animator objAnimator = obj.GetComponent<Animator>();
				objAnimator.Play(clip.name);
			}
			
			//GameObject obj = marker.contorolObject.Resolve(origin.GetGraph().GetResolver());
		}
	}
}
