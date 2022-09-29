using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public GameObject interfaceUI;
    public GameObject questAllUI;
    public GameObject questUIPrefab;

    [SerializeField] private GameObject uiPrefab;

    [SerializeField] private Text questName;
    [SerializeField] private Text questContext;

    [SerializeField] private GameObject proceedingList;
    [SerializeField] private GameObject completedList;

    private List<GameObject> objList = new List<GameObject>();

    private Button interactibleOffBtn;
    private bool isEnabled;


    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        UIManager uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isEnabled && uIManager.settingUI.GetComponent<CanvasGroup>().alpha == 0 && uIManager.loadingUI.transform.parent.GetComponent<CanvasGroup>().alpha == 0 && Time.timeScale == 1)
            {
                uIManager.OpenUI(canvasGroup, true);
                isEnabled = true;
                questName.text = "--";
                questContext.text = "--";
                SetQuestUI();
            }
            else if (isEnabled)
            {
                uIManager.CloseUI(canvasGroup);
                foreach (var item in objList)
                {
                    item.SetActive(false);
                }
                if (interactibleOffBtn != null)
                    interactibleOffBtn.interactable = true;
                isEnabled = false;
            }
        }
    }

    public void SetQuestAllUI()
    {
        GameObject go = Instantiate(questUIPrefab, transform.position, Quaternion.identity);
        go.transform.SetParent(questAllUI.transform);
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

        text.text = quest.title + " " + quest.name;
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
