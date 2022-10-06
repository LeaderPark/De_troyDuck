using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugRoom : MonoBehaviour
{
    GameManager gameManager;
    public GameObject selectChar;
    public GameObject playerChar;
    public Image charImage;

    private int idx = 0;
    public GameObject[] selectCharArray;
    public GameObject[] enemyArray;
    public Sprite[] sprite;

    private Vector3 originePos;

    private void Awake()
    {
        gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        UIManager uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        uIManager.cursor = true;
        playerChar = gameManager.Player.gameObject;
        originePos = selectChar.transform.position;
    }

    public void IdxUp()
    {
        idx = (idx + 1) % selectCharArray.Length;
        charImage.sprite = sprite[idx];
    }
    public void IdxDown()
    {
        if (idx > 0)
        {
            idx--;
        }
        else
        {
            idx = selectCharArray.Length-1;
        }
        charImage.sprite = sprite[idx];
    }
    public void SpawnChar()
    {
        if (playerChar != gameManager.Player.gameObject)
        {
            selectChar = playerChar;
            playerChar = gameManager.Player.gameObject;
        }
        if (playerChar == selectCharArray[idx]) return;

        selectChar.SetActive(false);
        selectCharArray[idx].SetActive(true);
        selectCharArray[idx].GetComponent<Entity>().Die();
        selectCharArray[idx].transform.position = originePos;
        selectChar = selectCharArray[idx];
    }
    public void EnemySpawn()
    {
        for (int i = 0; i < enemyArray.Length; i++)
        {
            enemyArray[i].SetActive(true);
            Entity entity = enemyArray[i].GetComponent<Entity>();
            entity.SetHp(1);
        }
    }
}
