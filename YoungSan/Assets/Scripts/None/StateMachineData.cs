using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StateMachineData", menuName = "ScriptableObjects/StateMachineData", order = 1)]
public class StateMachineData : ScriptableObject
{
    [Space(40)]
        
    public float minMoveTime;
    public float maxMoveTime;
    
    [Space(20)]
    public float minIdleTime;
    public float maxIdleTime;

    [Space(20)]
    public float homeTime;

    [Space(20)]
    public float searchDelay;
    
    [Space(10)]
    public float activityRadius;
    [Space(10)]
    public float searchRadius;
    [Space(10)]
    public float distanceRadius;
}
