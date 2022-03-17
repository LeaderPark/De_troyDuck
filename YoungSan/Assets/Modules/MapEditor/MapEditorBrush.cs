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
        }

        void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        
        void OnSceneGUI(SceneView sceneView)
        {
            Vector3 mousepos = Event.current.mousePosition;
            ray = HandleUtility.GUIPointToWorldRay(mousepos);
            
            RaycastHit hit;
            Vector3 pos100 = ray.origin + ray.direction * 100;
            float distance = 100 * (Mathf.Abs(ray.origin.y) - (int)MapEditor.objects["gridHeight"]) / Mathf.Abs(ray.origin.y - pos100.y);
            if (Physics.SphereCast(ray, (float)MapEditor.objects["brushSize"] / 2f, out hit, 100))
            {
                float hitDistance = hit.distance + (float)MapEditor.objects["brushSize"] / 4f;
                if (hitDistance < distance)
                {
                    hitPoint = ray.origin + ray.direction * hitDistance;
                }
                else
                {
                    hitPoint = ray.origin + ray.direction * distance;
                }
            }
            else
            {
                hitPoint = ray.origin + ray.direction * distance;
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(hitPoint, (float)MapEditor.objects["brushSize"] / 2f);

            Gizmos.color = Color.grey;
            int height = (int)MapEditor.objects["gridHeight"];
            
            Vector3 cameraPos = SceneView.GetAllSceneCameras()[0].transform.position;
            cameraPos.y = 0;
            Vector3 gridPivot = new Vector3((int)(cameraPos.x / ((Vector2)MapEditor.objects["gridInterver"]).x), 0, (int)(cameraPos.z / ((Vector2)MapEditor.objects["gridInterver"]).y)) * ((Vector2)MapEditor.objects["gridInterver"]);
            for (float i = 0; i <= 100; i += ((Vector2)MapEditor.objects["gridInterver"]).x)
            {
                Gizmos.DrawLine(gridPivot + new Vector3(i, height, -100), gridPivot + new Vector3(i, height, 100));
                Gizmos.DrawLine(gridPivot + new Vector3(-i, height, -100), gridPivot + new Vector3(-i, height, 100));
            }
            for (float i = 0; i <= 100; i += ((Vector2)MapEditor.objects["gridInterver"]).y)
            {
                Gizmos.DrawLine(gridPivot + new Vector3(-100, height, i), gridPivot + new Vector3(100, height, i));
                Gizmos.DrawLine(gridPivot + new Vector3(-100, height, -i), gridPivot + new Vector3(100, height, -i));
            }
        }
    }


    [CustomEditor(typeof(MapEditorBrush))]
    public class MapEditorBrushEditor : Editor
    {
    }
}
