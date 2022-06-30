using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using System.Threading;
using UnityEngine.UI;

public class TestBehaivor : PlayableBehaviour
{
    public GameObject talker;
    public string txt;
    public GameObject talkObj;
    public AnimationCurve delayCurve;
    public AnimationCurve fontSizeCurve;
    public TextColor[] textColors;

    private Vector3[] vertice;

    private TextMeshProUGUI talkBox;
    private TextMeshProUGUI fakeTalkbox;
    //private GameObject endImage;
    private PoolManager poolManager;
    private UIManager uiManager;
    public AudioClip test;

    private float time = 0;
    private int idx = 0;
    private EntityData entityData;
    private TimelineController timelineCon;

    private Coroutine textOut;
    public List<Coroutine> texts = new List<Coroutine>();


    //Ÿ�Ӷ��� �����ϸ� ����
    public override void OnGraphStart(Playable playable)
    {
        if (Application.isPlaying)
        {
            if (ManagerObject.Instance != null)
            {
                poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
                uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
            }
        }
        //Debug.Log("AAA");
    }
    //�÷��̾�� Ʈ���� ����ɶ� �ѹ� ����
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (test == null)
        {
            test = Resources.Load("Sounds/Test6") as AudioClip;
        }
        if (Application.isPlaying)
        {
            talkObj = poolManager.GetUIObject("TalkBoxTMP");
        }
        else
        {
            List<GameObject> pool = new List<GameObject>();
            GameObject canvas = GameObject.Find("CutSceneCanvas");

            time = 0;
            idx = 0;
            for (int i = 0; i < canvas.transform.childCount - 1; i++)
            {
                pool.Add(canvas.transform.GetChild(i).gameObject);
            }
            foreach (var item in pool)
            {
                if (!item.activeSelf)
                {
                    item.SetActive(true);
                    talkObj = item;
                    break;
                }
            }
            if (talkObj == null)
            {
                talkObj = GameObject.Instantiate(Resources.Load<GameObject>("PoolObject/TalkBoxTMP"), canvas.transform);
            }

        }
        talkBox = talkObj.transform.Find("text").GetComponent<TextMeshProUGUI>();
        fakeTalkbox = talkObj.transform.Find("fakeText").GetComponent<TextMeshProUGUI>();
        //endImage = talkObj.transform.Find("EndImage").gameObject;
        timelineCon = GameObject.Find("CutScenePrefab").GetComponent<TimelineController>();
        entityData = talker.GetComponent<Entity>().entityData;
        //talkBox.text = txt;
        talkBox.text = "";
        Color[] colors = new Color[txt.Length];
        if (textColors != null)
        {
            for (int i = 0; i < textColors.Length; i++)
            {
                for (int j = 0; j < txt.Length; j++)
                {
                    if (j >= textColors[i].startIndex && j < textColors[i].endIndex)
                    {
                        colors[j] = textColors[i].color;
                    }
                }
            }
        }
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < txt.Length; i++)
        {
            stringBuilder.Append("<color=#");
            stringBuilder.AppendFormat("{0:x2}", (int)(colors[i].r * 255));
            stringBuilder.AppendFormat("{0:x2}", (int)(colors[i].g * 255));
            stringBuilder.AppendFormat("{0:x2}", (int)(colors[i].b * 255));
            stringBuilder.Append("ff>");
            stringBuilder.Append("<size=");
            stringBuilder.Append(fontSizeCurve[i].value);
            stringBuilder.Append(">");
            stringBuilder.Append(txt[i]);
            stringBuilder.Append("</size>");
            stringBuilder.Append("</color>");
        }
        talkBox.text = stringBuilder.ToString();
        fakeTalkbox.text = "";
        textOut = uiManager.StartCoroutine(uiManager.TextAnimationPlay(talkBox, delayCurve, this));

        talkObj.SetActive(true);
        SetBoxSize();

    }
    //Ÿ�Ӷ��� ���� �ɸ��� ����(�������� ����)
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (Application.isPlaying)
        {
            if (talkObj != null)
            {
                talkObj.SetActive(false);
                uiManager.StopCoroutine(textOut);
                foreach (Coroutine coroutine in texts)
                {
                    uiManager.StopCoroutine(coroutine);
                }
                texts.Clear();
                //if ((int)playable.GetTime() >= (int)playable.GetDuration())
                //{
                //	//endImage.SetActive(false);
                //	talkObj.SetActive(false);
                //}
            }
        }
        else
        {
            if (talkObj != null)
                talkObj.SetActive(false);
        }
    }
    //�÷��̾�� Ʈ�� �����߿� ��� ����
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (talker == null || delayCurve.length == 0)
            return;
        vertice = talkBox.mesh.vertices;
        if (Application.isPlaying)
        {
            talkObj.transform.position = Camera.main.WorldToScreenPoint(talker.transform.position + new Vector3(0, entityData.uiPos, 0));
            time += Time.deltaTime * (float)playable.GetGraph().GetRootPlayable(0).GetSpeed();
            //endImage.SetActive(timelineCon.timelinePause);
            //for (bool b = true; b;)
            //{
            //	b = false;
            //	if (time >= delayCurve.Evaluate(idx * 0.1f))
            //	{
            //		b = true;
            //		time -= delayCurve.Evaluate(idx * 0.1f);
            //		if (idx < txt.Length)
            //		{
            //			//StringBuilder stringBuilder = new StringBuilder();
            //			//stringBuilder.Append("<size="+fontSizeCurve[idx].value+">"+txt[idx]+"</size>");
            //			//talkBox.text += stringBuilder;
            //			uiManager.StartCoroutine(uiManager.TextAnimation(idx, talkBox, vertice));
            //			idx = Mathf.Clamp(idx + 1, 0, txt.Length - 1);
            //		}
            //	}
            //}
            //if (time >= delayCurve.Evaluate(idx * 0.1f))
            //{
            //	time = 0;
            //	uiManager.StartCoroutine(uiManager.TextAnimation(idx, talkBox, vertice));
            //	idx = Mathf.Clamp(idx + 1, 0, txt.Length - 1);
            //}
        }
        else
        {
            try
            {
                talkObj.transform.position = Camera.main.WorldToScreenPoint(talker.transform.position + new Vector3(0, entityData.uiPos, 0));
                time += Time.deltaTime;
                for (bool b = true; b;)
                {
                    b = false;
                    if (time >= delayCurve.Evaluate(idx * 0.1f))
                    {
                        b = true;
                        time -= delayCurve.Evaluate(idx * 0.1f);
                        if (idx < txt.Length)
                        {
                            talkBox.text += txt[idx];
                            idx = Mathf.Clamp(idx + 1, 0, txt.Length);
                        }
                    }
                }
            }
            catch
            {

            }
        }
    }
    public void SetBoxSize()
    {
        talkBox.rectTransform.anchoredPosition = new Vector2(0, 0);
        float x = talkBox.preferredWidth;
        float y = talkBox.preferredHeight;
        //float x = fakeTalkbox.preferredWidth;
        //float y = fakeTalkbox.preferredHeight;
        RectTransform talkBoxRect = talkObj.GetComponent<RectTransform>();
        talkBoxRect.sizeDelta = new Vector2(x, y) + new Vector2(50, 50);
        talkBox.rectTransform.anchoredPosition += new Vector2(50 / 2, 0);
    }
    //public IEnumerator TextAnimation(int idx, TextMeshProUGUI talkBox)
    //{
    //	Mesh mesh = talkBox.mesh;
    //	Vector3[] vertice = talkBox.mesh.vertices;
    //	Vector3[] origineVertice = talkBox.mesh.vertices;
    //	Color32[] vertexColors = talkBox.textInfo.meshInfo[0].colors32;
    //	float time = 0;
    //	while (true)
    //	{
    //		time += Time.deltaTime;
    //		vertice[idx * 4 + 0] = Vector3.Lerp(origineVertice[idx * 4 + 0], origineVertice[idx * 4 + 0] + new Vector3(0, 10, 0), time / 0.1f);
    //		vertice[idx * 4 + 1] = Vector3.Lerp(origineVertice[idx * 4 + 1], origineVertice[idx * 4 + 1] + new Vector3(0, 10, 0), time / 0.1f);
    //		vertice[idx * 4 + 2] = Vector3.Lerp(origineVertice[idx * 4 + 2], origineVertice[idx * 4 + 2] + new Vector3(0, 10, 0), time / 0.1f);
    //		vertice[idx * 4 + 3] = Vector3.Lerp(origineVertice[idx * 4 + 3], origineVertice[idx * 4 + 3] + new Vector3(0, 10, 0), time / 0.1f);

    //		vertexColors[idx * 4 + 0].a = (byte)Mathf.Lerp(255, 0, time / 0.1f);
    //		vertexColors[idx * 4 + 1].a = (byte)Mathf.Lerp(255, 0, time / 0.1f);
    //		vertexColors[idx * 4 + 2].a = (byte)Mathf.Lerp(255, 0, time / 0.1f);
    //		vertexColors[idx * 4 + 3].a = (byte)Mathf.Lerp(255, 0, time / 0.1f);

    //		talkBox.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    //		mesh.vertices = vertice;
    //		talkBox.canvasRenderer.SetMesh(mesh);

    //		if (time > 0.1f)
    //		{
    //			yield break;
    //		}
    //	}
    //	yield return null;
    //}
}

[System.Serializable]
public class TextColor
{
    public int startIndex;
    public int endIndex;
    public Color color;
}