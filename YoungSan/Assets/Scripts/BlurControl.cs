using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class BlurControl : MonoBehaviour
{
    public Volume volume;
    DepthOfField depth;
    // Start is called before the first frame update
    void Awake()
    {
        volume.profile.TryGet<DepthOfField>(out depth);
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log(depth);
            if (depth != null)
            {
                depth.farMaxBlur -= 1f; 
            }
            
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Debug.Log(depth);
            if (depth != null)
            {
                depth.farMaxBlur += 1f; 
            }
            
        }
#endif
    }
    public void Blur(bool active,float fadeTime)
    {
        StopAllCoroutines();
        StartCoroutine(BlurFade(active, fadeTime));
    }
    IEnumerator BlurFade(bool active, float fadeTime)
    {
        float time = depth.farMaxBlur;
        if (!active)
        {
            time = 7 - depth.farMaxBlur;
        }
        while (true)
        {
            time += Time.deltaTime;
            if (active)
            {
                depth.farMaxBlur = Mathf.Lerp(0, 7, time / fadeTime);
                if (time >= fadeTime)
                {
                    depth.farMaxBlur = Mathf.Lerp(0, 7, 1);
                    yield break;
                }
            }
            else
            {
                depth.farMaxBlur = Mathf.Lerp(7, 0, time / fadeTime);
                if (time >= fadeTime)
                {
                    depth.farMaxBlur = Mathf.Lerp(7, 0, 1);
                    yield break;
                }
            }
            yield return null;
        }
    }
}
