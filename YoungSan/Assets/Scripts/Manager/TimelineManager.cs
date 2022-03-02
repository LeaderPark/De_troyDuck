using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : Manager
{
	[SerializeField]
	private GameObject directorObj;
	private void Start()
	{
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
