using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class ObjectControlReciver :Receiver
{

	public override void OnNotify(Playable origin, INotification notification, object context)
	{
		base.OnNotify(origin, notification, context);

		ObjectControlMarker marker = notification as ObjectControlMarker;
		if (marker != null)
		{
			if (timelineCon.targetMarker != null)
			{
				return;
			}
			GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
			for (int i = 0; i < marker.animationDatas.Length; i++)
			{
				GameObject obj;

				if (marker.animationDatas[i].mainChar)
				{
					obj = gameManager.Player.gameObject;
					obj.GetComponent<SpriteRenderer>().flipX = marker.animationDatas[i].isLeft;
				}
				else
				{
					obj = marker.animationDatas[i].contorolObject.Resolve(origin.GetGraph().GetResolver());
				}
				string clip = marker.animationDatas[i].animation;

				if (obj != null)
				{

					Animator objAnimator = obj.GetComponent<Animator>();
					if (marker.animationDatas[i].getMainCharAnimator)
					{
						obj.gameObject.transform.localScale = gameManager.Player.gameObject.transform.localScale;
						objAnimator.runtimeAnimatorController = gameManager.Player.GetComponent<Animator>().runtimeAnimatorController;
					}
                    switch (clip)
                    {
						case "Attack":
							clip = gameManager.Player.GetComponentInChildren<SkillSet>().skillDatas[EventCategory.DefaultAttack][0].skill.name;
							break;
                    }
                    objAnimator.Play(clip);
				}
			}
			for (int i = 0; i < marker.objectData.Length; i++)
			{
				GameObject obj;
				Transform objTrm;
				objTrm = marker.objectData[i].objTrm.Resolve(origin.GetGraph().GetResolver());
				bool _active = marker.objectData[i].active;
				if (marker.objectData[i].mainChar)
				{
					obj = gameManager.Player.gameObject;
					obj.GetComponent<Entity>().enabled = _active;
					obj.GetComponent<Player>().enabled = _active;
					obj.GetComponent<EntityEvent>().enabled = _active;
					obj.GetComponent<SpriteRenderer>().enabled = _active;
					obj.GetComponent<Collider>().enabled = _active;
					obj.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
				}
				else
				{
					obj = marker.objectData[i].obj.Resolve(origin.GetGraph().GetResolver());
					if (obj != null)
						obj.SetActive(_active);

				}
				if (marker.objectData[i].mainCharTrm)
				{
					obj.transform.position = gameManager.Player.transform.position;
				}
				else if (obj != null)
				{
					if (objTrm != null)
					{
						obj.transform.position = objTrm.position;
					}
					else
					{
						obj.transform.position = marker.objectData[i].objPos;
					}
				}
			}
			
			//GameObject obj = marker.contorolObject.Resolve(origin.GetGraph().GetResolver());
		}
	}

}
