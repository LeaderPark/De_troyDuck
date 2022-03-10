using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatUi : MonoBehaviour
{
	private SpriteRenderer hpBar;
	private SpriteRenderer fakeHpBar;
	public Entity entity;
	private RectTransform canvasPos;

	private void Awake()
	{
		hpBar = transform.Find("HpBar").GetComponent<SpriteRenderer>();
		fakeHpBar = transform.Find("FakeHpBar").GetComponent<SpriteRenderer>();
		canvasPos = transform.parent.GetComponent<RectTransform>();
	}
	public void SetPos()
	{
		print(transform.Find("HpBar"));
		transform.parent.SetParent(entity.transform);
		canvasPos.localPosition = new Vector3(0, 0, 0);
		canvasPos.localPosition += new Vector3(0, entity.entityData.uiPos, 0);
	}
	public void SetHpBarValue(float maxHp, float currentHp)
	{
		Debug.Log(currentHp);
		//hpBar.fillAmount = currentHp / maxHp;
		StartCoroutine(FakeHpSet(maxHp, currentHp));
		if (entity.isDead)
		{
			transform.parent.gameObject.SetActive(false);
		}
	}
	private IEnumerator FakeHpSet(float maxHp,float curretnHp)
	{
		//float fill = fakeHpBar.fillAmount;
		float time = 0;
		while (true)
		{
			time += Time.deltaTime;
			//fakeHpBar.fillAmount = Mathf.Lerp(fill, curretnHp / maxHp, time/0.5f);
			yield return null;
			if (time >= 0.5f)
			{
				//fakeHpBar.fillAmount = Mathf.Lerp(fill, curretnHp / maxHp, 1);
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
