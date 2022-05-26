using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class QuestReciver : Receiver
{
	public override void OnNotify(Playable origin, INotification notification, object context)
	{
		base.OnNotify(origin, notification, context);

		QuestMarker marker = notification as QuestMarker;
		if (marker != null)
		{
			QuestManager questManager = ManagerObject.Instance.GetManager(ManagerType.QuestManager) as QuestManager;
            if(marker.quest != null)
            {
				questManager.AddQuest(marker.quest);
				questManager.AddQuestValue(marker.quest);
				
			}
		}
	}
}
