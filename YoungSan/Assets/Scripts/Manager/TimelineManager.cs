using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : Manager
{
	[SerializeField]
	private GameObject directorObj;
	private void Start()
	{
		//StartCutScene("Prologue");
	}
	public void StartCutScene(string cutSceneName)
	{
		directorObj = transform.Find("CutScenePrefab").gameObject;
		PlayableAsset cutScene = Resources.Load("Timeline/"+cutSceneName) as PlayableAsset;
		PlayableDirector director = directorObj.GetComponent<PlayableDirector>();
		director.playableAsset = cutScene;
		director.Play();
	}
}