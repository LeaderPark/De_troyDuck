using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossSelectReciver : MonoBehaviour, INotificationReceiver
{
	Entity bossEntity;
	UIManager uIManager;
	private void Start()
	{
		uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
	}
	public void OnNotify(Playable origin, INotification notification, object context)
	{
		BossSelectMarker marker = notification as BossSelectMarker;

		if (marker != null)
		{
			GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
			GameObject bossObj = marker.bossObj.Resolve(origin.GetGraph().GetResolver());
			GameObject bossCamObj = marker.bossCamObj.Resolve(origin.GetGraph().GetResolver());

			bossCamObj.GetComponent<CinemachineVirtualCamera>().Follow = bossCamObj.GetComponentInChildren<CinemachineTargetGroup>().gameObject.transform;
			bossCamObj.SetActive(true);
			CinemachineTargetGroup targetGroup = bossCamObj.GetComponentInChildren<CinemachineTargetGroup>();

			targetGroup.m_Targets[0].target = gameManager.Player.gameObject.transform;
			targetGroup.m_Targets[1].target = bossObj.transform;

			bossEntity = bossObj.GetComponent<Entity>();
			uIManager.bossStatbar.entity = bossEntity;
			uIManager.bossStatbar.gameObject.SetActive(true);
			StartCoroutine(OpenHpBar(uIManager.bossStatbar.transform.localScale));
		}
	}
	IEnumerator OpenHpBar(Vector3 origin)
	{
		float time = 0;
		while (true)
		{
			time +=Time.deltaTime;
			float hp = Mathf.Lerp(0, bossEntity.clone.GetStat(StatCategory.Health), time);
			uIManager.bossStatbar.UpdateStatBar(hp);

			if (time >= 1)
			{
				uIManager.bossStatbar.UpdateStatBar(bossEntity.clone.GetStat(StatCategory.Health));
				yield break;
			}
			yield return null;
		}
	}
}
