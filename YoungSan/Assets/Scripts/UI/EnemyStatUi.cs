using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatUI : MonoBehaviour
{
	private GameObject hpBar;
	private GameObject fakeHpBar;
	public Entity entity;
	private Transform parentTrm;

    private void Awake()
	{
		parentTrm = transform.parent;
		hpBar = transform.Find("HpBar").gameObject;
		fakeHpBar = transform.Find("FakeHpBar").gameObject;
	}
	public void SetPos()
	{
		parentTrm.SetParent(entity.transform);
		parentTrm.localPosition = new Vector3(0, 0, 0);
		parentTrm.localPosition += new Vector3(0, entity.entityData.uiPos, 0);
	}
	public void SetHpBarValue(float maxHp, float currentHp)
	{
		//Debug.Log(entity.isDead);
		Vector3 origin = hpBar.transform.localScale;
		hpBar.transform.localScale = new Vector3(currentHp / maxHp, origin.y, origin.z);
		if (currentHp <= 0 || entity.isDead)
		{
			transform.parent.gameObject.SetActive(false);
		}
		else
		{
			StartCoroutine(FakeHpSet(maxHp, currentHp));
		}
	}
	private IEnumerator FakeHpSet(float maxHp,float curretnHp)
	{
		Vector3 origin = hpBar.transform.localScale;
		float fill = fakeHpBar.transform.localScale.x;
		float time = 0;
		while (true)
		{
			time += Time.deltaTime;
			fakeHpBar.transform.localScale = new Vector3(Mathf.Lerp(fill, curretnHp / maxHp, time/0.5f),origin.y,origin.x);
			yield return null;
			if (time >= 0.5f)
			{
				fakeHpBar.transform.localScale = new Vector3(Mathf.Lerp(fill, curretnHp / maxHp, 1), origin.y, origin.x);
				break;
			}

		}
	}
	//public void SetParent(Entity _entity)
	//{
	//	gameObject.transform.SetParent(_entity.gameObject.transform);
	//}
	//private void Update()
	//{
	//	if (entity != null)
	//	{
	//		transform.position = Camera.main.WorldToScreenPoint(entity.gameObject.transform.position + new Vector3(0, entity.entityData.uiPos, 0));
	//	}
	//}


}
