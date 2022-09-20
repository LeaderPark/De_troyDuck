using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossSelectReciver : Receiver
{
	List<Entity> bossEntityList = new List<Entity>();
	UIManager uiManager;
	private void Start()
	{
		uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
	}
	public override void OnNotify(Playable origin, INotification notification, object context)
	{
		base.OnNotify(origin, notification, context);

		BossSelectMarker marker = notification as BossSelectMarker;

		if (marker != null)
		{
			bossEntityList.Clear();
			GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
			//GameObject bossObj = marker.bossObj.Resolve(origin.GetGraph().GetResolver());
			GameObject bossCamObj = marker.bossCamObj.Resolve(origin.GetGraph().GetResolver());
			foreach (var item in marker.bossObjs)
			{
				if (item.bossObj.Resolve(origin.GetGraph().GetResolver()) != null)
				{
					bossEntityList.Add(item.bossObj.Resolve(origin.GetGraph().GetResolver()).GetComponent<Entity>());
				}
			}
			//if (bossCamObj != null)
			//{
			//	bossCamObj.GetComponent<CinemachineVirtualCamera>().Follow = bossCamObj.GetComponentInChildren<CinemachineTargetGroup>().gameObject.transform;
			//	bossCamObj.SetActive(true);
			//	CinemachineTargetGroup targetGroup = bossCamObj.GetComponentInChildren<CinemachineTargetGroup>();
			//	targetGroup.m_Targets[0].target = gameManager.Player.gameObject.transform;
			//	targetGroup.m_Targets[1].target = bossObj.transform;
			//}

			//bossEntity = bossObj.GetComponent<Entity>();

			uiManager.bossStatbar.entityList = bossEntityList;
			uiManager.bossName.text = "¡º"+marker.bossName+ "¡»";
			uiManager.bossName.gameObject.SetActive(true);
			uiManager.bossStatbar.gameObject.SetActive(true);
			//StartCoroutine(OpenHpBar(uiManager.bossStatbar.transform.localScale));
		}
	}
	//IEnumerator OpenHpBar(Vector3 origin)
	//{
	//	float time = 0;

	//	while (true)
	//	{
	//		float lerpHp = 0;
	//		foreach (var item in bossEntityList)
	//		{
	//			lerpHp += item.clone.GetStat(StatCategory.Health);
	//		}
	//		time +=Time.deltaTime;
	//		float hp = Mathf.Lerp(0, lerpHp, time);
	//		uiManager.bossStatbar.UpdateStatBar(hp);

	//		if (time >= 1)
	//		{
	//			uiManager.bossStatbar.UpdateStatBar(lerpHp);
	//			yield break;
	//		}
	//		yield return null;
	//	}
	//}
}
