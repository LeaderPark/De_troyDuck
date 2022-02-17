using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hitTest : MonoBehaviour
{
	public static hitTest Instance;
	[SerializeField]
	private GameObject hitImagePrefab;

	private void Awake()
	{
		if (Instance!=null)
		{
			Destroy(this);
		}
		Instance = this;
		print(Instance);
	}

	public void hitEffect()
	{
		StartCoroutine(hitEffectTest());
	}
	public IEnumerator hitEffectTest()
	{
		PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;

		Image hitImage = poolManager.GetObject("hitImageTest").GetComponent<Image>();

		hitImage.gameObject.transform.SetParent(this.gameObject.transform);
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

