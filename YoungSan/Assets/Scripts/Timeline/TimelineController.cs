using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class TimelineController : MonoBehaviour
{
	PlayableDirector director;
	[SerializeField] private Image fade;

	public Entity entity;

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
	}
	public void playAnimation(AnimationClip obj)
	{
		print(obj.name);
		entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[] {obj.name});
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
