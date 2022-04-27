using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways()]
public class DamageCount : MonoBehaviour
{
    string text;
    Rect rect = new Rect();
    bool loading;

    GUIStyle style;

    void Awake()
    {
        
    }

    public void Play(Vector3 position, int damage)
    {
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(position);
        text = damage.ToString();
        rect.width = text.Length * 25;
        rect.height = 1 * 25;
        rect.x = screenPoint.x - rect.width / 2f;
        rect.y = Camera.main.pixelHeight - screenPoint.y - rect.height / 2f;

        loading = true;
    }
    
    void OnGUI()
    {
        if (loading)
        {
            GUI.Label(rect, text);
        }
    }
}

#if UNITY_EDITOR

[UnityEditor.CustomEditor(typeof(DamageCount))]
public class DamageCountEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Play"))
        {
            DamageCount damageCount = (DamageCount)target;
            damageCount.Play(damageCount.transform.position, 90);
        }
    }
}
#endif
