using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToPlay : MonoBehaviour
{
    bool isRunning = false;
    void Awake()
    {
		Debug.Log(ManagerObject.Instance);
		//if (ManagerObject.Instance == null)
		//	Application.Quit();
	}
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isRunning)
            {
                isRunning = true;

                SoundManager soundManager = ManagerObject.Instance.GetManager(ManagerType.SoundManager) as SoundManager;
                soundManager.SetBgm(string.Empty);
                DataManager dataManager = ManagerObject.Instance.GetManager(ManagerType.DataManager) as DataManager;
                dataManager.Load();
            }

        }
    }

    // private IEnumerator GameStart()
    // {
    //     GetComponent<FadeInOut>().FadeInOut1(true);
    //     yield return new WaitForSeconds(2f);
    //     UnityEngine.SceneManagement.SceneManager.LoadScene("Maps");
    // }
}
