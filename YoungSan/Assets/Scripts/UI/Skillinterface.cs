using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skillinterface : MonoBehaviour
{
    public Text[] text_CoolTime;
    public Image[] image_fill;
    public GameObject[] activation_image;

    private SkillSet skillSet;
    void Awake()
    {
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        skillSet = gameManager.Player.GetComponentInChildren<SkillSet>();
        SetSkillDatas();
        Init_UI();
    }

    private void Init_UI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            image_fill[i].type = Image.Type.Filled;
            image_fill[i].fillMethod = Image.FillMethod.Radial360;
            image_fill[i].fillOrigin = (int)Image.Origin360.Top;
            image_fill[i].fillClockwise = false;
            Debug.Log("setting UI" + i);
        }
    }

    public void SetSkillDatas()
    {

        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        skillSet = gameManager.Player.GetComponentInChildren<SkillSet>();
        StopAllCoroutines();
	    for (int j = 0; j < Enum.GetValues(typeof(EventCategory)).Length; j++)
		{
            Set_FillAmount(0, 0, j, Enum.GetName(typeof(KeyType),j));


            if (skillSet.skillCoolTimes.ContainsKey((EventCategory)Enum.GetValues(typeof(EventCategory)).GetValue(j)))
            {
                for (int i = 0; i < skillSet.skillCoolTimes[(EventCategory)Enum.GetValues(typeof(EventCategory)).GetValue(j)].Length; i++)
                {
                    CoolDown((EventCategory)Enum.GetValues(typeof(EventCategory)).GetValue(j), i);

                }
            }

        }
        SkillUIActivation();


    }

    public void SkillUIActivation()
    {
        for (int i = 0; i < activation_image.Length; i++)
        {
            activation_image[i].SetActive(true);
        }
        for (int i = 0; i < skillSet.skillDatas.Count + 1; i++)
        {
            //추후 여기다가 스킬 이미지 갔다가 넣는거 만들면 됨 미래의 친구ssssss 
            activation_image[i].SetActive(false);
        }
    }
    public void CoolDown(EventCategory eventCategory, int index)
    {
        if(skillSet.skillCoolTimes.ContainsKey(eventCategory))
        {
            StartCoroutine(Cool(eventCategory, skillSet.skillCoolTimes[eventCategory][index], index));
        }
    }
    IEnumerator Cool(EventCategory eventCategory, float cool, int index)
    {
        float time = cool;
        string keyName = text_CoolTime[index].text;
        Debug.Log(time);
        while (true)
        {
            time -= Time.deltaTime;
            Set_FillAmount(time, cool, index,keyName);
            if (time <= 0)
            {
                //skillCoolTimes.RemoveAt(index);
                yield break;
            }
            yield return null;

        }
    }

    public void Set_FillAmount(float cool, float maxCool, int index,string keyName)
    {
        string txt;


        image_fill[index].fillAmount = cool / maxCool;

        if (cool > 0)
        {
            txt = cool.ToString("0.0");
        }
        else
        {
            txt = keyName;
            image_fill[index].fillAmount = 0;
        }
        text_CoolTime[index].text = txt;
    }
}
public enum KeyType
{
    M1,
    M2,
    E,
    R,
    F
}