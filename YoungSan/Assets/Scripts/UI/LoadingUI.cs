using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    public GameObject loadingObj;
    private Image loadingImage;


    // Start is called before the first frame update
    void Start()
    {
        loadingImage = GetComponent<Image>();
    }

    void Update()
    { 
            //SetPivot();
            loadingImage.sprite = loadingObj.GetComponent<SpriteRenderer>().sprite;
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
}
