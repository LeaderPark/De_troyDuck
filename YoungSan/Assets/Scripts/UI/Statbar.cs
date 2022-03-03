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
        uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        uiManager.UpdateCurrentStat();
        uiManager.UpdateMaxStat();
        UpdateStatBar();
        UpdateStatText();
        
    }

    void Update()
    {

    }

    public void UpdateStatBar()
    {
        currentStat = uiManager.UpdateCurrentStat();
        maxStat = uiManager.UpdateMaxStat();
        hpSlider.maxValue = maxStat.Item1;
        staminaSlider.maxValue = maxStat.Item2;
        hpSlider.value = currentStat.Item1;
        staminaSlider.value = currentStat.Item2;
    }

    public void UpdateStatText()
    {
        hpText.text = Mathf.Round(currentStat.Item1) + " / " + maxStat.Item1;
        staminaText.text = Mathf.Round(currentStat.Item2) + " / " + maxStat.Item2;
    }
}
