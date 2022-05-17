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
            DataManager dataManager = ManagerObject.Instance.GetManager(ManagerType.DataManager) as DataManager;
            dataManager.Load();
        }
    }

    // private IEnumerator GameStart()
    // {
    //     GetComponent<FadeInOut>().FadeInOut1(true);
    //     yield return new WaitForSeconds(2f);
    //     UnityEngine.SceneManagement.SceneManager.LoadScene("Maps");
    // }
}
