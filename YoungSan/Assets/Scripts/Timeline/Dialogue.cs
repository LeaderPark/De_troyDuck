using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [SerializeField]private RectTransform talkBoxTrm;
    private Text talkBoxTxt;
    private Text fakeTalkBoxTxt;

    public GameObject Talker;
    public string dialogueName;

    private bool dialoguePlayCheck;
    public bool dialogueEnd;

    Vector3 padding = new Vector3(30,30);
    List<Dictionary<string, object>> data;


    // Start is called before the first frame update
    private void Awake()
	{
        data = CSVReader.Read("Dialogue/testDialogue");
        talkBoxTxt = talkBoxTrm.gameObject.transform.Find("text").gameObject.GetComponent<Text>();
        fakeTalkBoxTxt = talkBoxTrm.gameObject.transform.Find("fakeText").gameObject.GetComponent<Text>();
    }
	void Start()
    {
        print(talkBoxTrm.transform.Find("text").gameObject.GetComponent<Text>());
        talkBoxTrm.gameObject.SetActive(false);
        fakeTalkBoxTxt.gameObject.SetActive(false);

    }
	private void Update()
	{
        talkBoxTrm.transform.position = Camera.main.WorldToScreenPoint(Talker.transform.position + new Vector3(0, 1));
    }
    public void TalkStart(int lineIdx)
    {
        StartCoroutine(SetText(lineIdx));
    }
    IEnumerator SetText(int lineIdx)
    {
        //testTalker.GetComponent<Entity>().GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[] { "test" });
        dialogueEnd = false;
        talkBoxTrm.transform.position = Camera.main.WorldToScreenPoint(Talker.transform.position + new Vector3(0, 1));

        List<string> dialogueData = new List<string>();
        List<float> dialogueDelay = new List<float>();

        for (int i = 0; i < data.Count; i++)
		{
            if ((int)data[i]["Line"] == lineIdx)
            {
                dialogueData.Add(data[i]["Dialogue"].ToString());
                dialogueDelay.Add(float.Parse(data[i]["Delay"].ToString()));
            }
		}

        talkBoxTrm.gameObject.SetActive(true);

        string str = "";
        for (int i = 0; i < dialogueData.Count; i++)
        {
            str += dialogueData[i];
            //for (int i = 0; i < Test.Length; i++)
            //{
            //          str += Test[i];
            //          if (i%10==0&&i!=0)
            //          {
            //              str += "\n";
            //          }

            //      }
            //fakeTalkBoxTxt.text = _text;

        }
        fakeTalkBoxTxt.text = str;

        float x = fakeTalkBoxTxt.preferredWidth;
        float y = fakeTalkBoxTxt.preferredHeight;

        talkBoxTrm.sizeDelta = new Vector3(x, y) + padding;
        talkBoxTxt.text = "";
        for (int i = 0; i < dialogueData.Count; i++)
        {
            dialoguePlayCheck = true;
			//print("´ë»ç : "+dialogueData[i] +"\n µô·¹ÀÌ : "+ dialogueDelay[i]);
			StartCoroutine(TypingText(dialogueData[i]));
			while (true)
			{
                yield return null;
                if (!dialoguePlayCheck)
                {
                    break;
                }
			}
            yield return new WaitForSeconds(dialogueDelay[i]);
        }
        dialogueEnd = true;

    }
    public void EndText()
    {
        Talker.GetComponent<Entity>().GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[] { "null" });
        talkBoxTrm.gameObject.SetActive(false);
    }
    IEnumerator TypingText(string dialogueData)
    {
        talkBoxTrm.gameObject.SetActive(true);
        for (int j = 0; j < dialogueData.Length; j++)
        {
            talkBoxTxt.text += dialogueData[j];
            yield return new WaitForSeconds(0.1f);
        }
        dialoguePlayCheck = false;
    }
}
