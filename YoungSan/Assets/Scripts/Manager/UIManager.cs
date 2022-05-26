using System.Net.Mail;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : Manager
{
    private float playerCurrentHP; 
    private float playerCurrentStamina;
    private float playerMaxHP;
    private float playerMaxStamina;
    private float hp;
    private float stamina;
    private Entity entity;
    [SerializeField]
    private CanvasGroup canvas;

    [SerializeField]
    public Image fade;

    public BossStatUI bossStatbar;
    [HideInInspector] public Statbar statbar;
    [HideInInspector] public Skillinterface skillinterface;
    [HideInInspector] public DieUI dieUI;
    [HideInInspector] public EnemyDelayUI enemyDelayUI;
    public QuestUI questUI;
    public Transform uiCanvas;

    public TimeLineSkipGage timeLineSkipGage;

    (float, float) currentStat;
    (float, float) maxStat;

    //Quest UI

    public GameObject SettingUIObj;
    public GameObject DieUIObj;

    void Awake()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 0)
            Init();
    }

    public void Init()
    {
        uiCanvas.gameObject.SetActive(true);
        statbar = transform.GetComponentInChildren<Statbar>();
        questUI = transform.GetComponentInChildren<QuestUI>();
        //Debug.Log(bossStatbar);
        skillinterface = transform.GetComponentInChildren<Skillinterface>();
        dieUI = transform.GetComponentInChildren<DieUI>();
        timeLineSkipGage = transform.GetComponentInChildren<TimeLineSkipGage>();

        EventManager eventManager = ManagerObject.Instance.GetManager(ManagerType.EventManager) as EventManager;
        eventManager.GetEventTrigger(typeof(StatEventTrigger)).Add(new GlobalEventTrigger.StatEvent((entity, category, oldValue, value) =>
        {
            statbar.UpdateStatBar();
            statbar.UpdateStatText();

            if(category == StatCategory.Health) EnemyHpBarUpdate(entity);
        }));
    }

    public (float, float) UpdateMaxStat()
    {
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        Entity entity = gameManager.Player.GetComponent<Entity>();
        playerMaxHP = entity.clone.GetMaxStat(StatCategory.Health);
        playerMaxStamina = entity.clone.GetMaxStat(StatCategory.Stamina);
        return (playerMaxHP,playerMaxStamina);
    }
    public float UpdateStat(Entity entity)
    {
        return entity.clone.GetStat(StatCategory.Health);
    }
    public void OnQuestUI(Quest quest)
    {
        questUI.SetQuestUIText(quest);
    }

    //public void GetEnemyHpBar(Entity entity)
    //{
    //    GameObject hpBar;
    //    if (entity.transform.Find("TestCanvas(Clone)") == null)
    //    {
    //        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
    //        hpBar = poolManager.GetUIObject("TestCanvas");
    //        hpBar.transform.SetParent(entity.transform);


    //    }
    //    else
    //    {
    //        hpBar = entity.transform.Find("TestCanvas(Clone)").gameObject;
    //    }

    //    EnemyStatUi enemyUi = hpBar.GetComponentInChildren<EnemyStatUi>();

    //    enemyUi.entity = entity;
    //    enemyUi.SetHpBarValue(entity.clone.GetMaxStat(StatCategory.Health), entity.clone.GetStat(StatCategory.Health));
    //}
    
	public void GetEnemyHpBar(Entity entity)
	{
            PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
            GameObject hpBar = poolManager.GetObject("EnemyHp");
            EnemyStatUI enemyUi = hpBar.GetComponentInChildren<EnemyStatUI>();
            enemyUi.entity = entity;

            enemyUi.SetPos();
            enemyUi.SetHpBarValue(entity.clone.GetMaxStat(StatCategory.Health), entity.clone.GetStat(StatCategory.Health));
    }

    public void EnemyHpBarUpdate(Entity entity)
    {
        if (entity.gameObject.tag == "Enemy")
        {
            Transform enemyHp = entity.gameObject.transform.Find("EnemyHp(Clone)");
            if (enemyHp != null)
            {
                EnemyStatUI enemyUi = enemyHp.GetComponentInChildren<EnemyStatUI>();

                enemyUi.entity = entity;
                enemyUi.SetHpBarValue(entity.clone.GetMaxStat(StatCategory.Health), entity.clone.GetStat(StatCategory.Health));
            }
            else
            {
                GetEnemyHpBar(entity);
            }
        }
        else if(entity.gameObject.tag == "Boss")
        {
            bossStatbar.entity = entity;
            bossStatbar.UpdateStatBar(entity.clone.GetStat(StatCategory.Health));
        }
    }
    
    public void UISetActive(bool active)
    {
        if (active)
            canvas.alpha = 1;
        else
            canvas.alpha = 0;
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

    private void TimeLineSkip()
    {
        //미래에 누군가에게 이거를 넘겨요 
    }

    public void OpenDiePanel()
    {
        DieUIObj.SetActive(true);
    }
}
