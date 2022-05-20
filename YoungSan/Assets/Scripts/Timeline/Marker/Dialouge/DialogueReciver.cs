using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class DialogueReciver : Reciver
{
	Dialogue dialogue;

	private void Start()
	{
		dialogue = GetComponent<Dialogue>();
	}

	public override void OnNotify(Playable origin, INotification notification, object context)
	{
		DialogueMarker marker = notification as DialogueMarker;
		if (marker != null)
		{
			List<Dictionary<string, object>> dialogueList = CSVReader.Read(string.Format("Dialogue/{0}", marker.dialogueFileName));
			dialogue.StartTalk(dialogueList,marker.wait);
		}
	}
}
