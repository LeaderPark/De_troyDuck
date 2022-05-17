using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : MonoBehaviour
{
    private SpriteRenderer[] breakPieces;
    
    public BreakSpriteData[] breakSpriteData;
    public Vector3 minVelocity;
    public Vector3 maxVelocity;
    public float time;

    bool loading;


    void Awake()
    {
        int count = 0;
        foreach (var item in breakSpriteData)
        {
            count += item.count;
        }

        breakPieces = new SpriteRenderer[count];
        for (int i = 0; i < breakPieces.Length; i++)
        {
            breakPieces[i] = Instantiate(transform.GetChild(0).gameObject, transform).GetComponent<SpriteRenderer>();
            breakPieces[i].gameObject.SetActive(false);
        }
        
        {
            int i = 0;

            foreach (var item in breakSpriteData)
            {
                for (int j = 0; j < item.count; j++)
                {
                    
                    breakPieces[i].sprite = item.sprite;

                    i++;
                }
            }
        }

    }

    public void Play()
    {
        if (loading) return;
        loading = true;

        StartCoroutine(Process());
    }

    IEnumerator Process()
    {
        for (int i = 0; i < breakPieces.Length; i++)
        {
            breakPieces[i].transform.position = transform.position;
            Rigidbody rigid = breakPieces[i].GetComponent<Rigidbody>();
            rigid.velocity = (new Vector3(Random.Range(minVelocity.x, maxVelocity.x), Random.Range(minVelocity.y, maxVelocity.y), Random.Range(minVelocity.z, maxVelocity.z)));
            rigid.angularVelocity = (new Vector3(0, 0, Random.Range(0, 30)));
            breakPieces[i].gameObject.SetActive(true);
        }
        
        yield return new WaitForSeconds(time);

        gameObject.SetActive(false);
        loading = false;

        yield return null;
    }

}

[System.Serializable]
public struct BreakSpriteData
{
    public Sprite sprite;
    public int count;
}