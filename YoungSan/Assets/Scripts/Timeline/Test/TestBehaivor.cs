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
	private GameObject endImage;
	private PoolManager poolManager;
	private float time = 0;
	private int idx = 0;
	private EntityData entityData;
	private TimelineController timelineCon;

	//타임라인 시작하면 실행
	public override void OnGraphStart(Playable playable)
	{
		if (Application.isPlaying)
		{
			if (ManagerObject.Instance != null)
			{
				poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
			}
		}
		//Debug.Log("AAA");
	}
	//플레이어블 트랙이 실행될때 한번 실행
	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		if (Application.isPlaying)
		{
			talkObj = poolManager.GetUIObject("TalkBox");
		}
		else
		{
			List<GameObject> pool = new List<GameObject>();
			GameObject canvas = GameObject.Find("CutSceneCanvas");

			time = 0;
			idx = 0;
			for (int i = 0; i < canvas.transform.childCount-1; i++)
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
		talkBox = talkObj.transform.Find("text").GetComponent<Text>();
		fakeTalkbox = talkObj.transform.Find("fakeText").GetComponent<Text>();
		endImage = talkObj.transform.Find("EndImage").gameObject;
		timelineCon = GameObject.Find("CutScenePrefab").GetComponent<TimelineController>();
		entityData = talker.GetComponent<Entity>().entityData;
		talkBox.text = "";
		fakeTalkbox.text = txt;
		talkObj.SetActive(true);
		SetBoxSize();

	}
	//타임라인 퍼즈 걸리면 실행(끝날때도 실행)
	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		if (Application.isPlaying)
		{
			if (talkObj != null)
			{
				if ((int)playable.GetTime() >= (int)playable.GetDuration())
				{
					endImage.SetActive(false);
					talkObj.SetActive(false);
				}
			}
		}
		else
		{
			if(talkObj!=null)
			talkObj.SetActive(false);
		}
	}
	//플레이어블 트랙 실행중에 계속 실행
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		if (talker == null || delayCurve.length == 0)
			return;
		if (Application.isPlaying)
		{
			talkObj.transform.position = Camera.main.WorldToScreenPoint(talker.transform.position+new Vector3(0,entityData.uiPos,0));
			time += Time.deltaTime* (float)playable.GetGraph().GetRootPlayable(0).GetSpeed();
			//endImage.SetActive(timelineCon.timelinePause);
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
			try
			{
			talkObj.transform.position = Camera.main.WorldToScreenPoint(talker.transform.position + new Vector3(0, entityData.uiPos, 0));
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
			catch
			{
				
			}
		}
	}
	public void SetBoxSize()
	{
		talkBox.rectTransform.anchoredPosition = new Vector2(0, 0);
		float x = fakeTalkbox.preferredWidth;
		float y = fakeTalkbox.preferredHeight;
		RectTransform talkBoxRect = talkObj.GetComponent<RectTransform>();
		talkBoxRect.sizeDelta = new Vector2(x, y) + new Vector2(50,50);
		talkBox.rectTransform.anchoredPosition += new Vector2(50/2 ,0);
	}
}
