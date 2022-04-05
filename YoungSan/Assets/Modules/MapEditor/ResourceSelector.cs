using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MapEditor
{
     #if UNITY_EDITOR
    public class ResourceSelector : EditorWindow
    {

        Vector2 scrollPos;

        void OnEnable()
        {
            RefreshResources();
            scrollPos = Vector2.zero;
            MapEditor.objects["ResourceIndex"] = 0;
        }

        void RefreshResources()
        {
            MapEditor.resources.Clear();
            foreach (GameObject obj in Resources.LoadAll<GameObject>("MapEditorPrefabs"))
            {
                MapEditor.resources.Add(obj);
            }
        }

        private void OnGUI()
        {   
            GUI.Box(new Rect(0, 0, position.width, position.height), new Texture2D((int)position.width, (int)position.height));  
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Width(position.width), GUILayout.Height(position.height - 100));
            
            int row = ((int)position.width - 20) / 80;
            int column = MapEditor.resources.Count / row;
            
            GUILayout.Space(20);

            for (int i = 0; i < column + 1; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                int count = row;
                if (i == column) count = MapEditor.resources.Count % row;
                else count = row;
                for (int j = 0; j < count; j++)
                {
                    int index = i * row + j;
                    if ((int)MapEditor.objects["ResourceIndex"] == index)
                    {
                        GUI.Box(new Rect(18.5f + j * 88f, 15f + i * 110f, 70, 70), "", GUI.skin.window);
                    }
                    GUILayout.BeginVertical(GUILayout.Width(60), GUILayout.Height(80));
                    if (GUILayout.Button(AssetPreview.GetAssetPreview(MapEditor.resources[index]), GUILayout.Width(60), GUILayout.Height(60)))
                    {
                        MapEditor.objects["ResourceIndex"] = index;
                        Selection.activeGameObject = MapEditor.resources[(int)MapEditor.objects["ResourceIndex"]];
                    }
                    GUILayout.Box(MapEditor.resources[index].name, GUILayout.Width(60), GUILayout.Height(20));
                    
                    GUILayout.EndVertical();
                    GUILayout.Space(20);
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(20);
            }

            GUILayout.EndScrollView();


            Event e = Event.current;
            switch (e.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                if (!new Rect(0, 0, position.width, position.height - 100).Contains(e.mousePosition)) break;
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                if (e.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    foreach (GameObject obj in DragAndDrop.objectReferences)
                    {
                        if (obj != null)
                        {
                            bool success = false;
                            string objectPath = string.Concat("Assets/Modules/MapEditor/Resources/MapEditorPrefabs/", obj.name, ".prefab");
                            for (int i = 1; i < 100; i++)
                            {
                                if (System.IO.File.Exists(objectPath))
                                {
                                    objectPath = string.Concat("Assets/Modules/MapEditor/Resources/MapEditorPrefabs/", obj.name, "(", i, ").prefab");
                                }
                                else break;
                            }

                            if (PrefabUtility.IsPartOfAnyPrefab(obj))
                            {
                                string path = AssetDatabase.GetAssetPath(obj);
                                if (path == string.Empty)
                                {
                                    PrefabUtility.SaveAsPrefabAsset(obj, objectPath, out success);
                                }
                                else
                                {
                                    success = AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(obj), objectPath);
                                }
                            }
                            else
                            {
                                PrefabUtility.SaveAsPrefabAsset(obj, objectPath, out success);
                            }

                            if (success)
                            {
                                RefreshResources();
                            }
                        }
                    }
                }
                Event.current.Use();
                break;
            }


            if (GUILayout.Button("Remove", GUILayout.Width(position.width)))
            {
                if (MapEditor.resources.Count > 0)
                {
                    if (EditorUtility.DisplayDialog("정말 삭제할거야?", "이걸 삭제한다고? 진짜?\n" + MapEditor.resources[(int)MapEditor.objects["ResourceIndex"]].name + " 이거 맞지? 맞는거지?", "응", "아니"))
                    {
                        string path = AssetDatabase.GetAssetPath(MapEditor.resources[(int)MapEditor.objects["ResourceIndex"]]);
                        System.IO.File.Delete(path);
                        System.IO.File.Delete(string.Concat(path, ".meta"));
                        AssetDatabase.Refresh();
                        MapEditor.resources.RemoveAt((int)MapEditor.objects["ResourceIndex"]);
                        MapEditor.objects["ResourceIndex"] = Mathf.Clamp((int)MapEditor.objects["ResourceIndex"], 0, MapEditor.resources.Count - 1);
                        if (MapEditor.resources.Count > 0) Selection.activeGameObject = MapEditor.resources[(int)MapEditor.objects["ResourceIndex"]];
                    }
                }
            }
        }

    }
    #endif
}