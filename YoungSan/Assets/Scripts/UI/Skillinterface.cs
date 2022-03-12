using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skillinterface : MonoBehaviour
{
    public Text[] text_CoolTime;
    public Image[] image_fill;
    public GameObject[] activation_image;
    public List<float> skillCoolTimes = new List<float>();
    public List<float> skillCoolTimeList = new List<float>();
    public List<SkillData> skillDataList = new List<SkillData>();

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
        skillDataList.Clear();
        skillCoolTimeList.Clear();

        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        skillSet = gameManager.Player.GetComponentInChildren<SkillSet>();
        skillDataList.Add(null);
        skillCoolTimeList.Add(0);
        skillDataList.Add(null);
        skillCoolTimeList.Add(gameManager.Player.GetComponent<Player>().dashCoolTime);
        for (int i = 0; i < skillSet.skillDatas.Length; i++)
        {
            if(skillSet.skillDatas[i].coolTime != 0)
            {
                skillDataList.Add(skillSet.skillDatas[i]);
                skillCoolTimeList.Add(skillSet.skillDatas[i].coolTime);
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
        for (int i = 0; i < skillDataList.Count; i++)
        {
            //추후 여기다가 스킬 이미지 갔다가 넣는거 만들면 됨 미래의 친구ssssss 
            activation_image[i].SetActive(false);
        }
    }
    public void CoolDown(int index)
    {
        if (skillCoolTimeList.Count - 1 >= index)
        {
            StartCoroutine(Cool(index, skillCoolTimeList[index]));

        }
        //skillCoolTimes.Add(skillCoolTimeList[index]);
    }
    IEnumerator Cool(int index,float cool)
    {
        float time = cool;
		while (true)
		{
            time -= Time.deltaTime;
            Set_FillAmount(time,index);
            if (time <= 0)
            {
                //skillCoolTimes.RemoveAt(index);
                yield break;
            }
            yield return null;

        }
    }
    private void Update()
    {
    }

    // private void Check_CoolTime()
    // {
    //     time_current = Time.time - time_start;
    //     if (time_current < time_cooltime)
    //     {
    //         Set_FillAmount(time_cooltime - time_current);
    //     }
    //     else if (!isEnded)
    //     {
    //         End_CoolTime();
    //     }
    // }

    // private void End_CoolTime()
    // {
    //     Set_FillAmount(0);
    //     isEnded = true;
    //     text_CoolTime.gameObject.SetActive(false);
    //     Debug.Log("Skills Available!");
    // }

    // private void Trigger_Skill()
    // {
    //     if(!isEnded)
    //     {
    //         return;
    //     }

    //     Reset_CoolTime();
    // }

    // private void Reset_CoolTime()
    // {
    //     text_CoolTime.gameObject.SetActive(true);
    //     time_current = time_cooltime;
    //     time_start = Time.time;
    //     Set_FillAmount(time_cooltime);
    //     isEnded = false;
    // }
    public void Set_FillAmount(float _value,int index)
    {
        if (skillCoolTimeList.Count - 1 >= index)
        {
            image_fill[index].fillAmount = _value / skillCoolTimeList[index];
            string txt = _value.ToString("0.0");
            text_CoolTime[index].text = txt;
        }
        else
        {
            image_fill[index].fillAmount = 0;
            text_CoolTime[index].text = "0.0";
        }
    }
}
