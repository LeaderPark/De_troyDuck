using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MapEditor
{
    #if UNITY_EDITOR
    public class MapEditor : MonoBehaviour
    {
        public static Dictionary<string, object> objects = new Dictionary<string, object>();
        public static List<GameObject> resources = new List<GameObject>();


        [MenuItem("MapEditor/Resource Selector")]
        public static void MapEditor_ResourceSelector_Show()
        {
            EditorWindow.GetWindow<ResourceSelector>().titleContent = new GUIContent("Resource Selector");
        }

        [MenuItem("MapEditor/Brush Selector")]
        public static void MapEditor_BrushSelector_Show()
        {
            EditorWindow.GetWindow<BrushSelector>().titleContent = new GUIContent("Brush Selector");
        }
        
    }
    #endif
}
