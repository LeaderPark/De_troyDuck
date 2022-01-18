using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DataInitializerWindow : EditorWindow
{
    DataCategory category;
    int selectIndex;

    Vector2 leftScrollViewPosition;
    Vector2 rightScrollViewPosition;

    private List<string> dataNameList;
    
    private string assetPath;


    public DataInitializerWindow()
    {
        category = DataCategory.Entity;
        selectIndex = 0;
        dataNameList = new List<string>();
        assetPath = "Asset/";
    }
    
    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal(GUI.skin.box, GUILayout.Height(20));

        TopCategory();

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        
        LeftScrollView();

        GUILayout.Space(10);

        RightScrollView();

        EditorGUILayout.EndHorizontal();
    }

    private void TopCategory()
    {
        GUIStyle selectedStyle = GUI.skin.button;
        foreach (var item in System.Enum.GetNames(typeof(DataCategory)))
        {
            if (category.ToString() == item)
            {
                selectedStyle = GUI.skin.box;
            }
            else
            {
                selectedStyle = GUI.skin.button;
            }
            if (GUILayout.Button(item, selectedStyle, GUILayout.Width(100)))
            {
                category = (DataCategory)System.Enum.Parse(typeof(DataCategory), item);
                dataNameList.Clear();
                selectIndex = 0;
            }
        }
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
        if (GUILayout.Button("Create New " + category.ToString()))
        {
            string name = "New " + category.ToString();
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
        if (GUILayout.Button("Remove " + category.ToString()))
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

        EditorGUILayout.LabelField(category.ToString(), EditorStyles.whiteLargeLabel);

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
    }
}

public enum DataCategory
{
    Entity,
    Skill
}