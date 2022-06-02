using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MergeCustomCollider : MonoBehaviour
{
    public static EditorWindow window;

    public const int width = 300;
    public const int height = 100;

    [MenuItem("Window/MergeCustomCollider")]
    public static void MergeCustomCollider_Show()
    {
        window = EditorWindow.GetWindow<MergeCustomColliderWindow>();
        window.titleContent = new GUIContent("MergeCustomCollider");
        Rect WindowRect = window.position;
        WindowRect.x = Screen.width;
        WindowRect.width = width;
        WindowRect.height = height;
        window.position = WindowRect;
        window.minSize = new Vector2(width, height / 10);
        window.maxSize = new Vector2(width, height);
    }
}
