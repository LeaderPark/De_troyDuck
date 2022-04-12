using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SearchEnemyReciver : MonoBehaviour, INotificationReceiver
{
	private TimelineAsset nextTimeLine;
	List<Entity> enemys = new List<Entity>();
	public void OnNotify(Playable origin, INotification notification, object context)
	{
		SearchEnemyMarker marker = notification as SearchEnemyMarker;
		if (marker != null)
		{
			nextTimeLine = marker.nextTimeLine;
			if (marker.enemys.Length == 0)
			{
				NextTimeLine();
			}
			else
			{
				
				for (int i = 0; i < marker.enemys.Length; i++)
				{
					Entity enemyEntity = marker.enemys[i].enemy.Resolve(origin.GetGraph().GetResolver()).GetComponent<Entity>();
					int a = i;
					enemyEntity.dead += () =>
					{
						enemys.Add(enemyEntity);
						if (enemys.Count >= marker.enemys.Length)
						{
							enemys.Clear();
							NextTimeLine();
						}
						enemyEntity.dead = null;
					};
					if (enemyEntity.gameObject.CompareTag("Boss"))
					{
						enemyEntity.dead += () =>
						{
							//StartCoroutine(TestSlow());
						};
					}
				}
			}
		}
	}
	private void NextTimeLine()
	{
		if (nextTimeLine != null)
		{
			TimelineManager timelineManager = ManagerObject.Instance.GetManager(ManagerType.TimelineManager) as TimelineManager;
			timelineManager.StartCutScene(nextTimeLine.name);
		}
		else
		{
			Debug.Log("암튼 다음걸로 넘어갔음");
		}
	}
	private IEnumerator TestSlow()
	{
		Time.timeScale = 0.2f;
		yield return new WaitForSecondsRealtime(3f);
		Time.timeScale = 1f;

	}
}
