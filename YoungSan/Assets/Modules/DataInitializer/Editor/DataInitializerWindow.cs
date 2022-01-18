using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DataInitializerWindow : EditorWindow
{
    int selectIndex;

    Vector2 leftScrollViewPosition;
    Vector2 rightScrollViewPosition;

    private List<string> dataNameList;
    
    private string assetPath;


    public DataInitializerWindow()
    {
        selectIndex = 0;
        dataNameList = new List<string>();
        assetPath = "Asset/";
    }
    
    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        
        LeftScrollView();

        GUILayout.Space(10);

        RightScrollView();

        EditorGUILayout.EndHorizontal();
    }

    private void LeftScrollView()
    {
        leftScrollViewPosition = EditorGUILayout.BeginScrollView(leftScrollViewPosition, false, false, GUIStyle.none, GUIStyle.none, GUI.skin.window, GUILayout.Width(200));

        CreateNewData();

        RemoveData();

        LoadDatas();

        EditorGUILayout.EndScrollView();
    }

    private void CreateNewData()
    {
        if (GUILayout.Button("Create New Entity"))
        {
            string name = "New Entity";
            dataNameList.Add(name);

            // GameObject obj = new GameObject(name);
            // PrefabUtility.SaveAsPrefabAsset(obj, assetPath + "/Prefabs/" + category.ToString() + "Data/" + name + ".prefab");
            // Destroy(obj);
            // EntityData data = new EntityData();
            // AssetDatabase.CreateAsset(data, assetPath + "/ScriptableObjects/" + category.ToString() + "Data/" + name + ".asset");
        }

        GUILayout.Space(5);
    }

    private void RemoveData()
    {
        if (GUILayout.Button("Remove Entity"))
        {
            if (dataNameList.Count > selectIndex)
            {
                dataNameList.RemoveAt(selectIndex);
                selectIndex = Mathf.Clamp(selectIndex, 1, dataNameList.Count) - 1;
            }
        }
    }

    private void LoadDatas()
    {
        GUILayout.Space(10);
        EditorGUILayout.BeginVertical(GUI.skin.window);
        
        GUIStyle selectedStyle = GUI.skin.button;
        for (int i = 0; i < dataNameList.Count; i++)
        {
            if (selectIndex == i)
            {
                selectedStyle = GUI.skin.box;
            }
            else
            {
                selectedStyle = GUI.skin.button;
            }
            if (GUILayout.Button(dataNameList[i], selectedStyle, GUILayout.Width(180)))
            {
                selectIndex = i;
            }
            GUILayout.Space(5);
        }
        EditorGUILayout.EndVertical();
    }

    private void RightScrollView()
    {
        rightScrollViewPosition = EditorGUILayout.BeginScrollView(rightScrollViewPosition);

        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("Entity", EditorStyles.whiteLargeLabel);

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
    }
}