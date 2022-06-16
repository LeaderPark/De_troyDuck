using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMapData : MonoBehaviour
{
    public Quest quest;
    public bool active;

    private void Awake()
    {
        if (quest == null) return;
        SceneManager sceneManager = ManagerObject.Instance.GetManager(ManagerType.SceneManager) as SceneManager;

        if (quest.clear)
        {
            gameObject.SetActive(active);
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
