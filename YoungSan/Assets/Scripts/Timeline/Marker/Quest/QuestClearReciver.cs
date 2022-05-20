using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class QuestClearReciver : Reciver
{
	public override void OnNotify(Playable origin, INotification notification, object context)
	{
		base.OnNotify(origin, notification, context);
		QuestClearMarker marker = notification as QuestClearMarker;
		if (marker != null)
		{
			QuestManager questManager = ManagerObject.Instance.GetManager(ManagerType.QuestManager) as QuestManager;
            if(marker.quest != null)
            {
                if(questManager.CheckClearQuest(marker.quest))
                    questManager.ClearQuest(marker.quest);
            }
		}
	}
}
