using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/Quest", order = 1)]
public class Quest : ScriptableObject
{
    public int questId;
    [TextArea()]
    public string title;
    [TextArea()]
    public string context;

    public ClearValue clearValue;
    public Quest prevQuest;
    public Quest nextQuest;
    
    public bool clear; 
}
