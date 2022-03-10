using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skillinterface : MonoBehaviour
{
    public Text[] text_CoolTime;
    public Image[] image_fill;
    public GameObject[] activation_image;
    public float[] skillCoolTimes {get; set;}
    private bool[] skillCools;
    public List<float> skillCoolTimeList = new List<float>();
    public List<SkillData> skillDataList = new List<SkillData>();

    private SkillSet skillSet;
    void Start()
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
        for (int i = 0; i < skillDataList.Count + 2; i++)
        {
            //추후 여기다가 스킬 이미지 갔다가 넣는거 만들면 됨 미래의 친구 
            activation_image[i].SetActive(false);
        }
    }
}
