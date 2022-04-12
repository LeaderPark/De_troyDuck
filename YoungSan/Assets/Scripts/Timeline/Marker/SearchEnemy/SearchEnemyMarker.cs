using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SearchEnemyMarker : Marker, INotification
{
	public PropertyName id { get { return new PropertyName(); } }
	public Enemys[] enemys;
	public TimelineAsset nextTimeLine;
}

[System.Serializable]

public struct Enemys
{
	public ExposedReference<GameObject> enemy;
}
