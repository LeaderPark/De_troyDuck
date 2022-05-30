using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silhouette : MonoBehaviour
{
    private SpriteRenderer mySpriteRenderer;
    private SpriteRenderer srcSpriteRenderer;

    public Material silhouette;
    public Material playerSilhouette;


    void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        srcSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    void SetMaterial()
    {
        if (transform.parent.gameObject.layer == 6)
        {
            mySpriteRenderer.material = playerSilhouette;
        }
        else
        {
            mySpriteRenderer.material = silhouette;
        }
    }

    void Start()
    {
        StartCoroutine(SetSprite());
    }

    IEnumerator SetSprite()
    {
        while (true)
        {
            mySpriteRenderer.sprite = srcSpriteRenderer.sprite;
            mySpriteRenderer.flipX = srcSpriteRenderer.flipX;
            SetMaterial();
            yield return null;
        }
    }
}
