using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SearchEnemyMarker : Marker, INotification
{
	public PropertyName id { get { return new PropertyName(); } }
	public bool mainChar = false;
	public Enemys[] enemys;
	public TimelineAsset nextTimeLine;
	public float waitTime;
}

[System.Serializable]

public struct Enemys
{
	public ExposedReference<GameObject> enemy;
}
