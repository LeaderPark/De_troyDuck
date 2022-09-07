using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenRoof : MonoBehaviour
{
    public RoofSquare[] roofSquares;

    public Transform[] fadeObjects;

    List<Rect> roofSquareRects;

    Coroutine roofFadeRoutine;

    bool isOpen;

    void Awake()
    {
        isOpen = false;
        if (roofSquares == null || roofSquares.Length == 0) return;
        roofSquareRects = new List<Rect>(roofSquares.Length);
        foreach (RoofSquare roofSquare in roofSquares)
        {
            roofSquareRects.Add(new Rect(roofSquare.center - roofSquare.size / 2 + new Vector2(transform.position.x, transform.position.z), roofSquare.size));
        }
    }

    void Update()
    {
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        Vector3 playerPosition = gameManager.Player.transform.position;

        if (roofSquareRects == null || roofSquareRects.Count == 0) return;
        bool inBound = false;
        foreach (Rect rect in roofSquareRects)
        {
            if (rect.Contains(new Vector2(playerPosition.x, playerPosition.z)))
            {
                inBound = true;
                break;
            }
        }

        if (inBound != isOpen)
        {
            if (fadeObjects == null || fadeObjects.Length == 0) return;

            foreach (Transform trm in fadeObjects)
            {
                if (roofFadeRoutine != null)
                {
                    StopCoroutine(roofFadeRoutine);
                }
                roofFadeRoutine = StartCoroutine((inBound ? FadeOut() : FadeIn()));
                isOpen = !isOpen;
            }
        }
    }

    IEnumerator FadeOut()
    {
        const float targetAlpha = 0f;
        SpriteRenderer[] srs = new SpriteRenderer[fadeObjects.Length];
        for (int objectIndex = 0; objectIndex < fadeObjects.Length; objectIndex++)
        {
            srs[objectIndex] = fadeObjects[objectIndex].GetComponent<SpriteRenderer>();
            srs[objectIndex].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
        float currentAlpha = srs[0].color.a;

        const float timeScale = 2f;
        float time = (currentAlpha - targetAlpha) / timeScale;

        while (time > 0)
        {
            time -= Time.deltaTime;
            for (int objectIndex = 0; objectIndex < srs.Length; objectIndex++)
            {
                Color temp = srs[objectIndex].color;
                temp.a = Mathf.Lerp(currentAlpha, targetAlpha, 1 - time / (1 / timeScale));
                srs[objectIndex].color = temp;
            }
            yield return null;
        }

        for (int objectIndex = 0; objectIndex < srs.Length; objectIndex++)
        {
            Color temp = srs[objectIndex].color;
            temp.a = targetAlpha;
            srs[objectIndex].color = temp;
        }
    }

    IEnumerator FadeIn()
    {
        const float targetAlpha = 1f;
        SpriteRenderer[] srs = new SpriteRenderer[fadeObjects.Length];
        for (int objectIndex = 0; objectIndex < fadeObjects.Length; objectIndex++)
        {
            srs[objectIndex] = fadeObjects[objectIndex].GetComponent<SpriteRenderer>();
            srs[objectIndex].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
        float currentAlpha = srs[0].color.a;

        const float timeScale = 2f;
        float time = (targetAlpha - currentAlpha) / timeScale;

        while (time > 0)
        {
            time -= Time.deltaTime;
            for (int objectIndex = 0; objectIndex < srs.Length; objectIndex++)
            {
                Color temp = srs[objectIndex].color;
                temp.a = Mathf.Lerp(currentAlpha, targetAlpha, 1 - time / (1 / timeScale));
                srs[objectIndex].color = temp;
            }
            yield return null;
        }

        for (int objectIndex = 0; objectIndex < srs.Length; objectIndex++)
        {
            Color temp = srs[objectIndex].color;
            temp.a = targetAlpha;
            srs[objectIndex].color = temp;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (roofSquares == null || roofSquares.Length == 0) return;
        foreach (RoofSquare roofSquare in roofSquares)
        {
            Vector3 center = transform.position + new Vector3(roofSquare.center.x, 0, roofSquare.center.y);
            Vector3 halfSize = new Vector3(roofSquare.size.x, 0, roofSquare.size.y) / 2;

            Vector3 ll = center - halfSize;
            Vector3 rl = center + new Vector3(halfSize.x, 0, -halfSize.z);
            Vector3 lr = center + new Vector3(-halfSize.x, 0, halfSize.z);
            Vector3 rr = center + halfSize;

            Gizmos.DrawLine(ll, rl);
            Gizmos.DrawLine(ll, lr);
            Gizmos.DrawLine(rl, rr);
            Gizmos.DrawLine(lr, rr);
            Gizmos.DrawLine(ll, rr);
            Gizmos.DrawLine(lr, rl);
        }
    }
}


[System.Serializable]
public struct RoofSquare
{
    public Vector2 center;
    public Vector2 size;
}
