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
	

	//컷씬 스킵 부분
	private float currentSkipTime = 0;
	private bool isKeyDown = false;
	private bool currentIsSkip = false;
	public float maxSkipTime = 1.5f;

	private void Awake()
	{
		director = GetComponent<PlayableDirector>();
		TextAsset fileData = Resources.Load("TestDialogue") as TextAsset;
	}
	private void Update()
	{
		if (timelinePause)
		{
			if (Input.anyKeyDown)
			{
				StartTimeline();
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
			Debug.Log(currentSkipTime);
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
		director.Play();
		print("����");
		timelinePause = false;
	}
	public void PauseTimeline()
	{
		director.Pause();
		print("����");
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
	
	public IEnumerator CurrentCutScene()
	{
		StartCoroutine(FadeOut());
		yield return new WaitForSeconds(2f);
		double durationTime = director.duration;
		director.time = durationTime - 0.1f;
		StartCoroutine(FadeIn());
		yield return new WaitForSeconds(1f);
		currentSkipTime = 0;
		isKeyDown = false;
	}
}
