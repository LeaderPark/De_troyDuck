using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BossSelectMarker : Marker, INotification
{
	public PropertyName id { get { return new PropertyName(); } }
	public string bossName;
	public BossObjs[] bossObjs;
	public ExposedReference<GameObject> bossCamObj;

}
[System.Serializable]
public struct BossObjs
{
	public ExposedReference<GameObject> bossObj;
}
