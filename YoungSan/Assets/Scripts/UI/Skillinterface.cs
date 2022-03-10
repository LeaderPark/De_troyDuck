using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skillinterface : MonoBehaviour
{
    public Text text_CoolTime; 
    public Image image_fill; 
    public float time_coolTime = 2; 
    private float time_current;
    private bool isEnded = true; 


    private void Update()
    {
        if (isEnded)
            return;
        Check_CoolTime();
    }

    void Check_CoolTime() 
    {
        time_current += Time.deltaTime; 
        if(time_current < time_coolTime) 
        {
            Set_FillAmount(time_current); 
        }
        else if(!isEnded)
        {
            End_CoolTime(); 
        }
    }

    void End_CoolTime()
    {
        Set_FillAmount(time_coolTime);
        isEnded = true; 
        //text_CoolTime.gameObject.SetActive(false); 
        text_CoolTime.text = "M2";
    }

    public void Trigger_Skill() 
    {
        if (!isEnded) return; //아직 쿨타임이면 안한다.

        Reset_CoolTime(); // 쿨타임을 돌린다.
    }

    void Reset_CoolTime()
    {
        text_CoolTime.gameObject.SetActive(true); 
        time_current = 0;
        Set_FillAmount(0);
        isEnded = false;
    }

    void Set_FillAmount(float value)
    {
        image_fill.fillAmount = value / time_coolTime;
        text_CoolTime.text = string.Format("{0}",value.ToString("0.0"));
    }
}
