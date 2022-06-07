using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharImageUI : MonoBehaviour
{
    private Image charImage;

    // Start is called before the first frame update
    void Start()
    {
        charImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 0)
        {
            GameManager gameManager = ManagerObject.Instance.GetManager((ManagerType.GameManager)) as GameManager;
            charImage.sprite = gameManager.Player.GetComponent<SpriteRenderer>().sprite;
        }
    }
}
