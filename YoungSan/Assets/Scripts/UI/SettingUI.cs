using System;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    public bool IsFullScreen = true;
    public int screenWidth = 1920;
    public int screenHeight = 1080;
    public Dropdown resolutionDropdown;
    List<Resolution> resolutions = new List<Resolution>();
    public int resolutionNum;
    public FullScreenMode screenMode;
    public Toggle fullScreenBtn;

    void Start()
    {
        Init_UI();
        Screen.SetResolution(1920, 1080, false);
        fullScreenBtn.isOn = false;
    }

    public void Init_UI()
    {
        resolutionDropdown.options.Clear();
        resolutions.AddRange(Screen.resolutions);
        resolutionDropdown.options.Clear();
        for(int i = 0; i < Screen.resolutions.Length; i++)
        {            
            if(Screen.resolutions[i].refreshRate == 60)
            {
                int optionNum = 0;
                double result = (double)((double)Screen.resolutions[i].width / (double)Screen.resolutions[i].height);
                float resultTruncate = (float)(Math.Truncate((result * 10)) / 10);
                if(resultTruncate == 1.7f)
                {
                    Dropdown.OptionData option = new Dropdown.OptionData();
                    option.text = Screen.resolutions[i].width + " X " + Screen.resolutions[i].height;   
                    resolutionDropdown.options.Add(option);  
                    if(Screen.resolutions[i].width == Screen.width && Screen.resolutions[i].height == Screen.height)
                    resolutionDropdown.value = optionNum;
                    optionNum++;
                }
                
            }
        }

        fullScreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }

    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void OkBtnClick()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }
}
