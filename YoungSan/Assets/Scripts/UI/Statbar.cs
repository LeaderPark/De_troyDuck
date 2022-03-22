using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statbar : MonoBehaviour
{
    public RectTransform hpRect;
    public RectTransform fakeHpRect;
    public RectTransform staminaRect;
    private float minHealth;
    private float minStamina;


    public Image hpStain;
    public Image staminaStain;
    //public Slider fakeHpSlider;
    //public Slider staminaSlider;
    public Text hpText;
    public Text staminaText;

    (float, float) currentStat;
    (float, float) maxStat;

    private UIManager uiManager;
	private void Awake()
	{
        minHealth = hpRect.anchoredPosition.x - hpRect.rect.width;
        minStamina = staminaRect.anchoredPosition.x - staminaRect.rect.width;

    }
    void Start()
    {
        uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        SetStatBar();
        UpdateStatBar();
        //UpdateStatText();
    }

    public void UpdateStatBar()
    {
        currentStat = uiManager.UpdateCurrentStat();
        maxStat = uiManager.UpdateMaxStat();
        //Debug.Log(1-(currentStat.Item1/maxStat.Item1));
        float currentHp = minHealth * (1 - (currentStat.Item1 / maxStat.Item1));
        float currentStamina = minStamina * (1 - (currentStat.Item2 / maxStat.Item2));
        hpRect.anchoredPosition = new Vector2(currentHp, 0);
        staminaRect.anchoredPosition = new Vector2(currentStamina, 0);

        hpStain.fillAmount = (currentStat.Item1 / maxStat.Item1) - 0.02f;
        staminaStain.fillAmount = (currentStat.Item2 / maxStat.Item2) - 0.02f;

        StartCoroutine(FakeHpSet(hpRect.anchoredPosition.x));

        //hpSlider.maxValue = maxStat.Item1;
        //staminaSlider.maxValue = maxStat.Item2;
        //fakeHpSlider.maxValue = maxStat.Item1;
        //hpSlider.value = currentStat.Item1;
        //staminaSlider.value = currentStat.Item2;
        //StartCoroutine(FakeHpSet(hpSlider.value));
    }
    public void SetStatBar()
    {



        StopAllCoroutines();
        currentStat = uiManager.UpdateCurrentStat();
        maxStat = uiManager.UpdateMaxStat();
        //hpSlider.maxValue = maxStat.Item1;
        //staminaSlider.maxValue = maxStat.Item2;
        //fakeHpSlider.maxValue = hpSlider.maxValue;
        //hpSlider.value = currentStat.Item1;
        //staminaSlider.value = currentStat.Item2;
        //fakeHpSlider.value = hpSlider.value;

    }

    public void UpdateStatText()
    {
        hpText.text = Mathf.Round(currentStat.Item1) + " / " + maxStat.Item1;
        staminaText.text = Mathf.Round(currentStat.Item2) + " / " + maxStat.Item2;
    }

    private IEnumerator FakeHpSet(float curretnHp)
    {
		//float fill = fakeHpSlider.value;

		//float time = 0;
		//while (true)
		//{
		//    time += Time.deltaTime;
		//    fakeHpSlider.value = Mathf.Lerp(fill, curretnHp, time / 1f);
		//    yield return null;
		//    if (time >= 1f)
		//    {
		//        fakeHpSlider.value = Mathf.Lerp(fill, curretnHp, 1);
		//        break;
		//    }

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
}
