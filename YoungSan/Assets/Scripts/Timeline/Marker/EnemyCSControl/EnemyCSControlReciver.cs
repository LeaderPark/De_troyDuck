using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyCSControlReciver :Receiver
{
	public override void OnNotify(Playable origin, INotification notification, object context)
	{
		base.OnNotify(origin, notification, context);

		EnemyCSControlMarker marker = notification as EnemyCSControlMarker;
		if (marker != null)
		{
			for (int i = 0; i < marker.enemyObjs.Length; i++)
			{
				GameObject enemy = marker.enemyObjs[i].obj.Resolve(origin.GetGraph().GetResolver());
				if (enemy != null)
				{
					enemy.GetComponent<StateMachine.StateMachine>().enabled = marker.scriptEnable;
					enemy.GetComponent<EntityEvent>().enabled = marker.scriptEnable;
					enemy.GetComponent<Entity>().enabled = marker.scriptEnable;
				}
			}
		}
	}
}
