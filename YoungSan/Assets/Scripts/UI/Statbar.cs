using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statbar : MonoBehaviour
{
    public Slider hpSlider;
    public Slider staminaSlider;
    public Text hpText;
    public Text staminaText;

    (float, float) currentStat;
    (float, float) maxStat;

    private UIManager uiManager;
    void Start()
    {
        // hpSlider = GetComponent<Slider>();
        // staminaSlider = GetComponent<Slider>();
        uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
    }

    void Update()
    {
        UpdateStatBar();
        UpdateStatText();
    }

    void UpdateStatBar()
    {
        currentStat = uiManager.UpdateCurrentStat();
        maxStat = uiManager.UpdateMaxStat();
        hpSlider.maxValue = maxStat.Item1;
        staminaSlider.maxValue = maxStat.Item2;
        hpSlider.value = currentStat.Item1;
        staminaSlider.value = currentStat.Item2;
    }

    void UpdateStatText()
    {
        hpText.text = currentStat.Item1 + " / " + maxStat.Item1;
        staminaText.text = currentStat.Item2 + " / " + maxStat.Item2;
    }
}
