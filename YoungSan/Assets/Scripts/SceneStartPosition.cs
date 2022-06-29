using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneStartPosition", menuName = "ScriptableObjects/SceneStartPosition", order = 1)]
public class SceneStartPosition : ScriptableObject
{
    public string beginScene;
    public string endScene;
    public Vector3 position;
#if UNITY_EDITOR


    void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (endScene != UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name) return;
        if (Selection.objects.Length <= 0 || !(Selection.objects[0] is SceneStartPosition) || ((SceneStartPosition)Selection.objects[0]).name != name) return;

        float handleSize = HandleUtility.GetHandleSize(position);

        position = Handles.DoPositionHandle(position, Quaternion.identity);

        Handles.color = Color.red;
        Handles.DrawWireCube(position + Vector3.up * handleSize / 2, Vector3.one * handleSize);

        string labelContent = beginScene + " -> " + endScene;
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = (int)(handleSize * 3);
        style.normal.textColor = Color.green;

        Handles.Label(position, labelContent, style);
    }
#endif
}
