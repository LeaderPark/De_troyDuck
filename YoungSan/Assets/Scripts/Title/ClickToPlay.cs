using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToPlay : MonoBehaviour
{
    void Awake()
    {
        Debug.Log(ManagerObject.Instance);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
            SaveData sv = gameManager.gameObject.GetComponent<SaveData>();
            StartCoroutine(sv.Load());
        }
    }

    // private IEnumerator GameStart()
    // {
    //     GetComponent<FadeInOut>().FadeInOut1(true);
    //     yield return new WaitForSeconds(2f);
    //     UnityEngine.SceneManagement.SceneManager.LoadScene("Maps");
    // }
}
