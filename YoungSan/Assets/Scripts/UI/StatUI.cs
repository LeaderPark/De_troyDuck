using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    public Text health;
    public Text currentHealth;
    public Text attack;
    public Text speed;
    public Text stamina;
    public Text currentStamina;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetStatText();
    }

    void SetStatText()
    {
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        Entity entity = gameManager.Player.GetComponent<Entity>();
        health.text = "Maxhealth : " + entity.clone.GetMaxStat(StatCategory.Health);
        currentHealth.text = "Health : " +entity.clone.GetStat(StatCategory.Health);
        stamina.text = "Maxstamina : " +entity.clone.GetMaxStat(StatCategory.Stamina);
        currentStamina.text = "Stamina : " +entity.clone.GetStat(StatCategory.Stamina);
        attack.text = "attack : " +entity.clone.GetStat(StatCategory.Attack);
        speed.text = "speed : " +entity.clone.GetStat(StatCategory.Speed);
    }
}
