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

        if (skillSet.skillDatas.ContainsKey(EventCategory.DefaultAttack))
        {
            skillImages[0].sprite = defaultAttackIcon;
            covers[0].SetActive(false);
        }
        else
        {
            covers[0].SetActive(true);
        }
        if (skillSet.skillDatas.ContainsKey(EventCategory.Skill1))
        {
            skillImages[1].sprite = entity.entityData.skillIcon[0];
            covers[1].SetActive(false);
        }
        else
        {
            covers[1].SetActive(true);
        }
        if (skillSet.skillDatas.ContainsKey(EventCategory.Skill2))
        {
            covers[2].SetActive(false);
        }
        else
        {
            covers[2].SetActive(true);
        }
        if (skillSet.skillDatas.ContainsKey(EventCategory.Skill3))
        {
            covers[3].SetActive(false);
        }
        else
        {
            covers[3].SetActive(true);
        }
    }

    void SetContent(int index, bool active, Entity entity)
    {

    }
}
