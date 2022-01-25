using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillArea : MonoBehaviour
{
    public SkillAreaBundle[] skillAreaBundles;
}

[System.Serializable]
public class SkillAreaBundle
{
    public EventCategory eventCategory;
    public SkillAreaData[] skillAreaDatas;
}