using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTheStart : MonoBehaviour
{
    private void OnTriggerEnter(Collider col) 
    {
        if(col.CompareTag("Player"))
        {
            TimelineManager timelineManager = ManagerObject.Instance.GetManager(ManagerType.TimelineManager) as TimelineManager;
            Debug.Log("ENTER");
            timelineManager.StartCutScene("Tutorial");
        }
    }
}
