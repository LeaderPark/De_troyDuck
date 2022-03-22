using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
            queue = new Queue();
        }

        void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        Queue queue;

        void Update()
        {
            for (;queue.Count > 0;)
            {
                (GameObject, Vector3) temp = ((GameObject, Vector3))queue.Dequeue();
                GameObject obj = GameObject.Instantiate(temp.Item1);
                obj.transform.position = temp.Item2;
                obj.transform.parent = (Transform)MapEditor.objects["brushParent"];
            }
        }

        private bool mouseDown = false;
        private bool removeTile = false;

        void OnSceneGUI(SceneView sceneView)
        {
            DrawDefaultPreview();

            switch (Event.current.type)
            {
                case EventType.MouseDown:
                if (Event.current.isMouse && Event.current.button == 0)
                {
                    mouseDown = true;
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
                    DrawTile();
                }
                break;
                case EventType.KeyDown:
                if (Event.current.keyCode == KeyCode.R)
                {
                    removeTile = true;
                }
                break;
                case EventType.KeyUp:
                if (Event.current.keyCode == KeyCode.R)
                {
                    removeTile = false;
                }
                break;
            }
        }

        void DrawDefaultPreview()
        {
            Vector3 mousepos = Event.current.mousePosition;
            ray = HandleUtility.GUIPointToWorldRay(mousepos);
            
            Vector3 pos100 = ray.origin + ray.direction * 100;
            float distance = 100 * (Mathf.Abs(ray.origin.y) - (int)MapEditor.objects["gridHeight"]) / Mathf.Abs(ray.origin.y - pos100.y);
            
            hitPoint = ray.origin + ray.direction * distance;
            hitPoint.y = (int)MapEditor.objects["gridHeight"];
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(hitPoint, (float)MapEditor.objects["brushSize"] / 2f);

            Gizmos.color = Color.grey;
            int height = (int)MapEditor.objects["gridHeight"];
            
            Vector3 cameraPos = SceneView.GetAllSceneCameras()[0].transform.position;
            cameraPos.y = 0;
            Vector3 gridPivot = new Vector3((int)(cameraPos.x / ((Vector2)MapEditor.objects["gridInterval"]).x), 0, (int)(cameraPos.z / ((Vector2)MapEditor.objects["gridInterval"]).y));
            gridPivot.x *= ((Vector2)MapEditor.objects["gridInterval"]).x;
            gridPivot.z *= ((Vector2)MapEditor.objects["gridInterval"]).y;
            for (float i = 0; i <= 100; i += ((Vector2)MapEditor.objects["gridInterval"]).x)
            {
                Gizmos.DrawLine(gridPivot + new Vector3(i, height, -100), gridPivot + new Vector3(i, height, 100));
                Gizmos.DrawLine(gridPivot + new Vector3(-i, height, -100), gridPivot + new Vector3(-i, height, 100));
            }
            for (float i = 0; i <= 100; i += ((Vector2)MapEditor.objects["gridInterval"]).y)
            {
                Gizmos.DrawLine(gridPivot + new Vector3(-100, height, i), gridPivot + new Vector3(100, height, i));
                Gizmos.DrawLine(gridPivot + new Vector3(-100, height, -i), gridPivot + new Vector3(100, height, -i));
            }
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
            }
        }

        void DrawBrush1()
        {
            Vector2 brushPos = new Vector2(hitPoint.x, hitPoint.z);
            Vector2 gridInterval = (Vector2)MapEditor.objects["gridInterval"];
            
            int height = (int)MapEditor.objects["gridHeight"];
            float brushSize = (float)MapEditor.objects["brushSize"];

            Vector2 rectPosDuration =  - Vector2.one * brushSize / 2f;
            Vector2 rectSize = gridInterval + Vector2.one * brushSize;

            System.Action<Vector2> setTile = (center) =>
            {
                Vector3 targetPos = new Vector3(center.x, hitPoint.y, center.y);
                if (removeTile)
                {
                    if (((Transform)MapEditor.objects["brushParent"]))
                    {
                        foreach (var item in ((Transform)MapEditor.objects["brushParent"]).GetComponentsInChildren<Transform>())
                        {
                            if (item.position == targetPos) DestroyImmediate(item.gameObject);
                        }
                    }
                    else
                    {
                        foreach (var item in FindObjectsOfType<Transform>())
                        {
                            if (item.position == targetPos) DestroyImmediate(item.gameObject);
                        }
                    }
                }
                else
                {
                    if (((Transform)MapEditor.objects["brushParent"]))
                    {
                        foreach (var item in ((Transform)MapEditor.objects["brushParent"]).GetComponentsInChildren<Transform>())
                        {
                            if (item.position == targetPos) return;
                        }
                    }
                    else
                    {
                        foreach (var item in FindObjectsOfType<Transform>())
                        {
                            if (item.position == targetPos) return;
                        }
                    }

                    if (MapEditor.resources.Count > (int)MapEditor.objects["ResourceIndex"])
                    {
                        (GameObject, Vector3) temp;
                        temp.Item1 = MapEditor.resources[(int)MapEditor.objects["ResourceIndex"]];
                        temp.Item2 = targetPos;
                        queue.Enqueue(temp);
                    }
                }
            };

            for (float i = 0; i <= 100; i += gridInterval.x)
            {
                for (float j = 0; j <= 100; j += gridInterval.y)
                {
                    Vector2 pos;
                    if (new Rect((pos = new Vector2(i, j) + rectPosDuration), rectSize).Contains(brushPos))
                    {
                        setTile(pos + rectSize / 2f);
                    }
                    if (new Rect((pos = new Vector2(-i, j) + rectPosDuration), rectSize).Contains(brushPos))
                    {
                        setTile(pos + rectSize / 2f);
                    }
                    if (new Rect((pos = new Vector2(i, -j) + rectPosDuration), rectSize).Contains(brushPos))
                    {
                        setTile(pos + rectSize / 2f);
                    }
                    if (new Rect((pos = new Vector2(-i, -j) + rectPosDuration), rectSize).Contains(brushPos))
                    {
                        setTile(pos + rectSize / 2f);
                    }
                }
            }
        }
        void DrawBrush2()
        {
            Debug.Log("hmm2");
        }
        void DrawBrush3()
        {
            Debug.Log("hmm3");
        }
    }
}
