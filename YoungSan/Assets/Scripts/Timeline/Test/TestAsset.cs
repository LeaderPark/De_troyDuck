using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEditor;
using UnityEngine.Timeline;

public class TestAsset : PlayableAsset
{
	public ExposedReference<GameObject> talker;
	public string dialogueMessage;

	public AnimationCurve delayCurve;
	public float activeTime;

	public ScriptPlayable<TestBehaivor> playable;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		playable = ScriptPlayable<TestBehaivor>.Create(graph);
		var behaviour = playable.GetBehaviour();

		if (Application.isPlaying)
		{
			//if (ManagerObject.Instance != null)
			//{
			//	PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
			//	behaviour.talkObj = poolManager.GetUIObject("TalkBox");
				
			//}
		}
		else
		{
			//GameObject talkObj = Instantiate(Resources.Load<GameObject>("PoolObject/TalkBox"));
			//behaviour.talkObj = talkObj;
			//behaviour.talkObj.transform.parent = GameObject.Find("TestCanvas").transform;
		}
		//behaviour.talkObj.SetActive(false);
		behaviour.delayCurve = delayCurve;
		behaviour.talker = talker.Resolve(graph.GetResolver());
		behaviour.txt = dialogueMessage;

		return playable;
	}
}
#if UNITY_EDITOR
[CustomEditor(typeof(TestAsset))]
public class TestAssetEditor : Editor
{

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (GUILayout.Button("Set Clip Size"))
		{
			TestAsset ta = (TestAsset)serializedObject.targetObject;
			if (ta.playable.IsNull()) return;
			for (int i = 0; i < ta.playable.GetGraph().GetOutputCount(); i++)
			{
				TrackAsset asset = ta.playable.GetGraph().GetOutput(i).GetReferenceObject() as TrackAsset;
				if (asset == null) continue;
				foreach (var clip in asset.GetClips())
				{
					if (clip.asset == ta)
					{
						double dur = 0;
						for (int j = 0; j < ta.dialogueMessage.Length; j++)
						{
							dur += ta.delayCurve.Evaluate(j * 0.1f);
						}
						clip.duration = dur+ ta.activeTime;
					}
				}
			}
		}
		if (GUILayout.Button("Set Default Curve"))
		{
			TestAsset ta = (TestAsset)serializedObject.targetObject;
			Keyframe[] keyframes = new Keyframe[ta.dialogueMessage.Length];
			for (int i = 0; i < ta.dialogueMessage.Length; i++)
			{
				keyframes[i].time = 0.05f * i;
				keyframes[i].value = 0.05f;
			}
			ta.delayCurve.keys = keyframes;
			for (int i = 0; i < ta.dialogueMessage.Length; i++)
			{
				AnimationUtility.SetKeyLeftTangentMode(ta.delayCurve, i, AnimationUtility.TangentMode.Linear);
				AnimationUtility.SetKeyRightTangentMode(ta.delayCurve, i, AnimationUtility.TangentMode.Linear);
			}
		}
	}
}
#endif