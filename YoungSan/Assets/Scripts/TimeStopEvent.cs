using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopEvent : MonoBehaviour
{
    public float waitTime;
    public EntityData[] entityData;
    public KeyCode[] inputKey;
    public bool mouseRight;
    public GameObject tutorialUi;

    private void Start()
    {
        if (!gameObject.activeSelf) return;
        if (entityData.Length == 0) return;
        EventManager eventManager = ManagerObject.Instance.GetManager(ManagerType.EventManager) as EventManager;
        GlobalEventTrigger.SkillEvent attackAction = null;
        attackAction = (entity, skillData) =>
         {
             foreach (EntityData item in entityData)
             {
                 if (skillData.skillSet.entity.entityData == item)
                 {
                     StopEvent();
                     eventManager.GetEventTrigger(typeof(SkillEventTrigger)).Remove(attackAction);
                     break;
                 }
             }
         };
        eventManager.GetEventTrigger(typeof(SkillEventTrigger)).Add(attackAction);
    }
    public void StopEvent()
    {
        StartCoroutine(TimeStopEndCheck());
    }
    IEnumerator TimeStopEndCheck()
    {
        yield return new WaitForSeconds(waitTime);
        tutorialUi.SetActive(true);
        Time.timeScale = 0;
        while (true)
        {
            if (inputKey.Length != 0)
            {
                foreach (KeyCode item in inputKey)
                {
                    if (Input.GetKey(item))
                    {
                        if (mouseRight == false || Input.GetMouseButton(1))
                        {
                            tutorialUi.SetActive(false);
                            Time.timeScale = 1;
                            yield break;
                        }
                    }
                }
            }
            else
            {
                if (Input.anyKey)
                {
                    tutorialUi.SetActive(false);
                    Time.timeScale = 1;
                    yield break;
                }
            }

            yield return null;
        }
    }

}
