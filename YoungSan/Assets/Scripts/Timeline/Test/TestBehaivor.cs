using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TestBehaivor : PlayableBehaviour
{
	public GameObject talker;
	public string txt;
	public GameObject talkObj;
	public AnimationCurve delayCurve;

	private Text talkBox;
	private Text fakeTalkbox;
	private PoolManager poolManager;
	private float time = 0;
	private int idx = 0;

	public override void OnGraphStart(Playable playable)
	{
		if (Application.isPlaying)
		{
			if (ManagerObject.Instance != null)
			{
				poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
			}
		}
		Debug.Log("AAA");
	}
	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		if (Application.isPlaying)
		{
			talkObj = poolManager.GetUIObject("TalkBox");
		}
		else
		{
			List<GameObject> pool = new List<GameObject>();
			GameObject canvas = GameObject.Find("TestCanvas");

			time = 0;
			idx = 0;
			for (int i = 0; i < canvas.transform.childCount; i++)
			{
				pool.Add(canvas.transform.GetChild(i).gameObject);
			}
			foreach (var item in pool)
			{
				if (!item.activeSelf)
				{
					item.SetActive(true);
					talkObj = item;
					break;
				}
			}
			if (talkObj == null)
			{
				talkObj = GameObject.Instantiate(Resources.Load<GameObject>("PoolObject/TalkBox"),canvas.transform);
			}
			
		}
		talkBox = talkObj.transform.Find("Text").GetComponent<Text>();
		fakeTalkbox = talkObj.transform.Find("fakeText").GetComponent<Text>();
		talkBox.text = "";
		fakeTalkbox.text = txt;
		Debug.Log("Play");
		talkObj.SetActive(true);
		SetBoxSize();

	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		if (Application.isPlaying)
		{
			if (talkObj != null)
				talkObj.SetActive(false);
		}
		else
		{
			if(talkObj!=null)
			talkObj.SetActive(false);
		}
	}
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		if (Application.isPlaying)
		{
			talkObj.transform.position = Camera.main.WorldToScreenPoint(talker.transform.position);
			time += Time.deltaTime;
			for (bool b = true; b;)
			{
				b = false;
				if (time >= delayCurve.Evaluate(idx * 0.1f))
				{
					b = true;
					time -= delayCurve.Evaluate(idx * 0.1f);
					if (idx < txt.Length)
					{
						talkBox.text += txt[idx];
						idx = Mathf.Clamp(idx + 1, 0, txt.Length);
					}
				}
			}
		}
		else
		{
			talkObj.transform.position = Camera.main.WorldToScreenPoint(talker.transform.position);
			time += Time.deltaTime;
			for (bool b = true; b;)
			{
				b = false;
				if (time >= delayCurve.Evaluate(idx * 0.1f))
				{
					b = true;
					time -= delayCurve.Evaluate(idx * 0.1f);
					if (idx < txt.Length)
					{
						talkBox.text += txt[idx];
						idx = Mathf.Clamp(idx + 1, 0, txt.Length);
					}
				}
			}
		}
	}
	public void SetBoxSize()
	{
		talkBox.rectTransform.anchoredPosition = new Vector2(0, 0);
		float x = fakeTalkbox.preferredWidth;
		float y = fakeTalkbox.preferredHeight;
		RectTransform talkBoxRect = talkObj.GetComponent<RectTransform>();
		talkBoxRect.sizeDelta = new Vector2(x, y) + new Vector2(30,30);
		talkBox.rectTransform.anchoredPosition += new Vector2(30/2 ,0);
	}
}
