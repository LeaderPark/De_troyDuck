using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[DisplayName("Jump/JumpMarker")]
public class JumpMarker : Marker, INotification
{
	public PropertyName id { get { return new PropertyName(); } }

	[SerializeField] public LoopMarker loopMarker;
	[SerializeField] public LoopEndMarker loopEndMarker;
	public bool qeustSelect;
	public Quest quest;


}
#if UNITY_EDITOR
[CustomEditor(typeof(JumpMarker))]
public class JumpEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		JumpMarker jMarker = (JumpMarker)target;
		var markers = jMarker.parent.GetMarkers().ToArray();
		GUILayout.Label("LoopMarker");
		foreach (var m in markers)
		{
			if (m.GetType() == typeof(LoopMarker))
			{
				if (GUILayout.Button(m.ToString()))
				{
					jMarker.loopMarker = m as LoopMarker;
				}
			}
		}
		GUILayout.Label("LoopEndMarker");
		foreach (var m in markers)
		{
			if (m.GetType() == typeof(LoopEndMarker))
			{
				if (GUILayout.Button(m.ToString()))
				{
					jMarker.loopEndMarker = m as LoopEndMarker;
				}
			}
		}



	}
}
#endif