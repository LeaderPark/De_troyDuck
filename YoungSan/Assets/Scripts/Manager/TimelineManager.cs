using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : Manager
{
	[SerializeField]
	private GameObject directorObj;
	
	void Start()
	{
		
	}

	public void StartCutScene(string cutSceneName)
	{
		directorObj = GameObject.Find("CutScenePrefab").gameObject;
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
		directorObj = GameObject.Find("CutScenePrefab").gameObject;
		PlayableAsset cutScene = cutSceneName;
		PlayableDirector director = directorObj.GetComponent<PlayableDirector>();
		director.Stop();
		director.playableAsset = cutScene;
		//Debug.Log(director.playableAsset);
		director.Play();
		//Debug.Log(director.playableGraph.GetRootPlayable(0).GetSpeed());
	}
}
