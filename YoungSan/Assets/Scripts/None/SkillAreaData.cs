using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAreaData : MonoBehaviour
{
    public AreaBox[] LeftAreaBox;
    public AreaBox[] RightAreaBox;

    public bool inLeftSkillArea {get; set;}
    public bool inRightSkillArea {get; set;}


    void Awake()
    {
        foreach (var item in LeftAreaBox)
        {
            item.skillAreaData = this;
            item.areaDirection = AreaDirection.Left;
        }
        foreach (var item in RightAreaBox)
        {
            item.skillAreaData = this;
            item.areaDirection = AreaDirection.Right;
        }
    }
}
