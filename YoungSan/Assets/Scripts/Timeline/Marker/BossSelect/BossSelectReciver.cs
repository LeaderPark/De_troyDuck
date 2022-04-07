using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossSelectReciver : MonoBehaviour, INotificationReceiver
{
	Entity bossEntity;
	public void OnNotify(Playable origin, INotification notification, object context)
	{
		BossSelectMarker marker = notification as BossSelectMarker;
		UIManager uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
		if (marker != null)
		{
			GameObject bossObj = marker.bossObj.Resolve(origin.GetGraph().GetResolver());
			bossEntity = bossObj.GetComponent<Entity>();
			Debug.Log(bossEntity);
			uIManager.bossStatbar.entity = bossEntity;
			uIManager.bossStatbar.gameObject.SetActive(true);

		}
	}
}
