using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatUi : MonoBehaviour
{
	private Image hpBar;
	public Entity entity;
	private RectTransform canvasPos;

	private void Awake()
	{
		hpBar = transform.Find("HpBar").GetComponent<Image>();
		canvasPos = transform.parent.GetComponent<RectTransform>();


	}
	public void SetPos()
	{
		transform.parent.SetParent(entity.transform);
		canvasPos.localPosition = new Vector3(0, 0, 0);
		canvasPos.localPosition += new Vector3(0, entity.entityData.uiPos, 0);
	}
	public void SetHpBarValue(float maxHp,float currentHp)
	{
		Debug.Log(currentHp);
		hpBar.fillAmount = currentHp / maxHp;
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
