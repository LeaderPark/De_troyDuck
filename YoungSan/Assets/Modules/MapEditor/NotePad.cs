using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class NotePad : MonoBehaviour
{
    public NotePadData[] notePadDatas;

    const float defaultDistance = 50;
    
    GUIStyle guiStyle = new GUIStyle();

    int useId;
    private Vector2 mousePos;
    bool mouseDown;

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
        switch (Event.current.type)
        {
            case EventType.MouseDown:
            if (Event.current.isMouse && Event.current.button == 0)
            {
                int id = HandleUtility.nearestControl;
                mousePos = Event.current.mousePosition;
                mouseDown = true;

                useId = id;
            }
            break;
            case EventType.MouseUp:
            if (Event.current.isMouse && Event.current.button == 0)
            {
                mouseDown = false;
            }
            break;
        }
        if (notePadDatas == null) return;
        if (notePadDatas.Length <= 0) return;
        foreach (NotePadData data in notePadDatas)
        {
            if (data.active)
            {
                Vector3 notepadPos = transform.position + data.pos;
                var guiLoc = HandleUtility.WorldToGUIPoint(notepadPos);
                float cameraDistance = Vector3.Distance(sceneView.camera.transform.position, notepadPos);

                Vector2 textLength = Vector2.zero;
                string[] ss = data.text.Split('\n');
                textLength.y = ss.Length;
                foreach (string s in ss)
                {
                    if (textLength.x < s.Length) textLength.x = s.Length;
                }

                Vector2 size = new Vector2(data.fontSize * 1.5f * textLength.x * defaultDistance / cameraDistance, data.fontSize * 1.5f * textLength.y * defaultDistance / cameraDistance);

                
                var oldcolor = Handles.color;

                Handles.color = data.outlineColor;
                Handles.DrawLine(notepadPos, new Vector3(notepadPos.x, 0, notepadPos.z));
                Handles.DrawWireCube(new Vector3(notepadPos.x, 0.25f, notepadPos.z), new Vector3(1f, 0.5f, 1f));

                if (Selection.activeGameObject == gameObject)
                {
                    switch (Event.current.type)
                    {
                        case EventType.Repaint:

                        if (mouseDown)
                        {
                            if (useId == data.id.x)
                            {
                                Handles.color = Handles.selectedColor;
                                Handles.ArrowHandleCap(data.id.x, new Vector3(notepadPos.x, 0, notepadPos.z), transform.rotation * Quaternion.LookRotation(Vector3.right), 10 * cameraDistance / defaultDistance, EventType.Repaint);
                        
                                Handles.color = Handles.yAxisColor;
                                Handles.ArrowHandleCap(data.id.y, new Vector3(notepadPos.x, 0, notepadPos.z), transform.rotation * Quaternion.LookRotation(Vector3.up), data.pos.y, EventType.Repaint);
                                
                                Handles.color = Handles.zAxisColor;
                                Handles.ArrowHandleCap(data.id.z, new Vector3(notepadPos.x, 0, notepadPos.z), transform.rotation * Quaternion.LookRotation(Vector3.forward), 10 * cameraDistance / defaultDistance, EventType.Repaint);
                            }
                            else if (useId == data.id.y)
                            {
                                Handles.color = Handles.xAxisColor;
                                Handles.ArrowHandleCap(data.id.x, new Vector3(notepadPos.x, 0, notepadPos.z), transform.rotation * Quaternion.LookRotation(Vector3.right), 10 * cameraDistance / defaultDistance, EventType.Repaint);
                                
                                Handles.color = Handles.selectedColor;
                                Handles.ArrowHandleCap(data.id.y, new Vector3(notepadPos.x, 0, notepadPos.z), transform.rotation * Quaternion.LookRotation(Vector3.up), data.pos.y, EventType.Repaint);

                                Handles.color = Handles.zAxisColor;
                                Handles.ArrowHandleCap(data.id.z, new Vector3(notepadPos.x, 0, notepadPos.z), transform.rotation * Quaternion.LookRotation(Vector3.forward), 10 * cameraDistance / defaultDistance, EventType.Repaint);
                            }
                            else if (useId == data.id.z)
                            {
                                Handles.color = Handles.xAxisColor;
                                Handles.ArrowHandleCap(data.id.x, new Vector3(notepadPos.x, 0, notepadPos.z), transform.rotation * Quaternion.LookRotation(Vector3.right), 10 * cameraDistance / defaultDistance, EventType.Repaint);
                                
                                Handles.color = Handles.yAxisColor;
                                Handles.ArrowHandleCap(data.id.y, new Vector3(notepadPos.x, 0, notepadPos.z), transform.rotation * Quaternion.LookRotation(Vector3.up), data.pos.y, EventType.Repaint);

                                Handles.color = Handles.selectedColor;
                                Handles.ArrowHandleCap(data.id.z, new Vector3(notepadPos.x, 0, notepadPos.z), transform.rotation * Quaternion.LookRotation(Vector3.forward), 10 * cameraDistance / defaultDistance, EventType.Repaint);
                            }
                            else
                            {
                                Handles.color = Handles.xAxisColor;
                                Handles.ArrowHandleCap(data.id.x, new Vector3(notepadPos.x, 0, notepadPos.z), transform.rotation * Quaternion.LookRotation(Vector3.right), 10 * cameraDistance / defaultDistance, EventType.Repaint);

                                Handles.color = Handles.yAxisColor;
                                Handles.ArrowHandleCap(data.id.y, new Vector3(notepadPos.x, 0, notepadPos.z), transform.rotation * Quaternion.LookRotation(Vector3.up), data.pos.y, EventType.Repaint);

                                Handles.color = Handles.zAxisColor;
                                Handles.ArrowHandleCap(data.id.z, new Vector3(notepadPos.x, 0, notepadPos.z), transform.rotation * Quaternion.LookRotation(Vector3.forward), 10 * cameraDistance / defaultDistance, EventType.Repaint);
                            }
                        }
                        else
                        {
                            Handles.color = Handles.xAxisColor;
                            Handles.ArrowHandleCap(data.id.x, new Vector3(notepadPos.x, 0, notepadPos.z), transform.rotation * Quaternion.LookRotation(Vector3.right), 10 * cameraDistance / defaultDistance, EventType.Repaint);

                            Handles.color = Handles.yAxisColor;
                            Handles.ArrowHandleCap(data.id.y, new Vector3(notepadPos.x, 0, notepadPos.z), transform.rotation * Quaternion.LookRotation(Vector3.up), data.pos.y, EventType.Repaint);

                            Handles.color = Handles.zAxisColor;
                            Handles.ArrowHandleCap(data.id.z, new Vector3(notepadPos.x, 0, notepadPos.z), transform.rotation * Quaternion.LookRotation(Vector3.forward), 10 * cameraDistance / defaultDistance, EventType.Repaint);
                        }
                        break;
                        case EventType.Layout:
                        Handles.color = Handles.xAxisColor;
                        Handles.ArrowHandleCap(data.id.x, new Vector3(notepadPos.x, 0, notepadPos.z), transform.rotation * Quaternion.LookRotation(Vector3.right), 10 * cameraDistance / defaultDistance, EventType.Layout);

                        Handles.color = Handles.yAxisColor;
                        Handles.ArrowHandleCap(data.id.y, new Vector3(notepadPos.x, 0, notepadPos.z), transform.rotation * Quaternion.LookRotation(Vector3.up), data.pos.y, EventType.Layout);

                        Handles.color = Handles.zAxisColor;
                        Handles.ArrowHandleCap(data.id.z, new Vector3(notepadPos.x, 0, notepadPos.z), transform.rotation * Quaternion.LookRotation(Vector3.forward), 10 * cameraDistance / defaultDistance, EventType.Layout);

                        break;
                        case EventType.MouseDrag:
                        if (mouseDown)
                        {
                            Vector3 mouseGap = (HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).direction - HandleUtility.GUIPointToWorldRay(mousePos).direction);
                            Vector3 move = mouseGap * Vector3.Distance(SceneView.GetAllSceneCameras()[0].transform.position, notepadPos);
                            
                            if (useId == data.id.x)
                            {
                                mousePos = Event.current.mousePosition;
                                data.pos += new Vector3(move.x, 0, 0);
                            }
                            else if (useId == data.id.y)
                            {
                                mousePos = Event.current.mousePosition;
                                data.pos += new Vector3(0, move.y, 0);
                            }
                            else if (useId == data.id.z)
                            {
                                mousePos = Event.current.mousePosition;
                                data.pos += new Vector3(0, 0, move.z);
                            }

                        }
                        break;
                    }
                }

                Handles.color = oldcolor;


                guiStyle = GUI.skin.box;
                int defaultFontSize = guiStyle.fontSize;
                Color normalTextColor = guiStyle.normal.textColor;
                Color hoverTextColor = guiStyle.hover.textColor;

                guiStyle.fontSize = (int)(data.fontSize * defaultDistance / cameraDistance);
                guiStyle.normal.textColor = Color.white;
                guiStyle.hover.textColor = Color.white;

                var rect = new Rect(guiLoc.x - size.x / 2f, guiLoc.y - size.y, size.x, size.y);

                var ray = HandleUtility.GUIPointToWorldRay(rect.position + new Vector2(rect.width / 2, rect.height / 2));
                var lbWPos = ray.origin;

                Handles.BeginGUI();
                Handles.DrawSolidRectangleWithOutline(rect, data.backgroundColor, data.outlineColor);
                GUI.Label(rect, data.text, guiStyle);
                Handles.EndGUI();
                
                guiStyle.fontSize = defaultFontSize;
                guiStyle.normal.textColor = normalTextColor;
                guiStyle.hover.textColor = hoverTextColor;
            }
        }
    }
}

[CustomEditor(typeof(NotePad))]
public class NotePadEditor : Editor
{
    SerializedProperty notePadDatas;

    void OnEnable()
    {
        notePadDatas = serializedObject.FindProperty("notePadDatas");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        int idx = 30000;
        for (int i = 0; i < notePadDatas.arraySize; i++)
        {
            ((NotePad)serializedObject.targetObject).notePadDatas[i].id = new Vector3Int(idx++, idx++, idx++);
        }
    }
}

[System.Serializable]
public class NotePadData
{
    public bool active;
    [TextArea()]
    public string text;
    public Vector3 pos;
    public int fontSize;
    [ColorUsage(true)]
    public Color backgroundColor;
    [ColorUsage(true)]
    public Color outlineColor;

    public Vector3Int id {get;set;}
}