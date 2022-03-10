using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statbar : MonoBehaviour
{
    public Slider hpSlider;
    public Slider fakeHpSlider;
    public Slider staminaSlider;
    public Text hpText;
    public Text staminaText;

    (float, float) currentStat;
    (float, float) maxStat;

    private UIManager uiManager;
    void Start()
    {
        uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        UpdateStatBar();
        UpdateStatText();
    }

    public void UpdateStatBar()
    {
        currentStat = uiManager.UpdateCurrentStat();
        maxStat = uiManager.UpdateMaxStat();
        hpSlider.maxValue = maxStat.Item1;
        staminaSlider.maxValue = maxStat.Item2;
        fakeHpSlider.maxValue = maxStat.Item1;
        hpSlider.value = currentStat.Item1;
        staminaSlider.value = currentStat.Item2;
        StartCoroutine(FakeHpSet(hpSlider.value));
    }
    public void SetStatBar()
    {
        currentStat = uiManager.UpdateCurrentStat();
        maxStat = uiManager.UpdateMaxStat();
        hpSlider.maxValue = maxStat.Item1;
        staminaSlider.maxValue = maxStat.Item2;
        fakeHpSlider.maxValue = maxStat.Item1;
        hpSlider.value = currentStat.Item1;
        staminaSlider.value = currentStat.Item2;
        fakeHpSlider.value = currentStat.Item1;
    }

    public void UpdateStatText()
    {
        hpText.text = Mathf.Round(currentStat.Item1) + " / " + maxStat.Item1;
        staminaText.text = Mathf.Round(currentStat.Item2) + " / " + maxStat.Item2;
    }

    private IEnumerator FakeHpSet(float curretnHp)
    {
        float fill = fakeHpSlider.value;

        float time = 0;
        while (true)
        {
            time += Time.deltaTime;
            fakeHpSlider.value = Mathf.Lerp(fill, curretnHp, time / 0.5f);
            yield return null;
            if (time >= 0.5f)
            {
                fakeHpSlider.value = Mathf.Lerp(fill, curretnHp, 1);
                break;
            }

        }
    }
}
