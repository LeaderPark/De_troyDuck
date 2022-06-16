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
	[SerializeField] public LoopEndMarker questRefuse;
	public bool qeustSelect;
	public Quest quest;


}
#if UNITY_EDITOR
[CustomEditor(typeof(JumpMarker))]
public class JumpEditor : Editor
{
	int[] markerIndex = new int[3];
	List<LoopMarker> loopMarkers = new List<LoopMarker>();
	List<LoopEndMarker> loopEndMarkers = new List<LoopEndMarker>();
	List<string> loopMarkerNames = new List<string>();
	List<string> loopEndMarkerNames = new List<string>();

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		JumpMarker jMarker = (JumpMarker)target;
		var markers = jMarker.parent.GetMarkers().ToArray();

		loopMarkers.Clear();
		loopEndMarkers.Clear();
		loopMarkerNames.Clear();
		loopEndMarkerNames.Clear();
		loopMarkers.Add(null);
		loopEndMarkers.Add(null);
		loopMarkerNames.Add("None");
		loopEndMarkerNames.Add("None");

		foreach (var m in markers)
		{
			if (m.GetType() == typeof(LoopMarker))
			{
				loopMarkers.Add(m as LoopMarker);
				loopMarkerNames.Add(m.ToString());
			}
			if (m.GetType() == typeof(LoopEndMarker))
			{
				loopEndMarkers.Add(m as LoopEndMarker);
				loopEndMarkerNames.Add(m.ToString());
			}
		}

		for (int i = 0; i < loopMarkers.Count; i++)
		{
			if (jMarker.loopMarker == loopMarkers[i])
			{
				markerIndex[0] = i;
			}
		}
		for (int i = 0; i < loopEndMarkers.Count; i++)
		{
			if (jMarker.loopEndMarker == loopEndMarkers[i])
			{
				markerIndex[1] = i;
			}
			if (jMarker.questRefuse == loopEndMarkers[i])
			{
				markerIndex[2] = i;
			}
		}

		GUILayout.Label("LoopMarker");
		markerIndex[0] = EditorGUILayout.Popup(markerIndex[0], loopMarkerNames.ToArray());
		jMarker.loopMarker = loopMarkers[markerIndex[0]];
		
		GUILayout.Space(10);

		GUILayout.Label("LoopEndMarker");
		markerIndex[1] = EditorGUILayout.Popup(markerIndex[1], loopEndMarkerNames.ToArray());
		jMarker.loopEndMarker = loopEndMarkers[markerIndex[1]];
		
		GUILayout.Space(10);
	GUILayout.Label("questRefuse");
	
		markerIndex[2] = EditorGUILayout.Popup(markerIndex[2], loopEndMarkerNames.ToArray());
		jMarker.questRefuse = loopEndMarkers[markerIndex[2]];

	}
}
#endif