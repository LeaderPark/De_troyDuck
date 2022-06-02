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

    [HideInInspector] public HashSet<int> focusVertex;
    [HideInInspector] public HashSet<int> focusQuad;
    public bool viewMode;
    [HideInInspector] public Vertex newVertex;
    [HideInInspector] public Quad newQuad;

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
        if (Selection.activeGameObject != gameObject && Selection.activeGameObject != null) focusVertex.Clear();
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

            DrawSelectedQuad(sceneView);
            DrawSelectedVertex(sceneView);
        }
        else
        {
            focusVertex.Clear();
            focusQuad.Clear();
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
                        if (focusVertex.Count > 0)
                        {
                            int vertexCount = vertices.Count;
                            foreach (var item in focusVertex)
                            {
                                Vertex v = new Vertex();
                                v.position = vertices[item].position;
                                vertices.Add(v);
                            }
                            focusVertex.Clear();
                            for (int i = vertexCount; i < vertices.Count; i++)
                            {
                                focusVertex.Add(i);
                            }
                        }
                        break;
                    case KeyCode.F2:
                        if (focusVertex.Count > 0)
                        {
                            DeleteVertex();
                            focusVertex.Clear();
                        }
                        break;
                    case KeyCode.F5:
                        if (focusVertex.Count == 1)
                        {
                            var enumerator = focusVertex.GetEnumerator();
                            enumerator.MoveNext();
                            newQuad.v1 = enumerator.Current;
                        }
                        break;
                    case KeyCode.F6:
                        if (focusVertex.Count == 1)
                        {
                            var enumerator = focusVertex.GetEnumerator();
                            enumerator.MoveNext();
                            newQuad.v2 = enumerator.Current;
                        }
                        break;
                    case KeyCode.F7:
                        if (focusVertex.Count == 1)
                        {
                            var enumerator = focusVertex.GetEnumerator();
                            enumerator.MoveNext();
                            newQuad.v3 = enumerator.Current;
                        }
                        break;
                    case KeyCode.F8:
                        if (focusVertex.Count == 1)
                        {
                            var enumerator = focusVertex.GetEnumerator();
                            enumerator.MoveNext();
                            newQuad.v4 = enumerator.Current;
                        }
                        break;
                    case KeyCode.F3:

                        Quad q = new Quad();
                        q.v1 = newQuad.v1;
                        q.v2 = newQuad.v2;
                        q.v3 = newQuad.v3;
                        q.v4 = newQuad.v4;
                        indices.Add(q);
                        focusVertex.Clear();
                        focusQuad.Clear();
                        focusQuad.Add(indices.Count - 1);

                        break;
                    case KeyCode.F4:
                        focusVertex.Clear();
                        focusQuad.Clear();
                        for (int i = 0; i < vertices.Count; i++)
                        {
                            focusVertex.Add(i);
                        }
                        break;
                }
            }
        }

    }

    private void DrawSelectedQuad(SceneView sceneView)
    {
        var enumerator = focusQuad.GetEnumerator();
        enumerator.MoveNext();

        float cameraDistance = Vector3.Distance(sceneView.camera.transform.position, vertices[indices[enumerator.Current].v1].position);
        Vector3 cameraNormal = (sceneView.camera.transform.position - vertices[indices[enumerator.Current].v1].position).normalized;

        Handles.color = Color.red;
        foreach (var item in focusQuad)
        {
            Quad quad = indices[item];
            Handles.DrawLine(vertices[quad.v1].position, vertices[quad.v2].position);
            Handles.DrawLine(vertices[quad.v2].position, vertices[quad.v3].position);
            Handles.DrawLine(vertices[quad.v3].position, vertices[quad.v4].position);
            Handles.DrawLine(vertices[quad.v4].position, vertices[quad.v1].position);
        }

        if (focusQuad.Count > 0)
        {
            Vector3 movePosition = vertices[indices[enumerator.Current].v1].position;
            movePosition = Handles.DoPositionHandle(movePosition, Quaternion.identity);
            Handles.color = Color.red;
            movePosition = Handles.FreeMoveHandle(movePosition, Quaternion.LookRotation(cameraNormal), cameraDistance * 0.02f, Vector3.one, Handles.CircleHandleCap);

            Vector3 deltaMove = movePosition - vertices[indices[enumerator.Current].v1].position;
            HashSet<int> h = new HashSet<int>();
            foreach (var item in focusQuad)
            {
                h.Add(indices[item].v1);
                h.Add(indices[item].v2);
                h.Add(indices[item].v3);
                h.Add(indices[item].v4);
            }

            foreach (var item in h)
            {
                vertices[item].position += deltaMove;
            }
        }
    }

    private void DrawSelectedVertex(SceneView sceneView)
    {
        var enumerator = focusVertex.GetEnumerator();
        enumerator.MoveNext();

        float cameraDistance = Vector3.Distance(sceneView.camera.transform.position, vertices[enumerator.Current].position);
        Vector3 cameraNormal = (sceneView.camera.transform.position - vertices[enumerator.Current].position).normalized;

        Handles.color = Color.red;
        foreach (var item in focusVertex)
        {
            float cD = Vector3.Distance(sceneView.camera.transform.position, vertices[item].position);
            Vector3 cN = (sceneView.camera.transform.position - vertices[item].position).normalized;
            Handles.DrawWireDisc(vertices[item].position, cN, cD * 0.02f);
        }

        if (focusVertex.Count > 0)
        {
            Vector3 movePosition = vertices[enumerator.Current].position;
            movePosition = Handles.DoPositionHandle(movePosition, Quaternion.identity);
            Handles.color = Color.red;
            movePosition = Handles.FreeMoveHandle(movePosition, Quaternion.LookRotation(cameraNormal), cameraDistance * 0.02f, Vector3.one, Handles.CircleHandleCap);

            Vector3 deltaMove = movePosition - vertices[enumerator.Current].position;

            foreach (var item in focusVertex)
            {
                vertices[item].position += deltaMove;
            }
        }
    }

    public void DeleteVertex()
    {
        List<int> focusVertexList = new List<int>();
        foreach (var item in focusVertex)
        {
            focusVertexList.Add(item);
        }

        focusVertexList.Sort((a, b) => { return b.CompareTo(a); });
        foreach (var item in focusVertexList)
        {
            vertices.RemoveAt(item);
            CheckQuad(item);
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
        Handles.color = Color.white;
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
            if (!Event.current.control)
            {
                focusQuad.Clear();
            }
            focusQuad.Add(index);
            focusVertex.Clear();
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
            if (!Event.current.control)
            {
                focusVertex.Clear();
            }
            focusVertex.Add(index);
            focusQuad.Clear();
        }
    }

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (mainMesh == null)
        {
            mainMesh = new Mesh();
            mainMesh.name = "Custom Collider";
        }
        if (focusVertex == null)
        {
            focusVertex = new HashSet<int>();
        }
        if (focusQuad == null)
        {
            focusQuad = new HashSet<int>();
        }
        if (vertices == null)
        {
            vertices = new List<Vertex>();
        }
        if (indices == null)
        {
            indices = new List<Quad>();
        }
        if (newVertex == null)
        {
            newVertex = new Vertex();
        }
        if (newQuad == null)
        {
            newQuad = new Quad();
        }
    }
}

