using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

public class MoveAsset : PlayableAsset
{
	public ScriptPlayable<MoveBehavior> playable;
	public ExposedReference<Transform> movePos;
	public ExposedReference<GameObject> moveObj;
	public bool mainChar;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		

		playable = ScriptPlayable<MoveBehavior>.Create(graph);
		var behaviour = playable.GetBehaviour();

		behaviour.movePos = movePos.Resolve(graph.GetResolver()).position;
		if (!mainChar)
		{
			behaviour.moveObj = moveObj.Resolve(graph.GetResolver());
			Debug.Log(behaviour);
		}
		else
		{
			if (Application.isPlaying)
			{
				GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
				behaviour.moveObj = gameManager.Player.gameObject;
			}

		}
		return playable;
	}
#if UNITY_EDITOR
#endif
}
