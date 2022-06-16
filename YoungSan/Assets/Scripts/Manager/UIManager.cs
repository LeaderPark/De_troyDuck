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

    public Quest quest1;

    [SerializeField] private CanvasGroup canvas;

    [SerializeField] private CanvasGroup fade;

    public QuestUI[] questUIObj;

    public bool important;

    void Start()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 0)
        {
            Init();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetQuestUI(quest1);
        }
    }

    public void Init()
    {
        important = false;
        skillinterface.Init();
        statbar.Init();

        EventManager eventManager = ManagerObject.Instance.GetManager(ManagerType.EventManager) as EventManager;
        eventManager.GetEventTrigger(typeof(StatEventTrigger)).Add(new GlobalEventTrigger.StatEvent((entity, category, oldValue, value) =>
        {
            statbar.UpdateStatBar();
            statbar.UpdateStatText();

            if (category == StatCategory.Health) enemyStatUI.EnemyHpBarUpdate(entity);
        }));
    }

    public void SetQuestUI(Quest quest)
    {
        for (int i = 0; i < questUIObj.Length; i++)
        {
            if (!questUIObj[i].isUsing)
            {
                questUIObj[i].SetQuestUIText(quest);
                return;
            }
        }
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

        InputManager inputManager = ManagerObject.Instance.GetManager(ManagerType.InputManager) as InputManager;
        inputManager.isTimeStop = true;
    }

    public void CloseUI(CanvasGroup canvasGroup)
    {
        Time.timeScale = 1;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        InputManager inputManager = ManagerObject.Instance.GetManager(ManagerType.InputManager) as InputManager;
        inputManager.isTimeStop = false;
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

    public void FadeInOut(bool fadeOut, bool FadeOrLoading, Action endAction = null)
    {
        if (fadeOut)
        {
            StartCoroutine(FadeOut(endAction, FadeOrLoading));
        }
        else
        {
            StartCoroutine(FadeIn(endAction, FadeOrLoading));
        }
    }

    private IEnumerator MakeEnemyDelay(Entity entity, float time)
    {
        if (time <= 0) yield break;
        SpriteRenderer spriteRenderer = entity.GetComponent<SpriteRenderer>();
        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
        GameObject obj = poolManager.GetObject("EnemyDelay");
        EnemyDelayUI enemyDelayUI = obj.GetComponentInChildren<EnemyDelayUI>();
        enemyDelayUI.SetTarget(spriteRenderer);
        enemyDelayUI.Play();
        StartCoroutine(enemyDelayUI.AfterImageInActive(obj, time));
        yield return null;
    }

    private IEnumerator FadeOut(Action endAction, bool FadeOrLoading)
    {
        if (FadeOrLoading)
        {
            fade.transform.GetChild(0).gameObject.SetActive(true);
            fade.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            fade.transform.GetChild(0).gameObject.SetActive(false);
            fade.transform.GetChild(1).gameObject.SetActive(true);
        }

        float alpha = 0f;

        while (true)
        {
            if (alpha < 1f)
            {
                alpha += Time.deltaTime * 1;
            }
            else
            {
                OpenUI(fade, false);
                if (endAction != null)
                    endAction();

                yield break;
            }
            alpha = Mathf.Clamp(alpha, 0, 1);
            fade.alpha = alpha;
            yield return null;
        }
    }

    private IEnumerator FadeIn(Action endAction, bool FadeOrLoading)
    {
        if (FadeOrLoading)
        {
            fade.transform.GetChild(0).gameObject.SetActive(true);
            fade.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            fade.transform.GetChild(0).gameObject.SetActive(false);
            fade.transform.GetChild(1).gameObject.SetActive(true);
        }

        float alpha = 1f;
        while (true)
        {
            if (alpha > 0f)
            {
                alpha -= Time.deltaTime * 1;
            }
            else
            {
                CloseUI(fade);

                yield break;
            }
            alpha = Mathf.Clamp(alpha, 0, 1);
            fade.alpha = alpha;
            yield return null;
        }
    }
}
