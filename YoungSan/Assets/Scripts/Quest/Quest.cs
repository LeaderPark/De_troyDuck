using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/Quest", order = 1)]
public class Quest : ScriptableObject
{
    [TextArea()]
    public string title;
    [TextArea()]
    public string context;
    public Quest prevQuest;
    public Quest nextQuest;

    
}
