using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MoveBehavior : PlayableBehaviour
{
	public Vector3 movePos;
	public Vector3 startPos;
	public GameObject moveObj;
	private float time = 0;
	GameManager gameManager;
	public override void OnGraphStart(Playable playable)
	{
		base.OnGraphStart(playable);
		Debug.Log("A");
		startPos = moveObj.transform.position;
		//gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;

	}
	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		base.OnBehaviourPlay(playable, info);
		Debug.Log(moveObj);
	}
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		base.ProcessFrame(playable, info, playerData);
		time += Time.deltaTime;
		moveObj.transform.position = Vector3.Lerp(startPos, movePos, time / (float)playable.GetDuration());
	}
	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		if (!Application.isPlaying)
		{
			moveObj.transform.position = startPos;

		}
		base.OnBehaviourPause(playable, info);
		time = 0;
	}
}
