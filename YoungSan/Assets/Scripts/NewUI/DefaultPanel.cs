using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefaultPanel : MonoBehaviour
{
    public Image[] skillImages;
    public Text[] contents;
    public GameObject[] covers;
    public Sprite defaultAttackIcon;
    public Sprite skillBasicIcon;

    void OnEnable()
    {
        SetSkill();
    }

    void SetSkill()
    {
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;

        SkillSet skillSet = gameManager.Player.GetComponent<EntityEvent>().skillSet;
        Entity entity = skillSet.entity;

        for (int i = 0; i < 4; i++)
        {
            SetContent(i, skillSet.skillDatas.ContainsKey((EventCategory)(i + 1)), entity, skillSet);
        }
    }

    void SetContent(int index, bool active, Entity entity, SkillSet skillSet)
    {
        if (active)
        {
            if (index == 0)
            {
                skillImages[index].sprite = defaultAttackIcon;
            }
            else
            {
                skillImages[index].sprite = entity.entityData.skillIcon[index - 1];
            }
            if (!(entity.entityData.skillContents == null || entity.entityData.skillContents.Length == 0))
                contents[index].text = InterpretContent((EventCategory)(index - 1), entity.entityData.skillContents[index].text, skillSet);
            covers[index].SetActive(false);
        }
        else
        {
            skillImages[index].sprite = skillBasicIcon;
            contents[index].text = string.Empty;
            covers[index].SetActive(true);
        }
    }

    string InterpretContent(EventCategory category, string text, SkillSet skillSet)
    {
        string result = text;
        for (int i = 0; i < skillSet.skillDatas[category].Length; i++)
        {
            for (int j = 0; j < skillSet.skillDatas[category][i].skillDamageForms.Length; j++)
            {
                result = result.Replace(string.Concat("{Damage", i, j, "}"), skillSet.skillDatas[category][i].CalculateSkillDamage().ToString());
            }
            result = result.Replace(string.Concat("{Stamina", i, "}"), skillSet.skillDatas[category][i].CalculateUseStamina().ToString());
        }

        result = result.Replace("<D", "<color=red>");
        result = result.Replace("<S", "<color=blue>");
        result = result.Replace(">", "</color>");

        return result;
    }
}
