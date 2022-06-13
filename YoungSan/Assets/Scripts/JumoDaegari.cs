using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumoDaegari : MonoBehaviour
{
    void OnGUI()
    {
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        if (Time.timeScale != 0 && gameManager.Player.enabled)
        {
            Vector3 worldPosition = transform.position + Vector3.up * 4f;
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

            string text = "Space";

            Rect rect = new Rect();
            rect.width = 50 * text.Length;
            rect.height = 70;
            rect.x = screenPosition.x - rect.width / 2;
            rect.y = Camera.main.pixelHeight - screenPosition.y - rect.height / 2;

            GUI.Box(rect, "");

            rect.width = 25 * text.Length;
            rect.height = 50;
            rect.x = screenPosition.x - rect.width / 2;
            rect.y = Camera.main.pixelHeight - screenPosition.y - rect.height / 2;

            GUIStyle style = GUIStyle.none;
            style.fontSize = 50;
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
