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
    public Text hpText;
    public Text staminaText;

    private float playerCurrentHP;
    private float playerCurrentStamina;
    private float playerMaxHP;
    private float playerMaxStamina;
    private float hp;
    private float stamina;

    (float, float) currentStat;
    (float, float) maxStat;

    private void Awake()
    {
        minHealth = hpRect.anchoredPosition.x - hpRect.rect.width;
        minStamina = staminaRect.anchoredPosition.x - staminaRect.rect.width;
    }

    public void Init()
    {
        SetStatBar();
        UpdateStatBar();
    }

    public void UpdateStatBar()
    {
        currentStat = UpdateCurrentStat();
        maxStat = UpdateMaxStat();
        float currentHp = minHealth * (1 - (currentStat.Item1 / maxStat.Item1));
        float currentStamina = minStamina * (1 - (currentStat.Item2 / maxStat.Item2));
        hpRect.anchoredPosition = new Vector2(currentHp, 0);
        staminaRect.anchoredPosition = new Vector2(currentStamina, 0);

        hpStain.fillAmount = (currentStat.Item1 / maxStat.Item1) - 0.02f;
        staminaStain.fillAmount = (currentStat.Item2 / maxStat.Item2) - 0.02f;

        StartCoroutine(FakeHpSet(hpRect.anchoredPosition.x));
    }
    public void SetStatBar()
    {
        StopAllCoroutines();
        currentStat = UpdateCurrentStat();
        maxStat = UpdateMaxStat();
    }

    public void UpdateStatText()
    {
        hpText.text = Mathf.Round(currentStat.Item1) + " / " + maxStat.Item1;
        staminaText.text = Mathf.Round(currentStat.Item2) + " / " + maxStat.Item2;
    }

    public (float, float) UpdateCurrentStat()
    {
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        Entity entity = gameManager.Player.GetComponent<Entity>();
        playerCurrentHP = entity.clone.GetStat(StatCategory.Health);
        playerCurrentStamina = entity.clone.GetStat(StatCategory.Stamina);
        return (playerCurrentHP, playerCurrentStamina);
    }

    public float BackUpHpStat()
    {
        currentStat = UpdateCurrentStat();
        maxStat = UpdateMaxStat();

        hp = currentStat.Item1 / maxStat.Item1;

        return hp;
    }

    public float BackUpStaminaStat()
    {
        currentStat = UpdateCurrentStat();
        maxStat = UpdateMaxStat();

        stamina = currentStat.Item2 / maxStat.Item2;

        return stamina;
    }

    public (float, float) UpdateMaxStat()
    {
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        Entity entity = gameManager.Player.GetComponent<Entity>();
        playerMaxHP = entity.clone.GetMaxStat(StatCategory.Health);
        playerMaxStamina = entity.clone.GetMaxStat(StatCategory.Stamina);
        return (playerMaxHP, playerMaxStamina);
    }

    public float UpdateStat(Entity entity)
    {
        return entity.clone.GetStat(StatCategory.Health);
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
}
