using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLineSkipGage : MonoBehaviour
{
    public Slider skipGage;

    void Awake()
    {
        skipGage = GetComponent<Slider>();
    }
}
