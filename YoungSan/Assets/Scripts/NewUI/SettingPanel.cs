using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{

    public Toggle fullScreenToggle;
    public Dropdown resolutionDropdown;

    public Slider bgmSlider;
    public Text bgmText;
    public Slider sfxSlider;
    public Text sfxText;

    public bool IsFullScreen = true;
    public int resolutionNum;
    public FullScreenMode screenMode;
    public List<string> resolutionText = new List<string>();
    public List<Resolution> resolutions = new List<Resolution>();
    public List<Resolution> setResolutions = new List<Resolution>();


    void Start()
    {
        Init_UI();
    }

    public void Init_UI()
    {
        SetResolution();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("bgmSlider"))
        {
            bgmSlider.value = PlayerPrefs.GetFloat("bgmSlider");
        }
        if (PlayerPrefs.HasKey("sfxSlider"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("sfxSlider");
        }
        SetBackGroundSound();
        SetSFXSound();
        if (PlayerPrefs.HasKey("width") && PlayerPrefs.HasKey("height") && PlayerPrefs.HasKey("screenMode"))
        {
            Screen.SetResolution(PlayerPrefs.GetInt("width"), PlayerPrefs.GetInt("height"), (FullScreenMode)PlayerPrefs.GetInt("screenMode"));
        }
    }

    #region 사운드
    public void SetBackGroundSound()
    {
        SoundManager soundManager = ManagerObject.Instance.GetManager(ManagerType.SoundManager) as SoundManager;
        float value = bgmSlider.value - 40;

        if (value.Equals(-40f)) soundManager.mixer.SetFloat("BG", -80);
        else soundManager.mixer.SetFloat("BG", value);

        bgmText.text = Mathf.Round(bgmSlider.value * 2.5f) + "%";
        PlayerPrefs.SetFloat("bgmSlider", bgmSlider.value);
    }

    public void SetSFXSound()
    {
        SoundManager soundManager = ManagerObject.Instance.GetManager(ManagerType.SoundManager) as SoundManager;
        float value = sfxSlider.value - 40;

        if (value.Equals(-40f)) soundManager.mixer.SetFloat("SFX", -80);
        else soundManager.mixer.SetFloat("SFX", value);

        sfxText.text = Mathf.Round(sfxSlider.value * 2.5f) + "%";
        PlayerPrefs.SetFloat("sfxSlider", sfxSlider.value);
    }


    #endregion

    #region 해상도와 창모드
    void SetResolution()
    {
        resolutions.AddRange(Screen.resolutions);

        resolutionDropdown.options.Clear();

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

        resolutionDropdown.AddOptions(resolutionText);
        fullScreenToggle.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    IEnumerator SetValue(int value)
    {
        yield return null;
        resolutionDropdown.value = value;
    }

    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }

    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void ApplyBtn()
    {
        Screen.SetResolution(setResolutions[resolutionNum].width, setResolutions[resolutionNum].height, screenMode);
        PlayerPrefs.SetInt("width", (int)setResolutions[resolutionNum].width);
        PlayerPrefs.SetInt("height", (int)setResolutions[resolutionNum].height);
        PlayerPrefs.SetInt("screenMode", (int)screenMode);
        Debug.Log("Setting : " + setResolutions[resolutionNum].width + " / " + setResolutions[resolutionNum].height + " / " + screenMode);
    }
    #endregion
}
