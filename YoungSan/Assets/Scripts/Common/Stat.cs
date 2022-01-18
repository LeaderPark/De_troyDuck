using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    public StatCategory category;
    public int minValue;
    public int maxValue;
}

public enum StatCategory
{
    Health,
    Attack,
    Speed,
    Stamina
}