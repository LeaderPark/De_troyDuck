using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEditor;
using System;

[CreateAssetMenu(fileName = "NpcData", menuName = "ScriptableObjects/NpcData", order = 1)]
public class NpcData : ScriptableObject
{
	public Quest quest;
	public TimelineAsset noneQusetTimeline;
	public List<TimelineAsset> timelineList = new List<TimelineAsset>();
}
