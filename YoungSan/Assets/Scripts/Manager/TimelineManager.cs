using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : Manager
{
	public GameObject directorObj;

	private void Awake()
	{

	}
	public void StartCutScene()
	{
		PlayableDirector director = directorObj.GetComponent<PlayableDirector>();
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
		director.Stop();
		director.playableAsset = cutScene;
		director.Play();
		//Debug.Log(director.playableGraph.GetRootPlayable(0).GetSpeed());
	}
}
