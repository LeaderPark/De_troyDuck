using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[DisplayName("Object/CSControl")]
public class EnemyCSControlMarker : Marker, INotification
{
	public PropertyName id { get { return new PropertyName(); } }
	public EnemyObjs[] enemyObjs;
	public bool scriptEnable;
}

[System.Serializable]
public struct EnemyObjs
{
	public ExposedReference<GameObject> obj;
}
