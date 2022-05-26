using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public float boundDistance;
    public bool isPlayerInBound;

    void Awake()
    {
        StartCoroutine(CheckInBound());
        StartCoroutine(Interaction());
    }

    void Start()
    {
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        if (gameManager.Player == null)
        {
            DataManager dataManager = ManagerObject.Instance.GetManager(ManagerType.DataManager) as DataManager;
            dataManager.Load();
        }
    }

    IEnumerator CheckInBound()
    {
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        while (true)
        {
            Vector2 current = new Vector2(transform.position.x, transform.position.z);
            Vector2 player = new Vector2(gameManager.Player.transform.position.x, gameManager.Player.transform.position.z);
            if (Vector2.Distance(current, player) <= boundDistance)
            {
                isPlayerInBound = true;
            }
            else
            {
                isPlayerInBound = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Interaction()
    {
        DataManager dataManager = ManagerObject.Instance.GetManager(ManagerType.DataManager) as DataManager;
        InputManager inputManager = ManagerObject.Instance.GetManager(ManagerType.InputManager) as InputManager;
        while (true)
        {
            if (isPlayerInBound && inputManager.CheckKeyState(KeyCode.Space, ButtonState.Down))
            {
                dataManager.Save();
            }
            yield return null;
        }

    }

    void OnGUI()
    {
        if (isPlayerInBound)
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
