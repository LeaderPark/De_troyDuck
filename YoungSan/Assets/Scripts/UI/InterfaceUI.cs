using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public GameObject interfaceUI;
    public GameObject questAllUI;
    public GameObject questUIPrefab;

    public Text proceedingquestUIText;
    public Text completequestUIText;

    private bool isEnabled;


    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isEnabled)
            {
                UIManager uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
                uIManager.OpenUI(canvasGroup, true);
                isEnabled = true;
                SetQuestUI();
            }
            else
            {
                UIManager uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
                uIManager.CloseUI(canvasGroup);
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
        proceedingquestUIText.text = "";
        completequestUIText.text = "";
        QuestManager questManager = ManagerObject.Instance.GetManager(ManagerType.QuestManager) as QuestManager;
        foreach (var item in questManager.proceedingQuests.Values)
        {
            proceedingquestUIText.text += "진행중인 퀘스트" + item + "\n";
        }

        foreach (var item in questManager.completedQuests.Values)
        {
            completequestUIText.text += "클리어한 퀘스트" + item + "\n";
        }
    }
}
