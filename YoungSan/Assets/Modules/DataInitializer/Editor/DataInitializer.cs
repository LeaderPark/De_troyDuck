using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DataInitializer : MonoBehaviour
{
    public static EditorWindow window;

    public const int width = 800;
    public const int height = 500;

    [MenuItem("DataInitializer/DataInitializer Wizard")]
    public static void HierarchySearcher_Show()
    {
        window = EditorWindow.GetWindow<DataInitializerWindow>();
        window.titleContent = new GUIContent("DataInitializer Wizard");
        Rect WindowRect = window.position;
        WindowRect.x = Screen.width;
        WindowRect.width = width;
        WindowRect.height = height;
        window.position = WindowRect;
        window.minSize = new Vector2(width, height / 10);
        window.maxSize = new Vector2(width, height);
    }
}
