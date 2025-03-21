using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    public Text questTitle;
    public Text questContext;
    public Text questValue;
    public Text questId;
    public GameObject clearImg;

    public RectTransform rectTransform;
    public Vector2 startPostion;

    public bool isQuestUI = false;
    public bool isUsing = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPostion = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);
    }

    void Update()
    {
        if (isQuestUI)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, new Vector2(startPostion.x - 700, rectTransform.anchoredPosition.y), Time.deltaTime * 2f);
        }
        else
        {
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, new Vector2(startPostion.x, rectTransform.anchoredPosition.y), Time.deltaTime * 2f);
        }
    }

    IEnumerator OpenQuestUI()
    {
        isQuestUI = true;
        isUsing = true;
        yield return new WaitForSeconds(3f);
        isQuestUI = false;
        yield return new WaitForSeconds(1.6f);
        isUsing = false;
        if (!isQuestUI)
            rectTransform.anchoredPosition = new Vector2(startPostion.x, startPostion.y);
    }

    public void SetQuestUIText(Quest msg)
    {
        QuestManager questManager = ManagerObject.Instance.GetManager(ManagerType.QuestManager) as QuestManager;
        clearImg.SetActive(questManager.IsComplete(msg.questId));
        //questId.text = "Quest " + msg.questId;
        questId.text = "";
        questTitle.text = msg.title;
        questContext.text = msg.context;
        StartCoroutine(OpenQuestUI());
    }
}
