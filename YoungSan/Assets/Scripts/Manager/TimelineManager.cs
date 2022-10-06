using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : Manager
{
	public GameObject directorObj;
	private GameManager gameManager;

	private void Awake()
	{
		gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
	}
	public void StartCutScene()
	{
		PlayableDirector director = directorObj.GetComponent<PlayableDirector>();
		StartCoroutine(CamSet(director.playableAsset));
		director.Play();

	}
	public void StartCutScene(string cutSceneName)
	{

		if (directorObj == null)
		{
			directorObj = GameObject.Find("CutScenePrefab").gameObject;
		}
		PlayableAsset cutScene = Resources.Load("Timeline/"+cutSceneName) as PlayableAsset;
		PlayableDirector director = directorObj.GetComponent<PlayableDirector>();
		StartCoroutine(CamSet(cutScene));

		director.Stop();
		director.playableAsset = cutScene;
		//Debug.Log(director.playableAsset);
		director.Play();
		//Debug.Log(director.playableGraph.GetRootPlayable(0).GetSpeed());

	}
	public void StartCutScene(PlayableAsset cutSceneName)
	{

		if (directorObj == null)
		{
			directorObj = GameObject.Find("CutScenePrefab").gameObject;
		}
		PlayableAsset cutScene = cutSceneName;
		PlayableDirector director = directorObj.GetComponent<PlayableDirector>();
		StartCoroutine(CamSet(cutScene));

		director.Stop();
		director.playableAsset = cutScene;
		director.Play();
		//Debug.Log(director.playableGraph.GetRootPlayable(0).GetSpeed());
	}
	IEnumerator CamSet(PlayableAsset cutScene)
	{
		if (cutScene.duration < 2) yield break;
		yield return null;

		CinemachineFramingTransposer body = ((CinemachineVirtualCamera)Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera).GetCinemachineComponent<CinemachineFramingTransposer>();
		if (body != null)
		{
			body.m_XDamping = 0;
			body.m_YDamping = 0;
			yield return null;
			body.m_XDamping = 1;
			body.m_YDamping = 1;
		}
	}
}
