using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToPlay : MonoBehaviour
{
    bool isRunning = false;

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
}
