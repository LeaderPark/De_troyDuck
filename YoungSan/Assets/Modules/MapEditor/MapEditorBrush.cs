using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace MapEditor
{
    [ExecuteAlways]
    public class MapEditorBrush : MonoBehaviour
    {
        public Ray ray;
        public Vector3 hitPoint;

        void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
            table = new Hashtable();

            StartCoroutine(Brush2Lock());
        }

        void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            StopAllCoroutines();
        }

        IEnumerator Brush2Lock()
        {
            while (true)
            {
                if (brush2lock)
                {
                    yield return new WaitForSeconds(0.2f);
                    brush2lock = false;
                }
                yield return null;
            }
        }

        Hashtable table;

        void Update()
        {
            lock(lockObject)
            {
                foreach ((GameObject, Vector3) item in table.Values)
                {
                    GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(item.Item1);
                    
                    obj.transform.position = item.Item2;
                    if ((Transform)MapEditor.objects["brushParent"] == null)
                    {
                        MapEditor.objects["brushParent"] = new GameObject("brushParent").transform;
                    }
                    obj.transform.parent = (Transform)MapEditor.objects["brushParent"];
                    if ((bool)MapEditor.objects["spriteMode"])
                    {
                        obj.GetComponent<SpriteRenderer>().sprite = (Sprite)MapEditor.objects["sprite"];
                    }
                }
                table.Clear();
            }
            if (EditorWindow.focusedWindow.GetType().ToString() != "UnityEditor.ObjectSelector") EditorWindow.GetWindow<BrushSelector>().Repaint();
        }

        private bool mouseDown = false;
        private object lockObject = new object();

        private bool brush2lock = false;
#endif
#if UNITY_EDITOR
        void OnSceneGUI(SceneView sceneView)
        {
            DrawDefaultPreview();

            switch (Event.current.type)
            {
                case EventType.MouseDown:
                if (Event.current.isMouse && Event.current.button == 0)
                {
                    mouseDown = true;
                    lock (lockObject)
                    {
                        DrawTile();
                    }
                }
                else mouseDown = false;
                break;
                case EventType.MouseUp:
                if (Event.current.isMouse && Event.current.button == 0)
                {
                    mouseDown = false;
                }
                break;
                case EventType.MouseDrag:
                if (mouseDown)
                {
                    lock (lockObject)
                    {
                        DrawTile();
                    }
                }
                break;
            }
        }
