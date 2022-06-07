using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
public class MergeCustomColliderWindow : EditorWindow
{
    private CustomCollider source;
    private CustomCollider target;

    void OnGUI()
    {
        GUILayout.Space(10);
        source = EditorGUILayout.ObjectField("Source", source, typeof(CustomCollider), true) as CustomCollider;
        target = EditorGUILayout.ObjectField("Target", target, typeof(CustomCollider), true) as CustomCollider;
        GUILayout.Space(20);
        if (GUILayout.Button("Setting"))
        {
            Merge();
        }
    }

    private void Merge()
    {
        int sourceVertexCount = source.vertices.Count;
        source.vertices.AddRange(target.vertices);

        List<Quad> qTemp = new List<Quad>();
        qTemp.AddRange(target.indices);
        DestroyImmediate(target);

        foreach (var item in qTemp)
        {
            item.v1 += sourceVertexCount;
            item.v2 += sourceVertexCount;
            item.v3 += sourceVertexCount;
            item.v4 += sourceVertexCount;
        }

        source.indices.AddRange(qTemp);
    }
}
#endif

