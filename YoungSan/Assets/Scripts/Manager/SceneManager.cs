using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : Manager
{
	public string testName;
	private void Awake()
	{
		UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoad;
	}
	public void LoadScene(string sceneName)
	{
		UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
		uiManager.FadeInOut(true, () => { UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName); });
	}
	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Y))
		LoadScene(testName);	
	}
	private void OnSceneLoad(Scene scene, LoadSceneMode mode)
	{
		UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
		uiManager.FadeInOut(false);
		//Debug.LogError(gameManager.Player);
		StartCoroutine(test1());
	}
	private IEnumerator test1()
	{
		yield return null;
		GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
		Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Follow = gameManager.Player.transform;

	}
}