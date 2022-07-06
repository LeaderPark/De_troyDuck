using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SearchEnemyReciver : Receiver
{
	private TimelineAsset nextTimeLine;
	GameManager gameManager;
	List<Entity> enemys = new List<Entity>();
	float waitTime;
	public override void OnNotify(Playable origin, INotification notification, object context)
	{
		base.OnNotify(origin, notification, context);

		SearchEnemyMarker marker = notification as SearchEnemyMarker;
		if (marker != null)
		{
			gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;

			nextTimeLine = marker.nextTimeLine;
			waitTime = marker.waitTime;
			if (marker.mainChar)
			{
				Entity playerEntity = gameManager.Player.GetComponent<Entity>();
				playerEntity.dead += () =>
				{
					StartCoroutine(NextTimeLine(waitTime));

				};
				return;
			}
			if (marker.enemys.Length == 0)
			{
				StartCoroutine(NextTimeLine(0));
			}
			else
			{
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
						if (enemyEntity.isDead)
						{
							enemys.Add(enemyEntity);
						}
						else
						{
							enemyEntity.dead += () =>
							{
								enemys.Add(enemyEntity);
								if (enemys.Count >= enemyCount)
								{
									enemys.Clear();
									StartCoroutine(NextTimeLine(waitTime));
								}
								enemyEntity.dead = null;
							};
						}
						if (enemyEntity.gameObject.CompareTag("Boss"))
						{
							enemyEntity.dead += () =>
							{
								StartCoroutine(TestSlow());
							};
						}
					}
				}
			}
		}
	}
	private IEnumerator NextTimeLine(float waitTime)
	{
		yield return new WaitForSecondsRealtime(waitTime);
		if (nextTimeLine != null)
		{
			TimelineManager timelineManager = ManagerObject.Instance.GetManager(ManagerType.TimelineManager) as TimelineManager;
			timelineManager.StartCutScene(nextTimeLine);
		}
		else
		{
			//Debug.Log("��ư �����ɷ� �Ѿ��");
		}
	}
	private IEnumerator TestSlow()
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
