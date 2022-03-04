using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TestBehaivor : PlayableBehaviour
{
	public GameObject game;
	public string txt;
	[HideInInspector]
	public GameObject talkObj;

	private Text talkBox;
	private PoolManager poolManager;
	private float time = 0;
	private int idx = 0;

	public override void OnGraphStart(Playable playable)
	{
		if (ManagerObject.Instance != null)
		{
			poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
			talkBox = talkObj.GetComponent<Text>();
			talkBox.text = "";
		}
		Debug.Log("AAA");
	}
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		time += Time.deltaTime;
		if (time >= 0.1f)
		{
			if (idx <= txt.Length - 1)
			{
				//GameObject gma = poolManager.GetUIObject("test");
				talkBox.text += txt[idx];
				Debug.Log(txt[idx]);
				idx++;
			}
			idx = Mathf.Clamp(idx, 0, txt.Length);
			time = 0;
		}


		//Debug.Log(playable.GetGraph().GetRootPlayable(0).GetDuration());
		//Debug.Log(game + " : " + txt);
	}
}
