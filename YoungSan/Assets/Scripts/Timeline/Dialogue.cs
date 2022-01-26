using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    TimelineController timelineController;

    [SerializeField] private RectTransform talkBoxTrm;
    private Text talkBoxTxt;
    private Text fakeTalkBoxTxt;

    public GameObject talker;
    private Entity talkerEntity;

    private bool dialoguePlayCheck;
    private bool wait = false;
    public bool dialogueEnd = false;
    

    Vector2 padding = new Vector2(30, 30);

    List<Dictionary<string, object>> data;
    private int lineIdx = 0;
    private int maxLineIdx = 0;


    // Start is called before the first frame update
    private void Awake()
    {
        timelineController = GetComponent<TimelineController>();
        talkBoxTxt = talkBoxTrm.gameObject.transform.Find("text").gameObject.GetComponent<Text>();
        fakeTalkBoxTxt = talkBoxTrm.gameObject.transform.Find("fakeText").gameObject.GetComponent<Text>();
    }
    void Start()
    {
        talkBoxTrm.gameObject.SetActive(false);
        fakeTalkBoxTxt.gameObject.SetActive(false);

    }
    private void Update()
    {
        talkBoxTrm.transform.position = Camera.main.WorldToScreenPoint(talker.transform.position + new Vector3(0, 1));
        if (dialogueEnd && Input.anyKeyDown)
        {
            NextTalkCheck();
        }
    }
    private void NextTalkCheck()
    {
        lineIdx++;
        dialogueEnd = false;
        if (lineIdx <= maxLineIdx)
        {
            EndText();
            NextTalk();
        }
        else
        {
            EndText();
            if(wait)
            timelineController.StartTimeline();

        }
    }
    public void StartTalk(List<Dictionary<string, object>> dialogueDataList,bool _wait)
    {
        dialogueEnd = false;
        data = dialogueDataList;
        lineIdx = 0;
        lineIdx++;
        maxLineIdx = int.Parse(data[data.Count - 1]["Line"].ToString());
        wait = _wait;
        NextTalk();
    }
    private void NextTalk()
    {
        List<Dictionary<string, object>> dialogueDataList = data.FindAll(x => int.Parse(x["Line"].ToString()) == lineIdx);

        string charName = data.Find(x => int.Parse(x["Line"].ToString()) == lineIdx)["Char"].ToString();
        List<string> dialougeList = new List<string>();
        List<float> dialougeDelayList = new List<float>();
        List<string> dialougeAnimationList = new List<string>();


        foreach (var item in dialogueDataList)
		{
            dialougeList.Add(item["Dialogue"].ToString());
            dialougeDelayList.Add(float.Parse(item["Delay"].ToString()));
            dialougeAnimationList.Add(item["Animation"].ToString());
        }
        TalkerSet(charName);
        StartCoroutine(ReadLineText(charName, dialougeList, dialougeDelayList, dialougeAnimationList));
    }

    private void TalkerSet(string talkerName)
    {
       talker = GameObject.Find(talkerName);
       talkerEntity = talker.GetComponent<Entity>();
    }
    private void AnimationSet(string animationName)
    {
        talkerEntity?.GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[] { animationName });
    }

    IEnumerator ReadLineText(string talkerName,List<string>dialogueList,List<float> delayLsit,List<string> dialougeAnimationList)
    {
        talkBoxTxt.text = "";
        fakeTalkBoxTxt.text = "";

        for (int i = 0; i < dialogueList.Count; i++)
        {
            fakeTalkBoxTxt.text += dialogueList[i];
        }

        float x = fakeTalkBoxTxt.preferredWidth;
        float y = fakeTalkBoxTxt.preferredHeight;
        talkBoxTrm.sizeDelta = new Vector2(x, y) + padding;

        for (int i = 0; i < dialogueList.Count; i++)
		{
            dialoguePlayCheck = true;
            AnimationSet(dialougeAnimationList[i]);
            StartCoroutine(TypingText(dialogueList[i]));

			while (!dialoguePlayCheck)
			{
				yield return null;
			}
            //print(delayLsit[i] + Time.deltaTime);
			yield return new WaitForSeconds(delayLsit[i]);
		}
        dialogueEnd = true;
        if (!wait)
        {
            EndText();
            yield return new WaitForSeconds(0.5f);
            NextTalkCheck();
        }
           
    }
    IEnumerator TypingText(string dialogue)
    {
        //yield return new WaitForSeconds(0.1f);
        talkBoxTrm.gameObject.SetActive(true);

        for (int j = 0; j < dialogue.Length; j++)
        {
            talkBoxTxt.text += dialogue[j];
            yield return new WaitForSeconds(0.1f);
        }
        dialoguePlayCheck = false;
    }
    public void EndText()
    {
        talkBoxTrm.gameObject.SetActive(false);
    }
}
