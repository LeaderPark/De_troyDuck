using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SearchEnemyReciver : Receiver
{
	//private TimelineAsset nextTimeLine;
	GameManager gameManager;
	//List<Entity> enemys = new List<Entity>();
	public override void OnNotify(Playable origin, INotification notification, object context)
	{
		base.OnNotify(origin, notification, context);

		SearchEnemyMarker marker = notification as SearchEnemyMarker;
		if (marker != null)
		{

			gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;

			TimelineAsset nextTimeLine = marker.nextTimeLine;
			float waitTime = marker.waitTime;
			if (marker.mainChar)
			{
				Entity playerEntity = gameManager.Player.GetComponent<Entity>();
				playerEntity.dead += () =>
				{
					StartCoroutine(NextTimeLine(waitTime, nextTimeLine));
				};
				return;
			}
			if (marker.enemys.Length == 0)
			{
				//Debug.Log("일단 실행");
				StartCoroutine(NextTimeLine(waitTime, nextTimeLine));
			}
			else
			{
				List<Entity> enemys = new List<Entity>();
				int enemyCount = 0;
				for (int i = 0; i < marker.enemys.Length; i++)
				{
					if (marker.enemys[i].enemy.Resolve(origin.GetGraph().GetResolver()) == null) continue;
					Entity enemyEntity = marker.enemys[i].enemy.Resolve(origin.GetGraph().GetResolver()).GetComponent<Entity>();
					if (enemyEntity != null)
					{
						enemyCount++;
					}
				}

				for (int i = 0; i < marker.enemys.Length; i++)
				{
					if (marker.enemys[i].enemy.Resolve(origin.GetGraph().GetResolver()) == null) continue;
					Entity enemyEntity = marker.enemys[i].enemy.Resolve(origin.GetGraph().GetResolver()).GetComponent<Entity>();

					if (enemyEntity != null)
					{
						if (enemyEntity.isDead|| enemyEntity.gameObject.CompareTag("Player"))
						{
							enemys.Add(enemyEntity);
						}
						else
						{
							System.Action deadAction = null;
								
							deadAction += () => 
							{
								enemys.Add(enemyEntity);
								if (enemys.Count >= enemyCount)
								{
									enemys.Clear();
									StartCoroutine(NextTimeLine(waitTime, nextTimeLine));
								}
								enemyEntity.dead -= deadAction;
							};
							if (enemyEntity.gameObject.CompareTag("Boss"))
							{
								deadAction += () =>
								{
									StartCoroutine(TestSlow(waitTime));
								};
							}
							enemyEntity.dead += deadAction;
						}

					}
				}
			}
		}
	}
	private IEnumerator NextTimeLine(float waitTime,TimelineAsset nextTimeLine)
	{
		yield return new WaitForSecondsRealtime(waitTime);
		if (nextTimeLine != null)
		{
			TimelineManager timelineManager = ManagerObject.Instance.GetManager(ManagerType.TimelineManager) as TimelineManager;
			timelineManager.StartCutScene(nextTimeLine);
		}
		else
		{
		}
	}
	private IEnumerator TestSlow(float waitTime)
	{
		gameManager.Player.ActiveScript(false);
		gameManager.CamFollowFind();
		Time.timeScale = 0.2f;
		yield return new WaitForSecondsRealtime(3f);
		if(waitTime<=0)
		gameManager.Player.ActiveScript(true);
		Time.timeScale = 1f;

	}
}
