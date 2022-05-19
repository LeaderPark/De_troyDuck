using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class TimelineController : MonoBehaviour
{
	public List<TimelineEvents> timelineEvents = new List<TimelineEvents>();


	PlayableDirector director;
	[SerializeField] private Image fade;
	public bool talkLoop = true;


	[HideInInspector]
	public JumpMarker jumpMarker;
	public LoopEndMarker targetMarker;
	//컷씬 스킵 부분
	private float currentSkipTime = 0;
	private bool isKeyDown = false;
	private bool currentIsSkip = false;
	public float maxSkipTime = 1.5f;

	private void Awake()
	{
		director = GetComponent<PlayableDirector>();
		UnityEngine.TextAsset fileData = Resources.Load("TestDialogue") as UnityEngine.TextAsset;
	}
	private void Update()
	{
		if (jumpMarker != null)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				StartTimeline();
			}
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
			talkLoop = true;
			}
		}

		if(Input.GetKeyDown(KeyCode.Space) && director.state == PlayState.Playing)
		{	
			isKeyDown = true;
		}
		else if(Input.GetKeyUp(KeyCode.Space))
		{
			currentSkipTime = 0;
			isKeyDown = false;
			currentIsSkip = false;
		}

		if(isKeyDown)
		{
			currentSkipTime += Time.deltaTime;
			//Debug.Log(currentSkipTime);
			if(currentSkipTime >= maxSkipTime)
			{
				currentIsSkip = true;
			}
		}

		if(currentIsSkip)
		{
			currentSkipTime = 0;
			currentIsSkip = false;
			isKeyDown = false;
			StartCoroutine(CurrentCutScene());
		}
	}
	public void StartTimeline()
	{
		if (talkLoop)
		{
			talkLoop = false;
			if(!jumpMarker.qeustSelect)
			director.time = jumpMarker.loopEndMarker.time;
			jumpMarker = null;
		}
	}
	public void TimelineEnd()
	{
		director.Stop();
	}
	public void PauseTimeline()
	{
		//director.playableGraph.Stop();

		director.playableGraph.GetRootPlayable(0).SetSpeed(0);

	//	director.playableGraph.GetRootPlayable(0).SetTime(director.playableGraph.GetRootPlayable(0).GetTime());

	}
	public void UISetActiveFalse()
	{
		UIManager uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
		uIManager.UISetActive(false);
	}
	public void UISetActiveTrue()
	{
		UIManager uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
		uIManager.UISetActive(true);

	}
	public void PlayerScriptActive(bool active)
	{
		GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
		gameManager.Player.ActiveScript(active);
	}
	public void FadeInOut(bool fadeOut)
	{
		if (fadeOut)
		{
			StartCoroutine(FadeOut());
		}
		else
		{
			StartCoroutine(FadeIn());
		}
	}
	private IEnumerator FadeOut()
	{
		float alpha = 0f;
		while (true)
		{
			if (alpha < 1f)
			{
				alpha += Time.deltaTime * 1;
			}
			else
			{
				yield break;
			}
			alpha = Mathf.Clamp(alpha, 0, 1);
			fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, alpha);
			yield return null;
		}
	}
	private IEnumerator FadeIn()
	{
		float alpha = 1f;
		while (true)
		{
			if (alpha > 0f)
			{
				alpha -= Time.deltaTime * 1;
			}
			else
			{
				yield break;
			}
			alpha = Mathf.Clamp(alpha, 0, 1);
			fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, alpha);
			yield return null;
		}
	}
	
	public IEnumerator CurrentCutScene()
	{
		director = GetComponent<PlayableDirector>();
		double durationTime = director.duration;
		TimelineAsset timelineAsset = (TimelineAsset)director.playableAsset;
		for (int j = 0; j < timelineAsset.rootTrackCount; j++)
		{
			for (int i = 0; i < timelineAsset.GetRootTrack(j).GetMarkerCount(); i++)
			{
				SceneLoadMarker loadmarker = timelineAsset.GetRootTrack(j).GetMarker(i) as SceneLoadMarker;
				JumpMarker jumpMarker = timelineAsset.GetRootTrack(j).GetMarker(i) as JumpMarker;

				if(jumpMarker != null)
				{
					Debug.Log("아니 쓱발아 퀘스트를 쳐 받으라고");
					yield break;	
				}

				if(loadmarker != null)
				{
					Debug.Log("아니 슈발 마커가 있다니깐");
					durationTime = loadmarker.time - 0.1f;
				}
			}
		}


		Debug.Log(durationTime);
		StartCoroutine(FadeOut());
		yield return new WaitForSeconds(2f);
		director.time = durationTime - 0.1f;
		StartCoroutine(FadeIn());
		yield return new WaitForSeconds(1f);
		currentSkipTime = 0;
		isKeyDown = false;
	}
}

[System.Serializable]
public struct TimelineEvents
{
	public string evnetName;
	public UnityEvent events;
}
