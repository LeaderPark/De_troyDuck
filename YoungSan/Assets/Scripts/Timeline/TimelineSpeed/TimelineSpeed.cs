using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineSpeed : PlayableAsset
{
	public float timeScale;

	public ScriptPlayable<TimelineSpeedBehaivor> playable;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		playable = ScriptPlayable<TimelineSpeedBehaivor>.Create(graph);
		playable.GetBehaviour().timeScale = timeScale;

		return playable;
	}
}
