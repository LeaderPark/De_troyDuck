using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public string target;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null)
        {
            GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
            SceneManager sceneManager = ManagerObject.Instance.GetManager(ManagerType.SceneManager) as SceneManager;

            if (gameManager.Player.gameObject == other.gameObject)
            {
                sceneManager.LoadScene(target);
            }
        }
    }
}