[CustomEditor(typeof(CustomCollider))]
public class CustomColliderEditor : Editor
{
    private Vector2 scrollPosition;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CustomCollider customCollider = target as CustomCollider;

        customCollider.Init();

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

        if (customCollider.focusQuad.Count > 0)
        {
            GUILayout.Label("Selected Quad (" + customCollider.focusQuad.Count + ")");
            for (int i = 0; i < customCollider.vertices.Count; i++)
            {
                if (customCollider.focusQuad.Contains(i))
                {
                    GUILayout.Label("Quad (" + i + ")");
                    DrawQuad(customCollider.indices[i], customCollider);

                    GUILayout.Space(10);
                }
            }

            if (GUILayout.Button("Delete Selected Quads"))
            {
                List<int> focusQuadList = new List<int>();
                foreach (var item in customCollider.focusQuad)
                {
                    focusQuadList.Add(item);
                }
                focusQuadList.Sort((a, b) => { return b.CompareTo(a); });
                foreach (var item in focusQuadList)
                {
                    customCollider.indices.RemoveAt(item);
                }
                customCollider.focusQuad.Clear();
            }
            GUILayout.Space(20);
        }

        if (customCollider.focusVertex.Count > 0)
        {
            GUILayout.Label("Selected Vertex (" + customCollider.focusVertex.Count + ")");
            for (int i = 0; i < customCollider.vertices.Count; i++)
            {
                if (customCollider.focusVertex.Contains(i))
                {
                    GUILayout.Label("Vertex (" + i + ")");
                    DrawVertex(customCollider.vertices[i]);

                    GUILayout.Space(10);
                }
            }

            if (GUILayout.Button("Delete Selected Vertices"))
            {
                customCollider.DeleteVertex();
                customCollider.focusVertex.Clear();
            }
            GUILayout.Space(20);
        }

