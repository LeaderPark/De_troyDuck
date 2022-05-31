using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

[ExecuteInEditMode]
[RequireComponent(typeof(MeshCollider))]
public class CustomCollider : MonoBehaviour
{

    [HideInInspector] public Mesh mainMesh;

    [HideInInspector] public List<Vertex> vertices;
    [HideInInspector] public List<Quad> indices;

    [HideInInspector] public int focusVertex;
    [HideInInspector] public int focusQuad;
    public bool viewMode;

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
        Init();
        if (Selection.activeGameObject != gameObject && Selection.activeGameObject != null) focusVertex = -1;
        if (viewMode)
        {
            InputProcess();
            for (int i = 0; i < indices.Count; i++)
            {
                DrawQuad(sceneView, indices[i], i);
            }
            for (int i = 0; i < vertices.Count; i++)
            {
                DrawVertex(sceneView, vertices[i], i);
            }
        }
        else
        {
            focusVertex = -1;
            focusQuad = -1;
        }
    }

    void InputProcess()
    {
        Event e = Event.current;

        if (e.isKey)
        {
            if (e.type == EventType.KeyDown)
            {
                switch (e.keyCode)
                {
                    case KeyCode.F1:
                        if (focusVertex != -1)
                        {
                            Vertex v = new Vertex();
                            v.position = vertices[focusVertex].position;
                            vertices.Add(v);
                            focusVertex = vertices.Count - 1;
                        }
                        break;
                    case KeyCode.F2:
                        if (focusVertex != -1)
                        {
                            vertices.RemoveAt(focusVertex);
                            CheckQuad(focusVertex);
                            focusVertex = -1;
                        }
                        break;
                }
            }
        }

    }

    public void CheckQuad(int index)
    {
        for (int i = 0; i < indices.Count; i++)
        {
            if (indices[i].v1 == index) { indices.RemoveAt(i); i--; continue; }
            else
            if (indices[i].v2 == index) { indices.RemoveAt(i); i--; continue; }
            else
            if (indices[i].v3 == index) { indices.RemoveAt(i); i--; continue; }
            else
            if (indices[i].v4 == index) { indices.RemoveAt(i); i--; continue; }

            if (indices[i].v1 > index) indices[i].v1--;
            if (indices[i].v2 > index) indices[i].v2--;
            if (indices[i].v3 > index) indices[i].v3--;
            if (indices[i].v4 > index) indices[i].v4--;
        }
    }

    void DrawQuad(SceneView sceneView, Quad quad, int index)
    {
        Handles.DrawLine(vertices[quad.v1].position, vertices[quad.v2].position);
        Handles.DrawLine(vertices[quad.v2].position, vertices[quad.v3].position);
        Handles.DrawLine(vertices[quad.v3].position, vertices[quad.v4].position);
        Handles.DrawLine(vertices[quad.v4].position, vertices[quad.v1].position);

        Vector3 v1v3 = Vector3.Lerp(vertices[quad.v1].position, vertices[quad.v3].position, 0.5f);
        Vector3 v2v4 = Vector3.Lerp(vertices[quad.v2].position, vertices[quad.v4].position, 0.5f);

        Vector3 pos = Vector3.Lerp(v1v3, v2v4, 0.5f);
        //float size = Mathf.Min(Mathf.Min(Vector3.Distance(vertices[quad.v1].position, vertices[quad.v2].position), Vector3.Distance(vertices[quad.v3].position, vertices[quad.v4].position)), Mathf.Min(Vector3.Distance(vertices[quad.v1].position, vertices[quad.v4].position), Vector3.Distance(vertices[quad.v2].position, vertices[quad.v3].position))) / 2;

        Handles.color = Color.white;
        if (Handles.Button(pos, Quaternion.identity, 0.02f, 0.02f, Handles.RectangleHandleCap))
        {
            focusQuad = index;
            focusVertex = -1;
        }
    }

    void DrawVertex(SceneView sceneView, Vertex vertex, int index)
    {
        float cameraDistance = Vector3.Distance(sceneView.camera.transform.position, vertex.position);
        Vector3 cameraNormal = (sceneView.camera.transform.position - vertex.position).normalized;

        Rect rect = new Rect();
        rect.size = Vector2.one * 0.1f;
        rect.position = HandleUtility.WorldToGUIPoint(vertex.position) - Vector2.one * 0.05f;

        Handles.color = Color.white;
        Handles.DrawSolidDisc(vertex.position, cameraNormal, cameraDistance * 0.015f);
        Handles.color = Color.green;
        Handles.DrawSolidDisc(vertex.position, cameraNormal, cameraDistance * 0.01f);

        Handles.color = Color.white;
        if (Handles.Button(vertex.position, Quaternion.LookRotation(cameraNormal), cameraDistance * 0.02f, cameraDistance * 0.02f, Handles.CircleHandleCap))
        {
            focusVertex = index;
            focusQuad = -1;
        }

        if (focusVertex == index)
        {
            vertex.position = Handles.DoPositionHandle(vertex.position, Quaternion.identity);
            Handles.color = Color.red;
            vertex.position = Handles.FreeMoveHandle(vertex.position, Quaternion.identity, cameraDistance * 0.02f, Vector3.one, Handles.CircleHandleCap);
        }
    }

    void Awake()
    {
        Init();
    }

    void Init()
    {
        if (mainMesh == null)
        {
            mainMesh = new Mesh();
            mainMesh.name = "Custom Collider";
            focusVertex = -1;
            focusQuad = -1;
        }
        if (vertices == null)
        {
            vertices = new List<Vertex>();
        }
        if (indices == null)
        {
            indices = new List<Quad>();
        }
    }
}

