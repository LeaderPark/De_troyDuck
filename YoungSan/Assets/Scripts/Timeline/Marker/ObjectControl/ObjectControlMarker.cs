using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEditor;
using System.ComponentModel;

[DisplayName("Object/ObjControl")]

public class ObjectControlMarker : Marker, INotification
{
	public PropertyName id { get { return new PropertyName(); } }

	public AnimationData[] animationDatas;
	public ObjectData[] objectData;
	//public ExposedReference<GameObject> contorolObject;
	//public ExposedReference<AnimationClip> animation;
	//public string animationName;

}

[System.Serializable]
public struct AnimationData
{
	public bool mainChar;
	public bool getMainCharAnimator;
	public ExposedReference<GameObject> contorolObject;
	public string animation;
	public bool flipX;
}
[System.Serializable]
public struct ObjectData
{
	public bool mainChar;
	public ExposedReference<GameObject> obj;
	public bool active;
	public Vector3 objPos;
	public ExposedReference<Transform> objTrm;
	public bool mainCharTrm;

}