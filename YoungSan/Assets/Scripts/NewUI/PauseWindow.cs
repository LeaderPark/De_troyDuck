using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseWindow : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    bool activated;

    public RectTransform selectPanel;
    public RectTransform mainPanel;

    public Button[] selectButtons;

    bool opened;

    PauseSelection selected;

    void Start()
    {
        foreach (PauseSelection pauseSelection in typeof(PauseSelection).GetEnumValues())
        {
            if (pauseSelection == PauseSelection.None) continue;

            selectButtons[(int)pauseSelection - 1].onClick.AddListener(() => { SelectMain(pauseSelection); });
        }
    }

    void Update()
    {
        UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!activated)
            {
                if (Time.timeScale == 1)
                {
                    uiManager.OpenUI(canvasGroup, true);
                    Open();
                }
            }
            else
            {
                uiManager.CloseUI(canvasGroup);
                Close();
                activated = false;
            }
        }

        if (opened)
        {
            for (int i = 0; i < selectButtons.Length; i++)
            {
                Button selectButton = selectButtons[i];
                var rtrm = selectButton.GetComponent<RectTransform>();
                Rect rect = rtrm.rect;
                rect.position += (Vector2)rtrm.position + Vector2.up * -20;
                rect.size += Vector2.up * 40;

                if ((int)selected == i + 1)
                {
                    ButtonSize(selectButton, new Vector3(1f, 1f, 1f));
                }
                if (rect.Contains(Input.mousePosition))
                {
                    if ((int)selected != i + 1)
                    {
                        ButtonSize(selectButton, new Vector3(1.34f, 1.34f, 1f));
                    }
                }
                else
                {
                    ButtonSize(selectButton, new Vector3(1f, 1f, 1f));
                }
            }
        }
    }

    void ButtonSize(Button button, Vector3 size)
    {
        Vector3 target = size;
        Vector3 current = button.transform.localScale;

        const float speed = 10f;

        button.transform.localScale = new Vector3(Mathf.Clamp(current.x + (target.x - current.x) * Time.unscaledDeltaTime * speed, 1, 1.34f), Mathf.Clamp(current.y + (target.y - current.y) * Time.unscaledDeltaTime * speed, 1, 1.34f), 1);
    }

    void Open()
    {
        StartCoroutine(OpenPanel(selectPanel));
        StartCoroutine(OpenPanel(mainPanel, () =>
        {
            activated = true;
            opened = true;
        }));
    }

    IEnumerator OpenPanel(RectTransform trm, System.Action action = null)
    {
        float timeStack = 0;
        const float time = 0.2f;

        Vector2 startPoint = trm.anchoredPosition;

        while (timeStack < time)
        {
            timeStack += Time.unscaledDeltaTime;

            trm.anchoredPosition = Vector2.Lerp(startPoint, Vector2.zero, timeStack / time);

            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }

        trm.anchoredPosition = Vector2.zero;

        action?.Invoke();
    }

    void Close()
    {
        selectPanel.anchoredPosition = new Vector2(-600, 0);
        mainPanel.anchoredPosition = new Vector2(0, -1200);
        opened = false;
        if (selected != PauseSelection.None) selectButtons[(int)selected - 1].transform.localScale = Vector3.one;
        UnselectMain(selected);
    }

    public void SelectMain(PauseSelection select)
    {
        if (selected == select)
        {
            UnselectMain(selected);
            UnselectProcedure(select);
            return;
        }
        UnselectMain(selected);
        selected = select;
        selectButtons[(int)select - 1].GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        SelectProcedure(select);
    }

    public void UnselectMain(PauseSelection select)
    {
        if (select == PauseSelection.None) return;
        selected = PauseSelection.None;
        selectButtons[(int)select - 1].GetComponent<Image>().color = Color.white;
    }

    public void SelectProcedure(PauseSelection select)
    {
        UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        switch (select)
        {
            case PauseSelection.Resume:
                uiManager.CloseUI(canvasGroup);
                Close();
                activated = false;
                break;
            case PauseSelection.Setting:
                break;
            case PauseSelection.Key:
                break;
            case PauseSelection.Quest:
                break;
            case PauseSelection.Quit:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
# else
                Application.Quit();
#endif
                break;
        }
    }

    public void UnselectProcedure(PauseSelection select)
    {
        switch (select)
        {
            case PauseSelection.Setting:
                break;
            case PauseSelection.Key:
                break;
            case PauseSelection.Quest:
                break;
        }
    }

    public enum PauseSelection
    {
        None,
        Resume,
        Setting,
        Key,
        Quest,
        Quit,
    }
}
