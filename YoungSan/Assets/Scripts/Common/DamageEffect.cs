using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
	private static DamageEffect instance;
	public static DamageEffect Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<DamageEffect>();
				if (instance == null)
				{
					instance = new GameObject("Damage Effect").AddComponent<DamageEffect>();
					DontDestroyOnLoad(instance.gameObject);
				}
			}
			return instance;
		}
	}

	private GameObject canvas;

	public void OnDamageEffect()
	{
		StartCoroutine(DamageEffectProcess());
	}
	public IEnumerator DamageEffectProcess()
	{
		PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;

		Image hitImage = poolManager.GetObject("DamageEffect").GetComponent<Image>();

		if (canvas == null)
		{
			canvas = GameObject.Find("Canvas");
		}

		hitImage.gameObject.transform.SetParent(canvas.transform);
		//hitImage.gameObject.transform.SetParent(this.gameObject.transform);
		hitImage.rectTransform.anchorMax = Vector2.one;
		hitImage.rectTransform.anchorMin = Vector2.zero;
		hitImage.rectTransform.anchoredPosition = Vector2.zero;

		hitImage.color = new Color(hitImage.color.r, hitImage.color.g, hitImage.color.b, 1);

		yield return new WaitForSeconds(0.05f);
		float time = 0;
		while (true)
		{
			time += Time.deltaTime;
			hitImage.color = new Color(hitImage.color.r, hitImage.color.g, hitImage.color.b, Mathf.Lerp(1, 0, time/0.5f));
			if (time >= 0.5f)
			{
				hitImage.color = new Color(hitImage.color.r, hitImage.color.g, hitImage.color.b, 0);
				break;
			}
			yield return null;
		}
		hitImage.gameObject.transform.SetParent(poolManager.gameObject.transform);
		hitImage.gameObject.SetActive(false);
	}
}

