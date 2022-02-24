using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManger : Manager
{
    private float playerCurrentHP; 
    private float playerCurrentStamina;
    private float playerMaxHP;
    private float playerMaxStamina;
    private Entity entity;

    void Start()
    {
        
    }

    void Update()
    {
        var currentStat = UpdateCurrentStat();
        Debug.Log("CurrentHP :" + currentStat.Item1 + "\t CurrentStamina : " + currentStat.Item2);
        var maxStat = UpdateMaxStat();
        Debug.Log("MaxHP :" + maxStat.Item1 + "\t MaxStamina : " + maxStat.Item2);
    }

    public (float, float) UpdateMaxStat()
    {
        entity = GameObject.FindWithTag("Player").GetComponent<Entity>();
        playerMaxHP = entity.clone.GetMaxStat(StatCategory.Health);
        playerMaxStamina = entity.clone.GetMaxStat(StatCategory.Stamina);
        return (playerMaxHP,playerMaxStamina);
    }

    public (float, float) UpdateCurrentStat()
    {
        entity = GameObject.FindWithTag("Player").GetComponent<Entity>();
        playerCurrentHP = entity.clone.GetStat(StatCategory.Health);
        playerCurrentStamina = entity.clone.GetStat(StatCategory.Stamina);
        return (playerCurrentHP, playerCurrentStamina);
    }
}
