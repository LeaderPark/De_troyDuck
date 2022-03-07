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
   // private Entity talkerEntity;

    private bool dialoguePlayCheck;
    private bool wait = false;
    private bool timeLineStart = false;
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
        if(talker!=null)
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
        if (timeLineStart)
        {
            timelineController.StartTimeline();
            EndText();
        }
        else
        {
            if (lineIdx <= maxLineIdx)
            {
                EndText();
                NextTalk();
            }
            else
            {
                EndText();
                if (wait)
                {
                    timelineController.StartTimeline();
                }
            }
        }
    }
    //처음 파일을 읽고 대화를 시작할때 쓸 함수
    public void StartTalk(List<Dictionary<string, object>> dialogueDataList,bool _wait)
    {
        dialogueEnd = false;
        data = dialogueDataList;
        lineIdx = 0;
        lineIdx++;
        maxLineIdx = int.Parse(data[data.Count - 1]["Line"].ToString());
        wait = _wait;
        timeLineStart = false;
        NextTalk();
    }

    //라인 데이터를 받아서 ReadLineText를 실행시켜주는 함수
    public void NextTalk()
    {
        timeLineStart = false;

        List<Dictionary<string, object>> dialogueDataList = data.FindAll(x => int.Parse(x["Line"].ToString()) == lineIdx);

        string charName = data.Find(x => int.Parse(x["Line"].ToString()) == lineIdx)["Char"].ToString();
        List<string> dialougeList = new List<string>();
        List<float> dialougeDelayList = new List<float>();
        List<string> dialougeAnimationList = new List<string>();
        List<string> dialougeCutList = new List<string>();


        foreach (var item in dialogueDataList)
		{
            dialougeList.Add(item["Dialogue"].ToString());
            dialougeDelayList.Add(float.Parse(item["Delay"].ToString()));
            dialougeAnimationList.Add(item["Animation"].ToString());
            dialougeCutList.Add(item["Cut"].ToString());
        }
        TalkerSet(charName);
        StartCoroutine(ReadLineText(charName, dialougeList, dialougeDelayList, dialougeAnimationList, dialougeCutList));
    }

    private void TalkerSet(string talkerName)
    {
       talker = GameObject.Find(talkerName);
       //talkerEntity = talker.GetComponent<Entity>();
    }
    private void AnimationSet(string animationName)
    {
		if (animationName != "None")
		{
			Animator objAnimator = talker.GetComponent<Animator>();
			objAnimator.Play(animationName);

		}
    }

    //한 라인의 데이터를 받아서 실행하는 함수
    IEnumerator ReadLineText(string talkerName,List<string>dialogueList,List<float> delayLsit,List<string> dialougeAnimationList,List<string> dialougeCutList)
    {
        talkBoxTxt.text = "";
        fakeTalkBoxTxt.text = "";
        talkBoxTxt.rectTransform.anchoredPosition = new Vector2(0, 0);

        for (int i = 0; i < dialogueList.Count; i++)
        {
            fakeTalkBoxTxt.text += dialogueList[i];
        }

        float x = fakeTalkBoxTxt.preferredWidth;
        float y = fakeTalkBoxTxt.preferredHeight;
        talkBoxTrm.sizeDelta = new Vector2(x, y) + padding;

        talkBoxTxt.rectTransform.anchoredPosition += new Vector2(padding.x / 2,0); 

        for (int i = 0; i < dialogueList.Count; i++)
		{
            dialoguePlayCheck = true;
            AnimationSet(dialougeAnimationList[i]);
            StartCoroutine(TypingText(dialogueList[i]));
            if (dialougeCutList[i] != "None")
            {
                timeLineStart = true;
                timelineController.StartTimeline();
            }
            while (dialoguePlayCheck)
			{
				yield return null;
			}
            //print(delayLsit[i] + Time.deltaTime);
			yield return new WaitForSeconds(delayLsit[i]);
		}
        if (!timeLineStart)
        {
            dialogueEnd = true;
        }
        //대화창 넘기는 ▽이런거 추가 예정
        if (!wait)
        {
            EndText();
            yield return new WaitForSeconds(0.5f);
            NextTalkCheck();
        }
           
    }
    IEnumerator TypingText(string dialogue)
    {
        SoundManager soundManager = ManagerObject.Instance.GetManager(ManagerType.SoundManager) as SoundManager;
        talkBoxTrm.gameObject.SetActive(true);

		for (int j = 0; j < dialogue.Length; j++)
		{
			talkBoxTxt.text += dialogue[j];
            if(j%4 ==0)
            soundManager.SoundStart("Test6", transform,false);
            yield return new WaitForSeconds(0.01f);
		}
        dialoguePlayCheck = false;
    }
    public void EndTalk()
    {
        timelineController.PauseTimeline();
        dialogueEnd = true;
        timeLineStart = false;
        //대화창 넘기는 ▽이런거 추가 예정
    }
    public void EndText()
    {
        talkBoxTrm.gameObject.SetActive(false);
    }
}