[CustomEditor(typeof(CustomCollider))]
public class CustomColliderEditor : Editor
{
    private Vector2 scrollPosition;
    private Vertex newVertex = new Vertex();
    private Quad newQuad = new Quad();

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CustomCollider customCollider = target as CustomCollider;

        GUILayout.Space(10);
        if (GUILayout.Button("Apply Mesh"))
        {
            MeshCollider meshCollider = customCollider.GetComponent<MeshCollider>();

            Vector3[] vertices = new Vector3[customCollider.vertices.Count];
            int[] indices = new int[customCollider.indices.Count * 4];

            for (int i = 0; i < customCollider.vertices.Count; i++)
            {
                vertices[i] = customCollider.vertices[i].position;
            }
            for (int i = 0; i < customCollider.indices.Count; i++)
            {
                indices[i * 4] = customCollider.indices[i].v1;
                indices[i * 4 + 1] = customCollider.indices[i].v2;
                indices[i * 4 + 2] = customCollider.indices[i].v3;
                indices[i * 4 + 3] = customCollider.indices[i].v4;
            }
            customCollider.mainMesh.SetVertices(vertices);
            customCollider.mainMesh.SetIndices(indices, MeshTopology.Quads, 0);

            meshCollider.sharedMesh = customCollider.mainMesh;
        }
        GUILayout.Space(20);
        GUILayout.Label("Vertex Count : " + customCollider.vertices.Count);
        GUILayout.Space(10);
        GUILayout.Label("Quad Count : " + customCollider.indices.Count);
        GUILayout.Space(20);

        if (customCollider.focusQuad != -1)
        {
            GUILayout.Label("Selected Quad");
            DrawQuad(customCollider.indices[customCollider.focusQuad], customCollider);

            GUILayout.Space(10);

            if (GUILayout.Button("Delete Selected Quad"))
            {
                customCollider.indices.RemoveAt(customCollider.focusQuad);
                customCollider.focusQuad = -1;
            }
            GUILayout.Space(20);
        }

        if (customCollider.focusVertex != -1)
        {
            GUILayout.Label("Selected Vertex");
            DrawVertex(customCollider.vertices[customCollider.focusVertex]);

            GUILayout.Space(10);

            if (GUILayout.Button("Delete Selected Vertex"))
            {
                customCollider.vertices.RemoveAt(customCollider.focusVertex);
                customCollider.CheckQuad(customCollider.focusVertex);
                customCollider.focusVertex = -1;
            }
            GUILayout.Space(20);
        }

        GUILayout.BeginVertical(GUI.skin.box);

        GUILayout.BeginVertical(GUI.skin.window);
        GUILayout.Label("New Quad");
        DrawQuad(newQuad, customCollider);
        GUILayout.Space(20);

        if (GUILayout.Button("Create Quad"))
        {
            Quad q = new Quad();
            q.v1 = newQuad.v1;
            q.v2 = newQuad.v2;
            q.v3 = newQuad.v3;
            q.v4 = newQuad.v4;
            customCollider.indices.Add(q);
            customCollider.focusQuad = customCollider.indices.Count - 1;
        }
        GUILayout.Space(20);
        GUILayout.EndVertical();
        GUILayout.Space(10);

        GUILayout.BeginVertical(GUI.skin.window);
        GUILayout.Label("New Vertex");
        DrawVertex(newVertex);
        GUILayout.Space(20);

        if (GUILayout.Button("Create Vertex"))
        {
            Vertex v = new Vertex();
            v.position = newVertex.position;
            customCollider.vertices.Add(v);
            customCollider.focusVertex = customCollider.vertices.Count - 1;
        }
        GUILayout.Space(20);
        GUILayout.EndVertical();

        GUILayout.EndVertical();
    }

    void DrawQuad(Quad quad, CustomCollider customCollider)
    {
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                quad.v1 = EditorGUILayout.IntField(quad.v1, GUILayout.Width(80));
                if (GUILayout.Button("Set Vertex"))
                {
                    quad.v1 = customCollider.focusVertex;
                }
                if (GUILayout.Button("Select Vertex"))
                {
                    customCollider.focusVertex = quad.v1;
                }
                GUILayout.EndVertical();
            }
            {
                GUILayout.BeginVertical();
                quad.v2 = EditorGUILayout.IntField(quad.v2, GUILayout.Width(80));
                if (GUILayout.Button("Set Vertex"))
                {
                    quad.v2 = customCollider.focusVertex;
                }
                if (GUILayout.Button("Select Vertex"))
                {
                    customCollider.focusVertex = quad.v2;
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                quad.v3 = EditorGUILayout.IntField(quad.v3, GUILayout.Width(80));
                if (GUILayout.Button("Set Vertex"))
                {
                    quad.v3 = customCollider.focusVertex;
                }
                if (GUILayout.Button("Select Vertex"))
                {
                    customCollider.focusVertex = quad.v3;
                }
                GUILayout.EndVertical();
            }
            {
                GUILayout.BeginVertical();
                quad.v4 = EditorGUILayout.IntField(quad.v4, GUILayout.Width(80));
                if (GUILayout.Button("Set Vertex"))
                {
                    quad.v4 = customCollider.focusVertex;
                }
                if (GUILayout.Button("Select Vertex"))
                {
                    customCollider.focusVertex = quad.v4;
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
    }

    void DrawVertex(Vertex vertex)
    {
        vertex.position = EditorGUILayout.Vector3Field("position", vertex.position);
    }
}

[System.Serializable]
public class Vertex
{
    [SerializeField]
    public Vector3 position;
}

[System.Serializable]
public class Quad
{
    [SerializeField] public int v1;
    [SerializeField] public int v2;
    [SerializeField] public int v3;
    [SerializeField] public int v4;
}

#endif