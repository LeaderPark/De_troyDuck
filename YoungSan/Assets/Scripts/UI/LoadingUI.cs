using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.UIElements;

public class LoadingUI : MonoBehaviour
{
    [SerializeField]
    Slider progressBar;
    public static string nextScene;

    public GameObject loadingObj;
    public Image loadingImage;

    private AsyncOperation op;

    void Update()
    { 
        loadingImage.sprite = loadingObj.GetComponent<SpriteRenderer>().sprite;
    }

    public void LoadScene(string sceneName)
    {
        progressBar.value = 0;
        loadingObj.SetActive(true);
        nextScene = sceneName;
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;

        op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime * 3f;
            if (op.progress < 0.9f)
            {
                progressBar.value = Mathf.Lerp(progressBar.value, op.progress, timer);
                if (progressBar.value >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.value = Mathf.Lerp(progressBar.value, 1f, timer);
                if (progressBar.value == 1.0f)
                {
                    yield return new WaitForSeconds(2f);
                    op.allowSceneActivation = true;
                    progressBar.value = 0;
                    yield break;
                }
            }
        }

    }
}
