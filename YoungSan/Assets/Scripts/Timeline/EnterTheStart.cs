using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class EnterTheStart : MonoBehaviour
{
    public TimelineAsset timeLineName;
    private void OnTriggerEnter(Collider col) 
    {
        if(col.CompareTag("Player"))
        {
            TimelineManager timelineManager = ManagerObject.Instance.GetManager(ManagerType.TimelineManager) as TimelineManager;
            timelineManager.StartCutScene(timeLineName.name);
        }
    }
}
