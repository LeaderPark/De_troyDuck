using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Skillinterface : MonoBehaviour
{
    public Text[] text_CoolTime;
    public Image[] image_fill;
    public Image[] skillIcons;
    public GameObject[] activation_image;

    private SkillSet skillSet;
    void Start()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 0)
        {
            Init();
        }
    }
    public void Init()
    {
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        skillSet = gameManager.Player.GetComponentInChildren<SkillSet>();
        for (int i = 0; i < transform.childCount; i++)
        {
            image_fill[i].type = Image.Type.Filled;
            image_fill[i].fillMethod = Image.FillMethod.Radial360;
            image_fill[i].fillOrigin = (int)Image.Origin360.Top;
            image_fill[i].fillClockwise = false;
        }
        SetSkillDatas();
    }

    public void SetSkillDatas()
    {
        StopAllCoroutines();
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        skillSet = gameManager.Player.GetComponentInChildren<SkillSet>();
        for (int i = 2; i < 5; i++)
        {
            EventCategory category = (EventCategory)i;

            Set_FillAmount(0, 0, (int)category - 2);
            if (skillSet.skillCoolTimes.ContainsKey(category))
            {
                Set_FillAmount(skillSet.skillCoolTimes[category][skillSet.skillDatas[category][skillSet.skillStackAmount[category]].targetIndex], skillSet.skillDatas[category][skillSet.skillStackAmount[category]].coolTime, (int)category - 2);
            }
        }
        SkillUIActivation(gameManager.Player.GetComponent<Entity>());
    }

    public void SkillUIActivation(Entity entity)
    {
        for (int i = 0; i < activation_image.Length; i++)
        {
            activation_image[i].SetActive(true);
        }
        for (int i = 0; i < skillSet.skillDatas.Count - 1; i++)
        {
            //추후 여기다가 스킬 이미지 갔다가 넣는거 만들면 됨 미래의 친구ssssss 
            //미래의 친구가 나였던거냐고 wwwwwwwwwww
            if (entity.entityData.skillIcon.Length >= i)
                skillIcons[i].sprite = entity.entityData.skillIcon[i];
            activation_image[i].SetActive(false);
        }

    }

    public void CoolDown(EventCategory eventCategory, int index)
    {
        if (skillSet.skillCoolTimes.ContainsKey(eventCategory))
        {
            //가만히 기다리면 그냥 msing뜸
            StartCoroutine(Cool(eventCategory, skillSet.skillCoolTimes[eventCategory][index], index));
        }
    }
    IEnumerator Cool(EventCategory eventCategory, float cool, int index)
    {
        if (eventCategory == EventCategory.DefaultAttack) yield break;
        float time = cool;
        //Debug.Log(time);
        while (true)
        {
            time -= Time.deltaTime;
            Set_FillAmount(time, cool, (int)eventCategory - 2);
            if (time <= 0)
            {
                //skillCoolTimes.RemoveAt(index);
                yield break;
            }
            yield return null;

        }
    }

    public void Set_FillAmount(float cool, float maxCool, int index)
    {
        string txt;

        image_fill[index].fillAmount = cool / maxCool;

        if (cool > 0)
        {
            txt = cool.ToString("0.0");
        }
        else
        {
            txt = Enum.GetName(typeof(KeyType), index);
            image_fill[index].fillAmount = 0;
        }
        text_CoolTime[index].text = txt;
    }
}
public enum KeyType
{
    E,
    R,
    F
}