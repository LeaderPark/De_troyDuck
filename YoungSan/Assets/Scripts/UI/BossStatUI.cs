using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStatUI : MonoBehaviour
{
    public Entity entity;

    public RectTransform hpRect;
    public RectTransform fakeHpRect;
    private float minHealth;

    public Image hpStain;

	private void Awake()
	{
        minHealth = hpRect.anchoredPosition.x - hpRect.rect.width;
    }
	public void UpdateStatBar(float curHp)
    {
        if (entity.isDead)
        {
            return;
        }
        float maxHp = entity.clone.GetMaxStat(StatCategory.Health);
        float hp = curHp;

        float currentHp = minHealth * (1 - (hp / maxHp));
        hpRect.anchoredPosition = new Vector2(currentHp, 0);
        hpStain.fillAmount = (hp / maxHp) - 0.02f;

        if (entity.clone.GetStat(StatCategory.Health) <= 0)
        {
            Invoke("uiActiveFalse", 1f);
        }
        if(gameObject.activeSelf)
        StartCoroutine(FakeHpSet(hpRect.anchoredPosition.x));
    }
    private IEnumerator FakeHpSet(float curretnHp)
    {
        float fill = fakeHpRect.anchoredPosition.x;

        float time = 0;
        while (true)
        {
            time += Time.deltaTime;
            fakeHpRect.anchoredPosition = new Vector2(Mathf.Lerp(fill, curretnHp, time / 1f), 0);
            yield return null;
            if (time >= 1f)
            {
                fakeHpRect.anchoredPosition = new Vector2(Mathf.Lerp(fill, curretnHp, 1f), 0);
                break;
            }
        }

    }
    private void uiActiveFalse()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}
