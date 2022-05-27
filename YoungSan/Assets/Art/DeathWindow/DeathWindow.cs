using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class DeathWindow : MonoBehaviour
{
    Coroutine play;
    System.Action onEndWindow;
    public float playSpeed;

    IEnumerator SetPosition()
    {
        while (play != null)
        {
            transform.position = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f)) + transform.forward * 5f;
            yield return null;
        }
    }

    IEnumerator Play()
    {
        yield return PlayVideo();
        yield return PlayEnd();

        TurnOffWindow();
        yield return null;
    }

    IEnumerator PlayVideo()
    {
        const float time = 0.5f;
        const float target = 0.6f;
        float tempTime = time;
        while (tempTime > 0f)
        {
            tempTime -= Time.deltaTime;
            float alpha = (1f - tempTime / time) * target;
            GetComponent<SpriteRenderer>().material.SetFloat("_BackgroundAlpha", Mathf.Clamp(alpha, 0, target));
            yield return null;
        }
        GetComponent<VideoPlayer>().time = 0f;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<VideoPlayer>().SetDirectAudioMute(0, false);
        GetComponent<VideoPlayer>().Play();
        GetComponent<VideoPlayer>().playbackSpeed = playSpeed;
        while (!GetComponent<VideoPlayer>().isPlaying)
        {
            yield return null;
        }
        while (GetComponent<VideoPlayer>().isPlaying)
        {
            yield return null;
        }
    }

    IEnumerator PlayEnd()
    {
        float time = 0.5f;
        const float alphaTarget = 0.4f;
        const float outlineTarget = 1f;
        const float overlayTarget = 1f;
        float tempTime = time;
        while (tempTime > 0f)
        {
            tempTime -= Time.deltaTime;
            float alpha = (1f - tempTime / time) * alphaTarget;
            GetComponent<SpriteRenderer>().material.SetFloat("_BackgroundAlpha", Mathf.Clamp(alpha, 0, alphaTarget) + 0.6f);
            yield return null;
        }
        time = 1f;
        tempTime = time;
        while (tempTime > 0f)
        {
            tempTime -= Time.deltaTime;
            float outline = (1f - tempTime / time) * outlineTarget;
            float overlay = (1f - tempTime / time) * overlayTarget;
            GetComponent<SpriteRenderer>().material.SetFloat("_OutLineAlpha", Mathf.Clamp(outline, 0, outlineTarget));
            GetComponent<SpriteRenderer>().material.SetFloat("_Overlay", Mathf.Clamp(overlay, 0, overlayTarget));
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        time = 1f;
        tempTime = time;
        while (tempTime > 0f)
        {
            tempTime -= Time.deltaTime;
            float outline = (1f - tempTime / time) * outlineTarget;
            GetComponent<SpriteRenderer>().material.SetFloat("_OutLineAlpha", Mathf.Clamp(1f - outline, 0, outlineTarget));
            yield return null;
        }
        onEndWindow?.Invoke();
    }

    public void TurnOnWindow(System.Action action)
    {
        if (play == null)
        {
            onEndWindow = action;
            play = StartCoroutine(Play());
            StartCoroutine(SetPosition());
        }
    }

    public void TurnOffWindow()
    {
        if (play != null)
        {
            StopCoroutine(play);
        }
        GetComponent<SpriteRenderer>().material.SetFloat("_BackgroundAlpha", 0);
        GetComponent<SpriteRenderer>().material.SetFloat("_Overlay", 0);
        GetComponent<SpriteRenderer>().material.SetFloat("_OutLineAlpha", 0);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<VideoPlayer>().time = 0f;
        GetComponent<VideoPlayer>().playbackSpeed = 0f;
        GetComponent<VideoPlayer>().SetDirectAudioMute(0, true);
        GetComponent<VideoPlayer>().audioOutputMode = VideoAudioOutputMode.None;
        play = null;
    }
}
