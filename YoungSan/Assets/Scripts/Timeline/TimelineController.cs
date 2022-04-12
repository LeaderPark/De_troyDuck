using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class TimelineController : MonoBehaviour
{
	PlayableDirector director;
	[SerializeField] private Image fade;
	private bool timelinePause = false;

	private void Update()
	{
		if (timelinePause)
		{
			if (Input.anyKeyDown)
			{
				StartTimeline();
			}
		}
	}
	private void Awake()
	{
		director = GetComponent<PlayableDirector>();
		TextAsset fileData = Resources.Load("TestDialogue") as TextAsset;
	}
	public void StartTimeline()
	{
		director.playableGraph.GetRootPlayable(0).SetSpeed(1);
		print("Ω√¿€");
	}
	public void PauseTimeline()
	{
		director.playableGraph.GetRootPlayable(0).SetSpeed(0);
		print("∏ÿ√„");
		timelinePause = true;
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
	public void TEst(string a)
	{
		Debug.LogError(a);
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
}
