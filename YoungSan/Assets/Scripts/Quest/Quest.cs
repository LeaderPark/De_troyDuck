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

    public Quest prevQuest;
    public Quest nextQuest;
    public QuestType type;
    public int destination;

    public bool resetPrevQuest;
}

public enum QuestType
{
    Condition,
    EnemyHunt,
}
