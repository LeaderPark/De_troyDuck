using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTheStart : MonoBehaviour
{
    public string timeLineName;
    private void OnTriggerEnter(Collider col) 
    {
        if(col.CompareTag("Player"))
        {
            TimelineManager timelineManager = ManagerObject.Instance.GetManager(ManagerType.TimelineManager) as TimelineManager;
            timelineManager.StartCutScene(timeLineName);
        }
    }
}
