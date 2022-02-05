using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEditor;

public class ObjectControlMarker : Marker, INotification
{
	public PropertyName id { get { return new PropertyName(); } }
	public ExposedReference<GameObject> contorolObject;
	public string animationName;

}