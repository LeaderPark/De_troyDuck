using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDelayUI : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer targetRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            Play();
        }
    }

    public IEnumerator AfterImageInActive(GameObject afterImage, float time)
    {
        yield return new WaitForSeconds(time);
        afterImage.SetActive(false);
    }

    public void SetTarget(SpriteRenderer sr)
    {
        targetRenderer = sr;
    }

    public void Play()
    {
        spriteRenderer.transform.localScale = targetRenderer.transform.localScale;
        spriteRenderer.flipX = targetRenderer.flipX;
        transform.position = new Vector3(targetRenderer.transform.position.x, targetRenderer.transform.position.y + 3f, targetRenderer.transform.position.z);
    }
}
