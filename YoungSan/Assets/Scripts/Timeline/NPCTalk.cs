using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class NPCTalk : MonoBehaviour
{
    public TimelineAsset timelineName;
	public void Talk()
	{
		TimelineManager timelineManager = ManagerObject.Instance.GetManager(ManagerType.TimelineManager) as TimelineManager;
		timelineManager.StartCutScene(timelineName);
	}
}
