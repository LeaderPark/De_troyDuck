using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class QuestReciver : MonoBehaviour, INotificationReceiver
{
	public void OnNotify(Playable origin, INotification notification, object context)
	{
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
