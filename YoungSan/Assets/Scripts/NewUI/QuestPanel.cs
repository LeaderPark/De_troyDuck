using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : MonoBehaviour
{

    [SerializeField] private GameObject uiPrefab;

    [SerializeField] private Text questName;
    [SerializeField] private Text questContext;

    [SerializeField] private GameObject proceedingList;
    [SerializeField] private GameObject completedList;

    private List<GameObject> objList = new List<GameObject>();

    private Button interactibleOffBtn;
    private bool isEnabled;

    void OnEnable()
    {
        SetQuestUI();
    }

    public void SetQuestUI()
    {
        QuestManager questManager = ManagerObject.Instance.GetManager(ManagerType.QuestManager) as QuestManager;
        foreach (Quest item in questManager.proceedingQuests.Values)
        {
            CreateQuestUI(item, proceedingList.transform);
        }
        if (questManager.haveSort)
        {
            questManager.completeQuestIds = questManager.completedQuests.Keys.Cast<int>().ToList();
            questManager.completeQuestIds.Sort();
            questManager.completeQuestIds.Reverse();
            Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            questManager.haveSort = false;
        }
        foreach (int item in questManager.completeQuestIds)
        {
            CreateQuestUI(questManager.GetQuest(item), completedList.transform);
        }
    }
    public void CreateQuestUI(Quest quest, Transform content)
    {
        QuestManager questManager = ManagerObject.Instance.GetManager(ManagerType.QuestManager) as QuestManager;

        //Debug.Log(quest + " " + content);
        GameObject questUI = GetObject(content);
        Button button = questUI.GetComponent<Button>();
        Text text = button.GetComponentInChildren<Text>();
        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(() =>
        {
            if (interactibleOffBtn != null) interactibleOffBtn.interactable = true;
            questName.text = quest.title;
            questContext.text = quest.context;
            button.interactable = false;
            interactibleOffBtn = button;
        });

        text.text = /*quest.title + " " + */quest.name;
    }
    public GameObject GetObject(Transform content)
    {
        foreach (GameObject item in objList)
        {
            if (item == null)
            {
                continue;
            }
            // �̷��� ������
            if (!item.activeSelf)
            {
                item.SetActive(true);
                item.transform.parent = content;
                return item;
            }
        }

        objList.Add(GameObject.Instantiate(uiPrefab, content));
        return objList[objList.Count - 1];
    }
}
