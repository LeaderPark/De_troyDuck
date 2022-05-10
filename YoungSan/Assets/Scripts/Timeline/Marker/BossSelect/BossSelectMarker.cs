using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BossSelectMarker : Marker, INotification
{
	public PropertyName id { get { return new PropertyName(); } }
	public ExposedReference<GameObject> bossObj;
	public ExposedReference<GameObject> bossCamObj;

}