#endif
#if UNITY_EDITOR
    void DrawDefaultPreview()
        {
            Vector3 mousepos = Event.current.mousePosition;
            ray = HandleUtility.GUIPointToWorldRay(mousepos);
            
            Vector3 pos100 = ray.origin + ray.direction * 100;
            float distance = 100 * (Mathf.Abs(ray.origin.y) - (float)MapEditor.objects["gridHeight"]) / Mathf.Abs(ray.origin.y - pos100.y);
            
            hitPoint = ray.origin + ray.direction * distance;
            hitPoint.y = (float)MapEditor.objects["gridHeight"];
        }

        void OnDrawGizmos()
        {

            if ((bool)MapEditor.objects["gridActive"])
            {
                Gizmos.color = Color.grey;
                float height = (float)MapEditor.objects["gridHeight"];
                Vector2 gridInterval = (Vector2)MapEditor.objects["gridInterval"];
                
                Vector3 cameraPos = SceneView.GetAllSceneCameras()[0].transform.position;
                cameraPos.y = 0;
                Vector3 gridPivot = new Vector3((int)(cameraPos.x / gridInterval.x), 0, (int)(cameraPos.z / gridInterval.y));
                gridPivot.x *= gridInterval.x;
                gridPivot.z *= gridInterval.y;
                for (float i = 0; i <= 100; i += gridInterval.x)
                {
                    Gizmos.DrawLine(gridPivot + new Vector3(i, height, -100), gridPivot + new Vector3(i, height, 100));
                    Gizmos.DrawLine(gridPivot + new Vector3(-i, height, -100), gridPivot + new Vector3(-i, height, 100));
                }
                for (float i = 0; i <= 100; i += gridInterval.y)
                {
                    Gizmos.DrawLine(gridPivot + new Vector3(-100, height, i), gridPivot + new Vector3(100, height, i));
                    Gizmos.DrawLine(gridPivot + new Vector3(-100, height, -i), gridPivot + new Vector3(100, height, -i));
                }
            }

            void DrawRect(Vector3 position, Vector3 gridPivot, float height, Vector2 gridInterval)
            {
                Vector2 startIndex = new Vector2((int)(position.x / gridInterval.x) * gridInterval.x, (int)(position.z / gridInterval.y) * gridInterval.y);

                if (position.x <= 0 && position.z >= 0)
                {
                    Gizmos.DrawLine(gridPivot + new Vector3(startIndex.x, height, startIndex.y), gridPivot + new Vector3(startIndex.x, height, startIndex.y + gridInterval.y));
                    Gizmos.DrawLine(gridPivot + new Vector3(startIndex.x - gridInterval.x, height, startIndex.y), gridPivot + new Vector3(startIndex.x - gridInterval.x, height, startIndex.y + gridInterval.y));
                    Gizmos.DrawLine(gridPivot + new Vector3(startIndex.x, height, startIndex.y), gridPivot + new Vector3(startIndex.x - gridInterval.x, height, startIndex.y));
                    Gizmos.DrawLine(gridPivot + new Vector3(startIndex.x, height, startIndex.y + gridInterval.y), gridPivot + new Vector3(startIndex.x - gridInterval.x, height, startIndex.y + gridInterval.y));
                }
                else if (position.x >= 0 && position.z >= 0)
                {
                    Gizmos.DrawLine(gridPivot + new Vector3(startIndex.x, height, startIndex.y), gridPivot + new Vector3(startIndex.x, height, startIndex.y + gridInterval.y));
                    Gizmos.DrawLine(gridPivot + new Vector3(startIndex.x + gridInterval.x, height, startIndex.y), gridPivot + new Vector3(startIndex.x + gridInterval.x, height, startIndex.y + gridInterval.y));
                    Gizmos.DrawLine(gridPivot + new Vector3(startIndex.x, height, startIndex.y), gridPivot + new Vector3(startIndex.x + gridInterval.x, height, startIndex.y));
                    Gizmos.DrawLine(gridPivot + new Vector3(startIndex.x, height, startIndex.y + gridInterval.y), gridPivot + new Vector3(startIndex.x + gridInterval.x, height, startIndex.y + gridInterval.y));
                }
                else if (position.x <= 0 && position.z <= 0)
                {
                    Gizmos.DrawLine(gridPivot + new Vector3(startIndex.x, height, startIndex.y), gridPivot + new Vector3(startIndex.x, height, startIndex.y - gridInterval.y));
                    Gizmos.DrawLine(gridPivot + new Vector3(startIndex.x - gridInterval.x, height, startIndex.y), gridPivot + new Vector3(startIndex.x - gridInterval.x, height, startIndex.y - gridInterval.y));
                    Gizmos.DrawLine(gridPivot + new Vector3(startIndex.x, height, startIndex.y), gridPivot + new Vector3(startIndex.x - gridInterval.x, height, startIndex.y));
                    Gizmos.DrawLine(gridPivot + new Vector3(startIndex.x, height, startIndex.y - gridInterval.y), gridPivot + new Vector3(startIndex.x - gridInterval.x, height, startIndex.y - gridInterval.y));
                }
                else if (position.x >= 0 && position.z <= 0)
                {
                    Gizmos.DrawLine(gridPivot + new Vector3(startIndex.x, height, startIndex.y), gridPivot + new Vector3(startIndex.x, height, startIndex.y - gridInterval.y));
                    Gizmos.DrawLine(gridPivot + new Vector3(startIndex.x + gridInterval.x, height, startIndex.y), gridPivot + new Vector3(startIndex.x + gridInterval.x, height, startIndex.y - gridInterval.y));
                    Gizmos.DrawLine(gridPivot + new Vector3(startIndex.x, height, startIndex.y), gridPivot + new Vector3(startIndex.x + gridInterval.x, height, startIndex.y));
                    Gizmos.DrawLine(gridPivot + new Vector3(startIndex.x, height, startIndex.y - gridInterval.y), gridPivot + new Vector3(startIndex.x + gridInterval.x, height, startIndex.y - gridInterval.y));
                }
            }

            Gizmos.color = Color.red;
            switch ((int)MapEditor.objects["brushIndex"])
            {
                case 0:
                case 2:
                {
                    float height = (float)MapEditor.objects["gridHeight"];
                    float brushSize = (float)MapEditor.objects["brushSize"];
                    Vector2 gridInterval = (Vector2)MapEditor.objects["gridInterval"];
                    Vector3 cameraPos = SceneView.GetAllSceneCameras()[0].transform.position;
                    cameraPos.y = 0;
                    Vector3 gridPivot = new Vector3((int)(cameraPos.x / gridInterval.x), 0, (int)(cameraPos.z / gridInterval.y));
                    gridPivot.x *= gridInterval.x;
                    gridPivot.z *= gridInterval.y;
                    Vector3 temp = hitPoint - gridPivot;
                    for (float i = 0; i < brushSize; i++)
                    {
                        for (float j = 0; j < brushSize; j++)
                        {
                            DrawRect(temp + new Vector3(i - brushSize / 2, 0, j - brushSize / 2), gridPivot, height, gridInterval);
                        }
                    }
                }
                break;
                case 3:
                {
                    float height = (float)MapEditor.objects["gridHeight"];
                    Vector2 gridInterval = (Vector2)MapEditor.objects["gridInterval"];
                    Vector3 cameraPos = SceneView.GetAllSceneCameras()[0].transform.position;
                    cameraPos.y = 0;
                    Vector3 gridPivot = new Vector3((int)(cameraPos.x / gridInterval.x), 0, (int)(cameraPos.z / gridInterval.y));
                    gridPivot.x *= gridInterval.x;
                    gridPivot.z *= gridInterval.y;
                    Vector3 temp = hitPoint - gridPivot;
                    DrawRect(temp, gridPivot, height, gridInterval);
                }
                break;
                case 1:
                    Gizmos.DrawWireSphere(hitPoint, (float)MapEditor.objects["brushSize"] / 2f);
                break;
            }
            Gizmos.color = Color.white;
        }

        void DrawTile()
        {
            switch ((int)MapEditor.objects["brushIndex"])
            {
                case 0:
                DrawBrush1();
                break;
                case 1:
                DrawBrush2();
                break;
                case 2:
                DrawBrush3();
                break;
                case 3:
                DrawBrush4();
                break;
            }
        }

        void DrawBrush1()
        {
            Vector2 brushPos = new Vector2(hitPoint.x, hitPoint.z);
            Vector2 gridInterval = (Vector2)MapEditor.objects["gridInterval"];
            
            float height = (float)MapEditor.objects["gridHeight"];
            float brushSize = (float)MapEditor.objects["brushSize"];

            Vector2 rectPosDuration =  - Vector2.one * brushSize / 2f;
            Vector2 rectSize = gridInterval + Vector2.one * brushSize;

            System.Action<Vector2> setTile = (center) =>
            {
                Vector3 targetPos = new Vector3(center.x, hitPoint.y, center.y);
                
                if (((Transform)MapEditor.objects["brushParent"]))
                {
                    foreach (var item in ((Transform)MapEditor.objects["brushParent"]).GetComponentsInChildren<Transform>())
                    {
                        if ((item.position - targetPos).sqrMagnitude < 0.01f)
                        {
                            if ((bool)MapEditor.objects["spriteMode"])
                            {
                                SpriteRenderer sr = item.GetComponent<SpriteRenderer>();
                                sr.sprite = (Sprite)MapEditor.objects["sprite"];
                            }
                        }
                    }
                }
                if (table.ContainsKey(targetPos)) return;

                if (MapEditor.resources.Count > (int)MapEditor.objects["ResourceIndex"])
                {
                    (GameObject, Vector3) temp;
                    temp.Item1 = MapEditor.resources[(int)MapEditor.objects["ResourceIndex"]];
                    temp.Item2 = targetPos;
                    table.Add(targetPos, temp);
                }
            };

            Vector3 cameraPos = SceneView.GetAllSceneCameras()[0].transform.position;
            cameraPos.y = 0;
            Vector2 gridPivot = new Vector2((int)(cameraPos.x / ((Vector2)MapEditor.objects["gridInterval"]).x), (int)(cameraPos.z / ((Vector2)MapEditor.objects["gridInterval"]).y));
            gridPivot.x *= ((Vector2)MapEditor.objects["gridInterval"]).x;
            gridPivot.y *= ((Vector2)MapEditor.objects["gridInterval"]).y;
            for (float i = 0; i <= 100; i += gridInterval.x)
            {
                for (float j = 0; j <= 100; j += gridInterval.y)
                {
                    Vector2 pos;
                    if (new Rect((pos = gridPivot + new Vector2(i, j) + rectPosDuration), rectSize).Contains(brushPos))
                    {
                        setTile(pos + rectSize / 2f);
                    }
                    if (new Rect((pos = gridPivot + new Vector2(-i, j) + rectPosDuration), rectSize).Contains(brushPos))
                    {
                        setTile(pos + rectSize / 2f);
                    }
                    if (new Rect((pos = gridPivot + new Vector2(i, -j) + rectPosDuration), rectSize).Contains(brushPos))
                    {
                        setTile(pos + rectSize / 2f);
                    }
                    if (new Rect((pos = gridPivot + new Vector2(-i, -j) + rectPosDuration), rectSize).Contains(brushPos))
                    {
                        setTile(pos + rectSize / 2f);
                    }
                }
            }
        }
        void DrawBrush2()
        {
            if (brush2lock) return;
            float brushSize = (float)MapEditor.objects["brushSize"];
            float min = -brushSize / 2;
            float max = brushSize / 2;

            for (int i = (int)MapEditor.objects["brushDensity"]; i > 0; i--)
            {
                Vector2 randPos = new Vector2(Random.Range(min, max), Random.Range(min, max));
                if (max * max < randPos.sqrMagnitude)
                {
                    i++;
                }
                else
                {
                    Vector3 targetPos = new Vector3(randPos.x + hitPoint.x, hitPoint.y, randPos.y + hitPoint.z);
                    if (MapEditor.resources.Count > (int)MapEditor.objects["ResourceIndex"])
                    {
                        (GameObject, Vector3) temp;
                        temp.Item1 = MapEditor.resources[(int)MapEditor.objects["ResourceIndex"]];
                        temp.Item2 = targetPos;
                        table.Add(targetPos, temp);
                    }
                }
            }
            brush2lock = true;
        }
        void DrawBrush3()
        {
            Vector2 brushPos = new Vector2(hitPoint.x, hitPoint.z);
            Vector2 gridInterval = (Vector2)MapEditor.objects["gridInterval"];
            float brushSize = (float)MapEditor.objects["brushSize"];

            HashSet<Vector3> destroyList = new HashSet<Vector3>();

            if (((Transform)MapEditor.objects["brushParent"]))
            {

                Vector2 rectPosDuration =  - Vector2.one * brushSize / 2f;
                Vector2 rectSize = gridInterval + Vector2.one * brushSize;
                
                Vector3 cameraPos = SceneView.GetAllSceneCameras()[0].transform.position;
                cameraPos.y = 0;
                Vector2 gridPivot = new Vector2((int)(cameraPos.x / ((Vector2)MapEditor.objects["gridInterval"]).x), (int)(cameraPos.z / ((Vector2)MapEditor.objects["gridInterval"]).y));
                gridPivot.x *= ((Vector2)MapEditor.objects["gridInterval"]).x;
                gridPivot.y *= ((Vector2)MapEditor.objects["gridInterval"]).y;
                for (float i = 0; i <= 100; i += gridInterval.x)
                {
                    for (float j = 0; j <= 100; j += gridInterval.y)
                    {
                        Vector2 pos;
                        if (new Rect((pos = gridPivot + new Vector2(i, j) + rectPosDuration), rectSize).Contains(brushPos))
                        {
                            Vector2 p = pos + rectSize / 2f;
                            destroyList.Add(new Vector3(p.x, 0, p.y));
                        }
                        if (new Rect((pos = gridPivot + new Vector2(-i, j) + rectPosDuration), rectSize).Contains(brushPos))
                        {
                            Vector2 p = pos + rectSize / 2f;
                            destroyList.Add(new Vector3(p.x, 0, p.y));
                        }
                        if (new Rect((pos = gridPivot + new Vector2(i, -j) + rectPosDuration), rectSize).Contains(brushPos))
                        {
                            Vector2 p = pos + rectSize / 2f;
                            destroyList.Add(new Vector3(p.x, 0, p.y));
                        }
                        if (new Rect((pos = gridPivot + new Vector2(-i, -j) + rectPosDuration), rectSize).Contains(brushPos))
                        {
                            Vector2 p = pos + rectSize / 2f;
                            destroyList.Add(new Vector3(p.x, 0, p.y));
                        }
                    }
                }

                Transform[] childs = ((Transform)MapEditor.objects["brushParent"]).GetComponentsInChildren<Transform>();
                for (int i = 1; i < childs.Length; i++)
                {
                    if (childs[i] == null) continue;
                    
                    foreach (var item in destroyList)
                    {
                        if ((childs[i].transform.position - item).sqrMagnitude < 0.001f)
                        {
                            DestroyImmediate(childs[i].gameObject, false);
                            break;
                        }
                    }
                }

                destroyList.Clear();
            }
        }
        void DrawBrush4()
        {
            Vector2 max = (Vector2)MapEditor.objects["gridInterval"] / 4;

            if (((Transform)MapEditor.objects["brushParent"]))
            {
                Transform[] childs = ((Transform)MapEditor.objects["brushParent"]).GetComponentsInChildren<Transform>();
                for (int i = 1; i < childs.Length; i++)
                {
                    if (childs[i] == null) continue;
                    Vector3 objPos = childs[i].position - hitPoint;
                    if (max.sqrMagnitude > new Vector2(objPos.x, objPos.z).sqrMagnitude)
                    {
                        SpriteRenderer sr = childs[i].GetComponent<SpriteRenderer>();
                        if (sr != null)
                        {
                            MapEditor.objects["sprite"] = sr.sprite;
                        }
                    }
                }
            }
        }
    }
}
#endif
