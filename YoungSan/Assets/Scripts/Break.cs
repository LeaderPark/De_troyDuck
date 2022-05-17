using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : MonoBehaviour
{
    public SpriteRenderer[] breakPieces;
    public Sprite sprite;
    public Vector3 minVelocity;
    public Vector3 maxVelocity;
    public float time;

    bool loading;


    void Awake()
    {
        breakPieces = new SpriteRenderer[transform.childCount];
        for (int i = 0; i < breakPieces.Length; i++)
        {
            breakPieces[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();
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
            breakPieces[i].sprite = sprite;
            Rigidbody rigid = breakPieces[i].GetComponent<Rigidbody>();
            rigid.velocity = (new Vector3(Random.Range(minVelocity.x, maxVelocity.x), Random.Range(minVelocity.y, maxVelocity.y), Random.Range(minVelocity.z, maxVelocity.z)));
            rigid.angularVelocity = (new Vector3(0, 0, Random.Range(0, 30)));
        }
        
        yield return new WaitForSeconds(time);

        gameObject.SetActive(false);
        loading = false;

        yield return null;
    }

}

