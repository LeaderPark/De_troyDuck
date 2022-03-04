using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TestAsset : PlayableAsset
{
	public ExposedReference<GameObject> talker;
	public string dialogueMessage;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		var playable = ScriptPlayable<TestBehaivor>.Create(graph);
		var behaviour = playable.GetBehaviour();

		if (ManagerObject.Instance != null)
		{
			PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
			behaviour.talkObj = poolManager.GetUIObject("test");
		}
		behaviour.game = talker.Resolve(graph.GetResolver());
		behaviour.txt = dialogueMessage;

		return playable;
	}
}
