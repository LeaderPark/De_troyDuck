using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SearchEnemyReciver : MonoBehaviour, INotificationReceiver
{
	private TimelineAsset nextTimeLine;
	GameManager gameManager;
	List<Entity> enemys = new List<Entity>();
	float waitTime;
	public void OnNotify(Playable origin, INotification notification, object context)
	{
		gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
		
		SearchEnemyMarker marker = notification as SearchEnemyMarker;
		if (marker != null)
		{
			nextTimeLine = marker.nextTimeLine;
			waitTime = marker.waitTime;
			if (marker.enemys.Length == 0)
			{
				StartCoroutine(NextTimeLine(0));
			}
			else
			{
				int enemyCount = 0;
				for (int i = 0; i < marker.enemys.Length; i++)
				{
					Entity enemyEntity = marker.enemys[i].enemy.Resolve(origin.GetGraph().GetResolver()).GetComponent<Entity>();
					if (enemyEntity != null)
					{
						enemyCount++;
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
			timelineManager.StartCutScene(nextTimeLine.name);
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
