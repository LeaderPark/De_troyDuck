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

    (float, float) currentStat;
    (float, float) maxStat;

    void Start()
    {
        statbar = transform.GetComponentInChildren<Statbar>();
    }

    public (float, float) UpdateMaxStat()
    {
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        Entity entity = gameManager.Player.GetComponent<Entity>();
        playerMaxHP = entity.clone.GetMaxStat(StatCategory.Health);
        playerMaxStamina = entity.clone.GetMaxStat(StatCategory.Stamina);
        return (playerMaxHP,playerMaxStamina);
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
