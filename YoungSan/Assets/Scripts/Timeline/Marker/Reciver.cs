using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class Reciver : MonoBehaviour, INotificationReceiver
{
	Dialogue dialogue;
	
	private void Start()
	{
		dialogue = GetComponent<Dialogue>();
	}
	public void OnNotify(Playable origin, INotification notification, object context)
	{
		DialogueMarker marker = notification as DialogueMarker;
		if (marker != null)
		{
			List<Dictionary<string, object>> dialogueList = CSVReader.Read(string.Format("Dialogue/{0}", marker.dialogueFileName));
			dialogue.StartTalk(dialogueList,marker.wait);
		}
	}
}
