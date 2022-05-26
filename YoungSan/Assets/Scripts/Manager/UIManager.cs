using System.Net.Mail;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : Manager
{
    public BossStatUI bossStatbar;
    public DieUI dieUI;
    public EnemyDelayUI enemyDelayUI;
    public EnemyStatUI enemyStatUI;
    public QuestUI questUI;
    public SettingUI settingUI;
    public Skillinterface skillinterface;
    public Statbar statbar;
    public StatUI statUI;
    public TimeLineSkipGage timeLineSkipGage;

    [SerializeField] private CanvasGroup canvas;

    [SerializeField] private Image fade;

    void Awake()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 0)
            Init();
    }

    public void Init()
    {
        // bossStatbar = transform.GetComponentInChildren<BossStatUI>();
        // dieUI = transform.GetComponentInChildren<DieUI>();
        // enemyDelayUI = transform.GetComponentInChildren<EnemyDelayUI>();
        // enemyStatUI = transform.GetComponentInChildren<EnemyStatUI>();
        // questUI = transform.GetComponentInChildren<QuestUI>();
        // settingUI = transform.GetComponentInChildren<SettingUI>();
        // skillinterface = transform.GetComponentInChildren<Skillinterface>();
        // statbar = transform.GetComponentInChildren<Statbar>();
        // timeLineSkipGage = transform.GetComponentInChildren<TimeLineSkipGage>();

        EventManager eventManager = ManagerObject.Instance.GetManager(ManagerType.EventManager) as EventManager;
        eventManager.GetEventTrigger(typeof(StatEventTrigger)).Add(new GlobalEventTrigger.StatEvent((entity, category, oldValue, value) =>
        {
            statbar.UpdateStatBar();
            statbar.UpdateStatText();

            if (category == StatCategory.Health) enemyStatUI.EnemyHpBarUpdate(entity);
        }));
    }

    public void UISetActive(bool active)
    {
        if (active)
            canvas.alpha = 1;
        else
            canvas.alpha = 0;
    }

    public void FadeInOut(bool fadeOut, Action endAction = null)
    {
        if (fadeOut)
        {
            StartCoroutine(FadeOut(endAction));
        }
        else
        {
            StartCoroutine(FadeIn(endAction));
        }
    }

    private IEnumerator FadeOut(Action endAction)
    {
        float alpha = 0f;
        while (true)
        {
            if (alpha < 1f)
            {
                alpha += Time.deltaTime * 1;
            }
            else
            {
                endAction();
                yield break;
            }
            alpha = Mathf.Clamp(alpha, 0, 1);
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, alpha);
            yield return null;
        }
    }
    private IEnumerator FadeIn(Action endAction)
    {
        float alpha = 1f;
        while (true)
        {
            if (alpha > 0f)
            {
                alpha -= Time.deltaTime * 1;
            }
            else
            {
                yield break;
            }
            alpha = Mathf.Clamp(alpha, 0, 1);
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, alpha);
            yield return null;
        }
    }
}
