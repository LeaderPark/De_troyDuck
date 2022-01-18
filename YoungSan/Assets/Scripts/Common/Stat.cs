using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] protected StatCategory category;
    [SerializeField] protected int value;
}

public enum StatCategory
{
    Health,
    Attack,
    Speed,
    Stamina
}