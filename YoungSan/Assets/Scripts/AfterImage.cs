using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer targetRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetTarget(SpriteRenderer sr)
    {
        targetRenderer = sr;
    }

    public void Play()
    {
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        spriteRenderer.sprite = targetRenderer.sprite;
        spriteRenderer.flipX = targetRenderer.flipX;
        transform.position = targetRenderer.transform.position;
    }

}
