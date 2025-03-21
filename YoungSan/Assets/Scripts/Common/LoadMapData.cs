using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMapData : MonoBehaviour
{
    public Quest quest;
    public bool active;
    public List<GameObject> activeObj;

    private void Awake()
    {
        if (quest == null) return;
        SceneManager sceneManager = ManagerObject.Instance.GetManager(ManagerType.SceneManager) as SceneManager;
        QuestManager questManager = ManagerObject.Instance.GetManager(ManagerType.QuestManager) as QuestManager;

        if (questManager.IsComplete(quest.questId))
        {
            gameObject.SetActive(active);
			foreach (var item in activeObj)
			{
                item.SetActive(active);
			}
        }
        //Action action = null;
        //action += () => gameObject.SetActive(active);
        //Action removeAction = null;
        //removeAction += () => { 
        //    sceneManager.afterSceneLoadAction -= action;
        //    sceneManager.afterSceneLoadAction -= removeAction;
        //};
        //if (quest.clear)
        //{

        //    sceneManager.afterSceneLoadAction += action;
        //    sceneManager.afterSceneLoadAction += removeAction;
        //}
    }
}
