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

    public bool IsFullScreen = true;
    public int resolutionNum;
    public FullScreenMode screenMode;
    public List<string> resolutionText = new List<string>();
    public List<Resolution> resolutions = new List<Resolution>();
    public List<Resolution> setResolutions = new List<Resolution>();

    void Start()
    {
        canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        Init_UI();
    }

    public void Init_UI()
    {
        SetResolution();
    }

    // void Update()
    // {
    //     SetBackGroundSound();
    //     SetSFXSound();

    //     UIManager uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;

    //     if (Input.GetKeyDown(KeyCode.Escape))
    //     {
    //         if (!isEnabled)
    //         {
    //             uIManager.OpenUI(canvasGroup, true);
    //             isEnabled = true;
    //         }
    //         else if (isEnabled)
    //         {
    //             uIManager.CloseUI(canvasGroup);
    //             isEnabled = false;
    //         }
    //     }
    // }

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

            if (Screen.resolutions[i].refreshRate >= 60 || Screen.resolutions[i].refreshRate <= 144)
            {
                double result = (double)((double)Screen.resolutions[i].width / (double)Screen.resolutions[i].height);
                float resultTruncate = (float)(Math.Truncate((result * 10000)) / 10000);
                //Debug.Log(Screen.resolutions[i]);

                //if (resultTruncate == 1.7777f)
                //{

                string resolutionSize = Screen.resolutions[i].width + " X " + Screen.resolutions[i].height + " @ " + Screen.resolutions[i].refreshRate + "hz";
                resolutionText.Add(resolutionSize);
                setResolutions.Add(Screen.resolutions[i]);

                if (Screen.resolutions[i].width == Screen.width && Screen.resolutions[i].height == Screen.height)
                {
                    StartCoroutine(SetValue(optionNum));
                }
                optionNum++;
                //}
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
