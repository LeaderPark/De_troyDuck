using System.Net.Mail;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Manager
{
    private float playerCurrentHP; 
    private float playerCurrentStamina;
    private float playerMaxHP;
    private float playerMaxStamina;
    private float hp;
    private float stamina;
    private Entity entity;
    public Statbar statbar;
    public Skillinterface skillinterface;

    (float, float) currentStat;
    (float, float) maxStat;

    void Start()
    {
        statbar = transform.GetComponentInChildren<Statbar>();
        skillinterface = transform.GetComponentInChildren<Skillinterface>();
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
            GameObject hpBar = poolManager.GetUIObject("TestCanvas");
            EnemyStatUi enemyUi = hpBar.GetComponentInChildren<EnemyStatUi>();

            enemyUi.entity = entity;
            enemyUi.SetHpBarValue(entity.clone.GetMaxStat(StatCategory.Health), entity.clone.GetStat(StatCategory.Health));
            enemyUi.SetPos();

    }
    public void EnemyHpBarUpdate(Entity entity)
    {
        if (entity.gameObject.tag != "Player")
        {
            Canvas canvas = entity.GetComponentInChildren<Canvas>();
            if (canvas != null)
            {
            Debug.Log(canvas);
                EnemyStatUi enemyUi = canvas.GetComponentInChildren<EnemyStatUi>();

                enemyUi.entity = entity;
                enemyUi.SetHpBarValue(entity.clone.GetMaxStat(StatCategory.Health), entity.clone.GetStat(StatCategory.Health));
            }
            else
            {
                GetEnemyHpBar(entity);
            }
        }
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
}
