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
    public EnemyStaUIt enemyStatUI;
    public QuestUI questUI;
    public SettingUI settingUI;
    public Skillinterface skillinterface;
    public Statbar statbar;
    public StatUI statUI;
    public TimeLineSkipGage timeLineSkipGage;

    [SerializeField] private CanvasGroup canvas;

    [SerializeField] private CanvasGroup fade;

    public bool important;

    void Awake()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 0)
        {
            Init();
        }
    }

    public void Init()
    {
        skillinterface.Init();
        statbar.Init();

        EventManager eventManager = ManagerObject.Instance.GetManager(ManagerType.EventManager) as EventManager;
        eventManager.GetEventTrigger(typeof(StatEventTrigger)).Add(new GlobalEventTrigger.StatEvent((entity, category, oldValue, value) =>
        {
            statbar.UpdateStatBar();
            statbar.UpdateStatText();

            if (category == StatCategory.Health) enemyStatUI.EnemyHpBarUpdate(entity);
        }));
        important = false;
    }

    #region On Off

    public void OpenUI(CanvasGroup canvasGroup, bool isTimeStop)
    {
        if (important)
        {
            return;
        }

        if (isTimeStop)
        {
            Time.timeScale = 0;
        }

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void CloseUI(CanvasGroup canvasGroup)
    {
        Time.timeScale = 1;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    #endregion On Off

    public void SetDelayUI(Entity entity, float time)
    {
        StartCoroutine(MakeEnemyDelay(entity, time));
    }

    public void UISetActiveTimeLine(bool active)
    {
        if (active)
            canvas.alpha = 1;
        else
            canvas.alpha = 0;
    }

    public void UISetActiveFalse()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<CanvasGroup>())
            {
                CloseUI(transform.GetChild(i).GetComponent<CanvasGroup>());
            }
        }
        important = true;
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

    private IEnumerator MakeEnemyDelay(Entity entity, float time)
    {
        SpriteRenderer spriteRenderer = entity.GetComponent<SpriteRenderer>();
        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
        GameObject obj = poolManager.GetObject("EnemyDelay");
        EnemyDelayUI enemyDelayUI = obj.GetComponentInChildren<EnemyDelayUI>();
        enemyDelayUI.SetTarget(spriteRenderer);
        enemyDelayUI.Play();
        StartCoroutine(enemyDelayUI.AfterImageInActive(obj, time));
        yield return null;
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
                CloseUI(fade);
                if (endAction != null)
                    endAction();

                yield break;
            }
            alpha = Mathf.Clamp(alpha, 0, 1);
            fade.alpha = alpha;
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
                OpenUI(fade, false);
                yield break;
            }
            alpha = Mathf.Clamp(alpha, 0, 1);
            fade.alpha = alpha;
            yield return null;
        }
    }
}
