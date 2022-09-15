using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EndScene : MonoBehaviour
{
	private void Awake()
	{
        Time.timeScale = 1.001f;
	}
	private void Start()
	{
        UIManager manager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        manager.cursor = true;
	}
	public void Retry()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        System.IO.File.Delete(Application.persistentDataPath + "/SaveData.json");
        Application.Quit();
    }

    public void FeedBack()
    {
        Application.OpenURL("https://forms.gle/uC6UuoRxtXqRWGvz9");
    }
}
