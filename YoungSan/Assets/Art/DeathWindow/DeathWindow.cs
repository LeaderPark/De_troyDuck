using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWindow : MonoBehaviour
{
    void Update()
    {
        transform.position = Camera.main.ViewportToWorldPoint(Vector2.one * 0.5f) + transform.forward * 5f;
    }
}
