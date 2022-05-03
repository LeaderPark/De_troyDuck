using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways()]
public class DamageCount : MonoBehaviour
{
    string text;
    int fontSize;
    Rect rect = new Rect();
    bool loading;


    public void Play(Vector3 position, int damage)
    {
        if (loading) return;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(position);
        text = damage.ToString();
        rect.width = text.Length * 25;
        rect.height = 1 * 25;
        rect.x = screenPoint.x - rect.width / 2f;
        rect.y = Camera.main.pixelHeight - screenPoint.y - rect.height / 2f;
        
        StartCoroutine(Process(new Vector2(screenPoint.x, Camera.main.pixelHeight - screenPoint.y)));
        loading = true;
    }

    IEnumerator Process(Vector2 baseVec)
    {
        float timeStack = 0;
        int dir = Random.Range(0, 2);
        while (timeStack < 1f)
        {
            timeStack += Time.deltaTime;
            timeStack = Mathf.Clamp(timeStack, 0f, 1f);

            float x = timeStack * 5.1f;

            Vector2 moveVec;

            moveVec.y = 4 * (Mathf.Pow(x - 2f, 2) - 2);
            if (dir == 0)
            {
                moveVec.x = x;
            }
            else
            {
                moveVec.x = -x;
            }

            fontSize = (int)Mathf.Lerp(50, 1, timeStack / 1f);
            rect.position = baseVec + moveVec * 10 - Vector2.one * fontSize / 2f;

            yield return null;
        }
        loading = false;
        gameObject.SetActive(false);
        yield return null;
    }
    
    void OnGUI()
    {
        if (loading)
        {
            GUIStyle style = GUIStyle.none;
            style.fontSize = fontSize;
            style.normal.textColor = Color.black;
            rect.position -= Vector2.one * 2;
            GUI.Label(rect, text, style);
            rect.position += Vector2.one * 4;
            GUI.Label(rect, text, style);
            style.normal.textColor = Color.white;
            rect.position -= Vector2.one * 2;
            GUI.Label(rect, text, style);
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
