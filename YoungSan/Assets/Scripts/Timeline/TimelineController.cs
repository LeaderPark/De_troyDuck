using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class TimelineController : MonoBehaviour
{
	PlayableDirector director;

	public Entity entity;

	private void Awake()
	{
		director = GetComponent<PlayableDirector>();
		TextAsset fileData = Resources.Load("TestDialogue") as TextAsset;
	}
	public void StartTimeline()
	{
		director.playableGraph.GetRootPlayable(0).SetSpeed(1);
		print("Ω√¿€");

	}
	public void PauseTimeline()
	{
		director.playableGraph.GetRootPlayable(0).SetSpeed(0);
		print("∏ÿ√„");
	}
	public void playAnimation(AnimationClip obj)
	{
		print(obj.name);
		entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[] {obj.name});
	}
}
