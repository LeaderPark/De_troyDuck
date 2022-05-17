using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineSpeedBehaivor : PlayableBehaviour
{
	public float timeScale;
	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		Time.timeScale = timeScale;
	}
	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		Time.timeScale = 1;
	}
}
