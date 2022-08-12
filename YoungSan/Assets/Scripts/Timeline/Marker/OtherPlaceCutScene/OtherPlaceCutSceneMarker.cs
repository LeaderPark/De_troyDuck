using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class OtherPlaceCutSceneMarker : Marker,INotification
{
	public PropertyName id { get { return new PropertyName(); } }

	public GameObject nextCutScenePrefab;
}