        GUILayout.BeginVertical(GUI.skin.box);

        GUILayout.BeginVertical(GUI.skin.window);
        GUILayout.Label("New Quad");
        DrawQuad(customCollider.newQuad, customCollider);
        GUILayout.Space(20);

        if (GUILayout.Button("Create Quad"))
        {
            Quad q = new Quad();
            q.v1 = customCollider.newQuad.v1;
            q.v2 = customCollider.newQuad.v2;
            q.v3 = customCollider.newQuad.v3;
            q.v4 = customCollider.newQuad.v4;
            customCollider.indices.Add(q);
            customCollider.focusVertex.Clear();
            customCollider.focusQuad.Clear();
            customCollider.focusQuad.Add(customCollider.indices.Count - 1);
        }
        GUILayout.Space(20);
        GUILayout.EndVertical();
        GUILayout.Space(10);

        GUILayout.BeginVertical(GUI.skin.window);
        GUILayout.Label("New Vertex");
        DrawVertex(customCollider.newVertex);
        GUILayout.Space(20);

        if (GUILayout.Button("Create Vertex"))
        {
            Vertex v = new Vertex();
            v.position = customCollider.newVertex.position;
            customCollider.vertices.Add(v);
            customCollider.focusVertex.Clear();
            customCollider.focusQuad.Clear();
            customCollider.focusVertex.Add(customCollider.vertices.Count - 1);
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
                    var enumerator = customCollider.focusVertex.GetEnumerator();
                    enumerator.MoveNext();
                    if (customCollider.focusVertex.Count == 1)
                    {
                        quad.v1 = enumerator.Current;
                    }
                }
                if (GUILayout.Button("Select Vertex"))
                {
                    customCollider.focusVertex.Clear();
                    customCollider.focusVertex.Add(quad.v1);
                }
                GUILayout.EndVertical();
            }
            {
                GUILayout.BeginVertical();
                quad.v2 = EditorGUILayout.IntField(quad.v2, GUILayout.Width(80));
                if (GUILayout.Button("Set Vertex"))
                {
                    var enumerator = customCollider.focusVertex.GetEnumerator();
                    enumerator.MoveNext();
                    if (customCollider.focusVertex.Count == 1)
                    {
                        quad.v2 = enumerator.Current;
                    }
                }
                if (GUILayout.Button("Select Vertex"))
                {
                    customCollider.focusVertex.Clear();
                    customCollider.focusVertex.Add(quad.v2);
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
                    var enumerator = customCollider.focusVertex.GetEnumerator();
                    enumerator.MoveNext();
                    if (customCollider.focusVertex.Count == 1)
                    {
                        quad.v3 = enumerator.Current;
                    }
                }
                if (GUILayout.Button("Select Vertex"))
                {
                    customCollider.focusVertex.Clear();
                    customCollider.focusVertex.Add(quad.v3);
                }
                GUILayout.EndVertical();
            }
            {
                GUILayout.BeginVertical();
                quad.v4 = EditorGUILayout.IntField(quad.v4, GUILayout.Width(80));
                if (GUILayout.Button("Set Vertex"))
                {
                    var enumerator = customCollider.focusVertex.GetEnumerator();
                    enumerator.MoveNext();
                    if (customCollider.focusVertex.Count == 1)
                    {
                        quad.v4 = enumerator.Current;
                    }
                }
                if (GUILayout.Button("Select Vertex"))
                {
                    customCollider.focusVertex.Clear();
                    customCollider.focusVertex.Add(quad.v4);
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