using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[ExecuteAlways]
public class Break : MonoBehaviour
{
    public SpriteRenderer sr;

    void Update()
    {
        GetComponent<VisualEffect>().SetTexture("BreakObject", sr.sprite.texture);
    }
}
