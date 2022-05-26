using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteAlways()]
#endif
public class DamageCount : MonoBehaviour
{
    bool isPlayer;
    bool isHeal;
    string text;
    int fontSize;
    Rect rect = new Rect();
    bool loading;


    public void Play(Vector3 position, int damage, bool isPlayer, bool isHeal)
    {
        if (loading) return;
        loading = true;
        this.isPlayer = isPlayer;
        this.isHeal = isHeal;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(position);
        text = damage.ToString();
        rect.width = text.Length * 25;
        rect.height = 1 * 25;
        rect.x = screenPoint.x - rect.width / 2f;
        rect.y = Camera.main.pixelHeight - screenPoint.y - rect.height / 2f;
        
        StartCoroutine(Process(new Vector2(screenPoint.x, Camera.main.pixelHeight - screenPoint.y)));
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
            GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;

            if (gameManager.Player == null) return;
            if (gameManager.Player.GetComponent<Entity>().isDead) return;

            GUIStyle style = GUIStyle.none;
            style.fontSize = fontSize;
            style.normal.textColor = Color.black;
            rect.position -= Vector2.one * 2;
            GUI.Label(rect, text, style);
            rect.position += Vector2.one * 4;
            GUI.Label(rect, text, style);
            if (isPlayer)
            {
                style.normal.textColor = Color.red;
            }
            else
            {
                style.normal.textColor = Color.white;
            }
            if (isHeal)
            {
                style.normal.textColor = Color.green;
            }
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
            damageCount.Play(damageCount.transform.position, 90, false, false);
        }
    }
}
#endif
