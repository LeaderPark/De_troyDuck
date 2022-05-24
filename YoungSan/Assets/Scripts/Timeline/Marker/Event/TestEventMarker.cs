using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Reflection;

public class TestEventMarker : Marker, INotification
{
	public PropertyName id { get { return new PropertyName(); } }
	public EventData[] events = new EventData[0];

}

#if UNITY_EDITOR
[CustomEditor(typeof(TestEventMarker))]
public class TestEventEditor : Editor
{
    private Vector2 scrollPosition;//오리북세
    public int componentIndices;
    public int functionIndices;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

        TestEventMarker marker = (TestEventMarker)target;

        if (marker != null)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUI.skin.box);
            for (int i = 0; i < marker.events.Length; i++)
            {
                GUILayout.BeginVertical(GUI.skin.window);
                GUILayout.Label($"Element {i}", GUI.skin.window);

                GUILayout.Space(10);

                if (marker.events[i].obj == null)
                {
                    marker.events[i].obj = (GameObject)EditorGUILayout.ObjectField((Object)marker.events[i].obj, typeof(GameObject), true);
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(marker.events[i].obj.name, GUI.skin.box);
                    if (GUILayout.Button("Reset"))
                    {
                        marker.events[i].obj = null;
                        marker.events[i].component = string.Empty;
                        marker.events[i].function = string.Empty;
                        marker.events[i].param = null;
                    }
                    GUILayout.EndHorizontal();
                }
                

                if (marker.events[i].obj != null)
                {

                    GUILayout.Space(10);
                    GUILayout.Label("Component");

                    if (marker.events[i].component == string.Empty)
                    {
                        List<string> componentNames = new List<string>();
                        Component[] components = marker.events[i].obj.GetComponents<Component>();
                        foreach (var component in components)
                        {
                            componentNames.Add(component.GetType().Name);
                        }

                        componentIndices = EditorGUILayout.Popup(componentIndices, componentNames.ToArray());
                        
                        if (GUILayout.Button("Set Component"))
                        {
                            marker.events[i].component = componentNames[componentIndices];
                        }
                    }
                    else
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(marker.events[i].component, GUI.skin.box);
                        if (GUILayout.Button("Reset"))
                        {
                            marker.events[i].component = string.Empty;
                            marker.events[i].function = string.Empty;
                            marker.events[i].param = null;
                        }
                        GUILayout.EndHorizontal();
                    }
                    
                    if (marker.events[i].component != string.Empty)
                    {
                        GUILayout.Space(10);
                        GUILayout.Label("Method");

                        Component com = marker.events[i].obj.GetComponent(marker.events[i].component);
                        if (com != null)
                        {
                            if (com.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Length != 0)
                            {
                                if (marker.events[i].function == string.Empty)
                                {
                                    List<string> functionNames = new List<string>();
                                    List<MethodInfo> functionInfos = new List<MethodInfo>();

                                    foreach (var member in com.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                                    {
                                        if (!functionNames.Contains(member.Name))
                                        {
                                            functionNames.Add(member.Name);
                                            functionInfos.Add(member);
                                        }
                                    }
                                    functionIndices = EditorGUILayout.Popup(functionIndices, functionNames.ToArray());
                                
                                    if (GUILayout.Button("Set Method"))
                                    {
                                        marker.events[i].function = functionInfos[functionIndices].Name;
                                    }
                                }
                                else
                                {
                                    GUILayout.BeginHorizontal();
                                    EditorGUILayout.LabelField(marker.events[i].function, GUI.skin.box);
                                    if (GUILayout.Button("Reset"))
                                    {
                                        marker.events[i].function = string.Empty;
                                        marker.events[i].param = null;
                                    }
                                    GUILayout.EndHorizontal();
                                }

                                if (marker.events[i].function != string.Empty)
                                {
                                    MethodInfo info = null;
                                    foreach (var method in marker.events[i].obj.GetComponent(marker.events[i].component).GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                                    {
                                        if (method.Name == marker.events[i].function)
                                        {
                                            info = method;
                                        }
                                    }
                                    if (info != null)
                                    {
                                        ParameterInfo[] paramArray = info.GetParameters();

                                        GUILayout.Space(10);
                                        
                                        GUILayout.Label("Parameter");
                                        GUILayout.Label($"Count : {paramArray.Length}");

                                        if (marker.events[i].param == null || marker.events[i].param.Length != paramArray.Length)
                                        {
                                            marker.events[i].param = new ParameterData[paramArray.Length];
                                        }

                                        if (paramArray.Length != 0)
                                        {
                                            for (int j = 0; j < paramArray.Length; j++)
                                            {
                                                if (marker.events[i].param[j] == null) continue;
                                                EditorGUILayout.LabelField(paramArray[j].ParameterType.ToString(), GUI.skin.box);

                                                switch (paramArray[j].ParameterType.ToString())
                                                {
                                                    case "System.Single":
                                                    if (marker.events[i].param[j].data == string.Empty) marker.events[i].param[j].data = "0";
                                                    marker.events[i].param[j].data = EditorGUILayout.FloatField(float.Parse(marker.events[i].param[j].data)).ToString();
                                                    marker.events[i].param[j].type = "float";
                                                    break;
                                                    case "System.Int32":
                                                    if (marker.events[i].param[j].data == string.Empty) marker.events[i].param[j].data = "0";
                                                    marker.events[i].param[j].data = EditorGUILayout.IntField(int.Parse(marker.events[i].param[j].data)).ToString();
                                                    marker.events[i].param[j].type = "int";
                                                    break;
                                                    case "System.Boolean":
                                                    if (marker.events[i].param[j].data == string.Empty) marker.events[i].param[j].data = "false";
                                                    marker.events[i].param[j].data = EditorGUILayout.Toggle(bool.Parse(marker.events[i].param[j].data)).ToString();
                                                    marker.events[i].param[j].type = "bool";
                                                    break;
                                                    case "System.String":
                                                    marker.events[i].param[j].data = EditorGUILayout.TextField(marker.events[i].param[j].data);
                                                    marker.events[i].param[j].type = "string";
                                                    break;
                                                    case "UnityEngine.Vector2":
                                                    if (marker.events[i].param[j].data == string.Empty) marker.events[i].param[j].data = "0*0";
                                                    string[] ss2 = marker.events[i].param[j].data.Split('*');
                                                    Vector2 v2 = EditorGUILayout.Vector2Field("", new Vector2(float.Parse(ss2[0]), float.Parse(ss2[1])));
                                                    marker.events[i].param[j].data = $"{v2.x}*{v2.y}";
                                                    marker.events[i].param[j].type = "Vector2";
                                                    break;
                                                    case "UnityEngine.Vector3":
                                                    if (marker.events[i].param[j].data == string.Empty) marker.events[i].param[j].data = "0*0*0";
                                                    string[] ss3 = marker.events[i].param[j].data.Split('*');
                                                    Vector3 v3 = EditorGUILayout.Vector3Field("", new Vector3(float.Parse(ss3[0]), float.Parse(ss3[1]), float.Parse(ss3[2])));
                                                    marker.events[i].param[j].data = $"{v3.x}*{v3.y}*{v3.z}";
                                                    marker.events[i].param[j].type = "Vector3";
                                                    break;
                                                    default:
                                                    break;
                                                }

                                                GUILayout.Space(5);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                
                GUILayout.Space(20);
                GUILayout.EndVertical();
                GUILayout.Space(20);
            }
            GUILayout.EndScrollView();
        }

	}
}
#endif

[System.Serializable]
public struct EventData
{
    [HideInInspector] public GameObject obj;
    [HideInInspector] public string component;
    [HideInInspector] public string function;
    [HideInInspector] public ParameterData[] param;
}

[System.Serializable]
public class ParameterData
{
    [HideInInspector] public string data = string.Empty;
    [HideInInspector] public string type = string.Empty;
}