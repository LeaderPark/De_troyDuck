using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshCollider))]
public class CustomCollider : MonoBehaviour
{
    public Mesh mainMesh;

    public Vector3[] vertices;
    public int[] indices;

    void Awake()
    {
        mainMesh = new Mesh();
        vertices = new Vector3[0];
        indices = new int[0];
    }
}

[CustomEditor(typeof(CustomCollider))]
public class CustomColliderEditor : Editor
{
    private Vector3 newVertex;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CustomCollider customCollider = target as CustomCollider;

        if (GUILayout.Button("Apply Mesh"))
        {
            MeshCollider meshCollider = customCollider.GetComponent<MeshCollider>();
            meshCollider.sharedMesh = customCollider.mainMesh;
        }

        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("New Vertex");
        newVertex = EditorGUILayout.Vector3Field("position", newVertex);
        GUILayout.EndVertical();
    }
}
