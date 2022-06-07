using System;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private bool isEnabled = false;

    [Header("게임 소리")]
    public AudioMixer audioMixer;
    public Slider setBackGroundSoundSlider;
    public Text setBackGroundSoundText;
    public Slider setSFXSoundSlider;
    public Text setSFXSoundText;

    [Header("해상도와 창모드")]
    public Toggle setFullScreenBtn;
    public Dropdown setResolutionDropdown;

    private bool IsFullScreen = true;
    private int resolutionNum;
    private FullScreenMode screenMode;
    private List<string> resolutionText = new List<string>();
    private List<Resolution> resolutions = new List<Resolution>();
    private List<Resolution> setResolutions = new List<Resolution>();

    void Start()
    {
        canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        Init_UI();
    }

    public void Init_UI()
    {
        SetResolution();
    }

    void Update()
    {
        SetBackGroundSound();
        SetSFXSound();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isEnabled)
            {
                UIManager uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
                uIManager.OpenUI(canvasGroup);
                isEnabled = true;
            }
            else
            {
                UIManager uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
                uIManager.CloseUI(canvasGroup);
                isEnabled = false;
            }
        }
    }

    #region 사운드
    public void SetBackGroundSound()
    {
        float value = setBackGroundSoundSlider.value - 40;

        if (value.Equals(-40f)) audioMixer.SetFloat("BG", -80);
        else audioMixer.SetFloat("BG", value);

        setBackGroundSoundText.text = Mathf.Round(setBackGroundSoundSlider.value * 2.5f) + "%";
    }

    public void SetSFXSound()
    {
        float value = setSFXSoundSlider.value - 40;

        if (value.Equals(-40f)) audioMixer.SetFloat("SFX", -80);
        else audioMixer.SetFloat("SFX", value);

        setSFXSoundText.text = Mathf.Round(setSFXSoundSlider.value * 2.5f) + "%";
    }


    #endregion

    #region 해상도와 창모드
    void SetResolution()
    {
        resolutions.AddRange(Screen.resolutions);
        setResolutionDropdown.options.Clear();

        int optionNum = 0;
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRate == 60)
            {
                double result = (double)((double)Screen.resolutions[i].width / (double)Screen.resolutions[i].height);
                float resultTruncate = (float)(Math.Truncate((result * 10000)) / 10000);

                if (resultTruncate == 1.7777f)
                {
                    string resolutionSize = Screen.resolutions[i].width + " X " + Screen.resolutions[i].height;
                    resolutionText.Add(resolutionSize);
                    setResolutions.Add(Screen.resolutions[i]);

                    if (Screen.resolutions[i].width == Screen.width && Screen.resolutions[i].height == Screen.height)
                    {
                        StartCoroutine(SetValue(optionNum));
                    }
                    optionNum++;
                }
            }
        }

        setResolutionDropdown.AddOptions(resolutionText);
        setFullScreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    IEnumerator SetValue(int value)
    {
        yield return null;
        setResolutionDropdown.value = value;
    }

    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }

    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.Windowed : FullScreenMode.FullScreenWindow;
    }

    public void ApplyBtn()
    {
        Screen.SetResolution(setResolutions[resolutionNum].width, setResolutions[resolutionNum].height, screenMode);
        Debug.Log("Setting : " + setResolutions[resolutionNum].width + " / " + setResolutions[resolutionNum].height + " / " + screenMode);
    }
    #endregion
}
