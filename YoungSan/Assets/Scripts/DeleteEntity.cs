using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteEntity : MonoBehaviour
{
    public void Delete()
    {
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        var datas = FindObjectsOfType<Entity>();

        for (int i = 0; i < datas.Length; i++)
        {
            if (datas[i].isDead && gameManager.Player.gameObject != datas[i].gameObject)
            {
                datas[i].gameObject.SetActive(false);
            }
        }
    }
}
