using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace MapEditor
{
    #if UNITY_EDITOR
    public class BrushSelector : EditorWindow
    {
        bool brushing;

        Dictionary<string, Brush> brushes;

        GUIStyle green;
        GUIStyle red;

        void OnEnable()
        {
            brushing = false;
            MapEditor.objects["brushSize"] = 0.1f;
            MapEditor.objects["brushIndex"] = 0;
            MapEditor.objects["gridInterval"] = Vector2.one;
            MapEditor.objects["gridHeight"] = 0.0f;
            MapEditor.objects["brushParent"] = null;
            MapEditor.objects["brushDensity"] = 1;
            MapEditor.objects["gridActive"] = true;

            brushes = new Dictionary<string, Brush>();
            green = new GUIStyle();
            red = new GUIStyle();

            green.normal.textColor = Color.green;
            green.alignment = TextAnchor.MiddleCenter;
            green.normal.background = Texture2D.whiteTexture;

            red.normal.textColor = Color.red;
            red.alignment = TextAnchor.MiddleCenter;
            red.normal.background = Texture2D.whiteTexture;

            foreach (Brush brush in Resources.LoadAll<Brush>("ScriptableObjects"))
            {
                brushes.Add(brush.name, brush);
            }
        }

        void OnDisable()
        {
            if (MapEditor.objects.ContainsKey("BrushObj"))
            {
                if (MapEditor.objects["BrushObj"] != null)
                {
                    DestroyImmediate(((MapEditorBrush)MapEditor.objects["BrushObj"]).gameObject);
                    MapEditor.objects["BrushObj"] = null;
                }
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal(GUILayout.Height(40));
            if (!brushing)
            {
                if (GUILayout.Button("Brush Off", red, GUILayout.Width(120), GUILayout.Height(40)))
                {
                    brushing = true;
                    MapEditor.objects["BrushObj"] = new GameObject("BrushObj").AddComponent<MapEditorBrush>();
                }
                SceneVisibilityManager.instance.EnableAllPicking();
            }
            else
            {
                if (GUILayout.Button("Brush On", green, GUILayout.Width(120), GUILayout.Height(40)))
                {
                    brushing = false;
                    if (MapEditor.objects["BrushObj"] != null)
                    {
                        DestroyImmediate(((MapEditorBrush)MapEditor.objects["BrushObj"]).gameObject);
                        MapEditor.objects["BrushObj"] = null;
                    }
                }
                SceneVisibilityManager.instance.DisableAllPicking();
            }
            GUILayout.Space(5);

            GUILayout.BeginVertical();

            MapEditor.objects["brushSize"] = Mathf.Clamp(EditorGUILayout.FloatField("Brush Size", (float)MapEditor.objects["brushSize"], GUILayout.Width(200)), 0.1f, 100f);


            Vector2 temp = EditorGUILayout.Vector2Field("Grid Interval", (Vector2)MapEditor.objects["gridInterval"], GUILayout.Width(200));
            MapEditor.objects["gridInterval"] = new Vector2(Mathf.Clamp(temp.x, 0.1f, 100f), Mathf.Clamp(temp.y, 0.1f, 100f));
            
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            MapEditor.objects["gridHeight"] = Mathf.Clamp(EditorGUILayout.FloatField("Grid Height", (float)MapEditor.objects["gridHeight"], GUILayout.Width(200)), 0.0f, 100f);
            MapEditor.objects["brushParent"] = EditorGUILayout.ObjectField("BrushParent", (Object)MapEditor.objects["brushParent"], typeof(Transform), true, GUILayout.Width(200));
            MapEditor.objects["brushDensity"] = Mathf.Clamp(EditorGUILayout.IntField("Brush Density", (int)MapEditor.objects["brushDensity"], GUILayout.Width(200)), 1, 100);
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            MapEditor.objects["gridActive"] = EditorGUILayout.Toggle("Grid Active", (bool)MapEditor.objects["gridActive"], GUILayout.Width(200));
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal(GUI.skin.box, GUILayout.Height(60));

            DrawBrushes();

            GUILayout.EndHorizontal();
        }


        private void DrawBrushes()
        {
            for (int i = 0; i < 3; i++)
            {
                if ((int)MapEditor.objects["brushIndex"] == i)
                {
                    GUI.Box(new Rect(2.5f + 82.5f * i, 70, 70, 70), "", red);
                }
                if (GUILayout.Button(brushes[string.Concat("Brush", i + 1)].texutre, GUILayout.Width(60), GUILayout.Height(60)))
                {
                    MapEditor.objects["brushIndex"] = i;
                }
                if (i < 2) GUILayout.Space(20);
            }
        }
    }
    
    #endif
}
