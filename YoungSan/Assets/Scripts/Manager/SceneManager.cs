using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : Manager
{
    private void Awake()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoad;
    }
    public void LoadScene(string sceneName)
    {
        UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        uiManager.FadeInOut(true, false, () => { UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName); });
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
        Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Follow = gameManager.Player.transform;
        UIManager uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;

        uIManager.UISetActiveTimeLine(true);
        TimelineManager timelineManager = ManagerObject.Instance.GetManager(ManagerType.TimelineManager) as TimelineManager;
        timelineManager.directorObj = GameObject.Find("CutScenePrefab").gameObject;
        if (timelineManager.directorObj.GetComponent<TimelineController>().onSceneLoadPlay)
        {
            timelineManager.StartCutScene();
        }
    }
}
