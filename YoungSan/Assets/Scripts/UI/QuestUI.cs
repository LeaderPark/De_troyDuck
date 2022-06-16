using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    //Quest UI
    public Text questTitle;
    public Text questContext;
    public Text questValue;
    public Text questId;
    public GameObject clearImg;

    public RectTransform questUI;
    public GameObject questUIObj;
    public GameObject questUIParent;
    public Quest quest1;

    public bool isQuestUI = false;

    void Start()
    {
        //SetQuestUIText(quest1);
    }

    // Update is called once per frame
    void Update()
    {
        if (isQuestUI)
        {
            questUI.anchoredPosition = Vector2.Lerp(questUI.anchoredPosition, new Vector2(960, 540), Time.deltaTime * 2f);
        }
        else
        {
            questUI.anchoredPosition = Vector2.Lerp(questUI.anchoredPosition, new Vector2(1660, 540), Time.deltaTime * 2f);
        }
    }

    IEnumerator OpenQuestUI()
    {
        isQuestUI = true;
        yield return new WaitForSeconds(3f);
        questUI.anchoredPosition = new Vector2(960, 540);
        isQuestUI = false;
        yield return new WaitForSeconds(3f);
        questUI.anchoredPosition = new Vector2(1660, 540);
    }

    public void SetQuestUIText(Quest msg)
    {
        QuestManager questManager = ManagerObject.Instance.GetManager(ManagerType.QuestManager) as QuestManager;
        clearImg.SetActive(questManager.IsComplete(msg.questId));
        questId.text = "Quest " + msg.questId;
        questTitle.text = msg.title;
        questContext.text = msg.context;
        StartCoroutine(OpenQuestUI());
    }

    public void SetQuestUI()
    {
        QuestManager questManager = ManagerObject.Instance.GetManager(ManagerType.QuestManager) as QuestManager;
        foreach (int item in questManager.proceedingQuests.Keys)
        {
            Quest quest = questManager.proceedingQuests[item] as Quest;
            GameObject qu = Instantiate(questUIObj, transform.position, Quaternion.identity, questUIParent.transform)
                ;
            QuestUI questUI = qu.GetComponent<QuestUI>();
            questUI.questTitle.text = quest.title;
            questUI.questContext.text = quest.context;
        }
    }

}
