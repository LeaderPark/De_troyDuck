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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetPivot();
        }
    }

    void SetPivot()
    {
        Vector2 imageSprite = gameObject.GetComponent<Image>().sprite.pivot; //256, 320
        Vector2 imageRect = gameObject.GetComponent<Image>().sprite.rect.size; //4 , 4
        Vector2 imagePivot = imageSprite / imageRect; //0.4 , 0.5

        Vector2 rectSize = gameObject.GetComponent<RectTransform>().sizeDelta; //300 , 300
        Vector2 rectPivot = gameObject.GetComponent<RectTransform>().pivot; //0.5 , 0.5

        Vector2 pivotValue = rectPivot - imagePivot; //0.1, 0.0
        Vector2 valueSize = pivotValue * rectSize;
        valueSize.y -= 150 / (imageRect.y / 160);

        gameObject.GetComponent<RectTransform>().anchoredPosition = valueSize;
    }

    void SetPivot1()
    {
        // Vector2 pixelPivot = gameObject.GetComponent<Image>().sprite.pivot;
        // gameObject.GetComponent<Image>().sprite.
        // 
        // Vector2 size = gameObject.GetComponent<RectTransform>().sizeDelta;
        // size /= 10;
        // 
        // Vector2 size = gameObject.GetComponent<RectTransform>().sizeDelta;
        // Debug.Log(size);
        // size *= gameObject.GetComponent<Image>().pixelsPerUnit;
        // Debug.Log(size);
        // Vector2 pixelPivot = gameObject.GetComponent<Image>().sprite.pivot;
        // Debug.Log(pixelPivot);
        // Vector2 percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
        // Debug.Log(percentPivot);
        // gameObject.GetComponent<RectTransform>().pivot = percentPivot;

    }
}
