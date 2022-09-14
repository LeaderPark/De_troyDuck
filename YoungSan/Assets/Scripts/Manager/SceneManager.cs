using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : Manager
{
    public Action afterSceneLoadAction;

    Hashtable sceneStartPosition;

    private void Awake()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoad;
        sceneStartPosition = new Hashtable();
        LoadSceneStartPositionData();
    }

    private void LoadSceneStartPositionData()
    {
        SceneStartPosition[] prefabs = Resources.LoadAll<SceneStartPosition>("ScriptableObjects/SceneStartPosition");
        foreach (SceneStartPosition item in prefabs)
        {
            sceneStartPosition.Add((item.beginScene, item.endScene), item.position);
        }
    }

    public void LoadScene(string sceneName)
    {
        UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        string curSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        uiManager.FadeInOut(true, false, () =>
        {
            if (sceneStartPosition.Contains((curSceneName, sceneName)))
            {
                GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
                gameManager.Player.transform.position = (Vector3)sceneStartPosition[(curSceneName, sceneName)];
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        });

    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        //Debug.LogError(gameManager.Player);
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 0)
        {
            uiManager.FadeInOut(false, false);
            StartCoroutine(test1());
        }
    }
    private IEnumerator test1()
    {
        yield return null;
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        Debug.Log(gameManager.Player.name);
        gameManager.playerFollowCam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
        gameManager.playerFollowCam.Follow = gameManager.Player.transform;
        UIManager uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        TimelineManager timelineManager = ManagerObject.Instance.GetManager(ManagerType.TimelineManager) as TimelineManager;
        timelineManager.directorObj = GameObject.Find("CutScenePrefab").gameObject;
        TimelineController timelineCon = timelineManager.directorObj.GetComponent<TimelineController>();
        DataManager dataManager = ManagerObject.Instance.GetManager(ManagerType.DataManager) as DataManager;

        uIManager.UISetActiveTimeLine(true);
		if (timelineCon.onSceneLoadPlay && timelineCon.startTimeline != null)
		{
			timelineManager.StartCutScene(timelineCon.startTimeline);
		}
		afterSceneLoadAction?.Invoke();
    }
}
