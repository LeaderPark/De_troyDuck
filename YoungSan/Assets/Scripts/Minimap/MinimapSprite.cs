using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MinimapSprite : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public MinimapCamera minimapCamera;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(90, transform.parent.eulerAngles.y, 0);
        transform.position = new Vector3(transform.parent.position.x, 1, transform.parent.position.z);
    
        if(spriteRenderer.isVisible == false)
        {
            minimapCamera.ShowBorderIndicator(transform.position);
            //Debug.Log("false");
        }
        else
        {
            minimapCamera.HideBorderIncitator();
            //Debug.Log("true");
        }  
    }
}
