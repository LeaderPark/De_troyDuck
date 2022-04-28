using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class JumpMarker : Marker, INotification
{
	public PropertyName id { get { return new PropertyName(); } }

	[SerializeField] public LoopMarker test;

}
[CustomEditor(typeof(JumpMarker))]
public class JumpEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		JumpMarker jMarker = (JumpMarker)serializedObject.targetObject;
		var markers = jMarker.parent.GetMarkers().ToArray();
		foreach (var m in markers)
		{
			if (m.GetType() == typeof(LoopMarker))
			{
				if (GUILayout.Button(m.ToString()))
				{
					jMarker.test = m as LoopMarker;
				}
			}
		}

	}
}
