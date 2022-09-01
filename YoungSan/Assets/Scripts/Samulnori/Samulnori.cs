using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samulnori : MonoBehaviour
{
    public List<Entity> samulEntities = new List<Entity>();

    public float radius;
    int samulCount;
    bool samulDead;


    void Awake()
    {
        samulCount = samulEntities.Count;
    }

    void Start()
    {
        Play();
    }

    public void Play()
    {
        StartCoroutine(PlayRoutine());
        StartCoroutine(CheckRoutine());
    }

    IEnumerator CheckRoutine()
    {
        List<int> removes = new List<int>();
        while (true)
        {

            for (int i = 0; i < samulEntities.Count; i++)
            {
                if (samulEntities[i].isDead)
                {
                    removes.Add(i);
                }
            }

            for (int i = 0; i < removes.Count; i++)
            {
                samulEntities.RemoveAt(removes[removes.Count - i - 1]);
                samulCount--;
                samulDead = true;
            }

            removes.Clear();

            yield return null;
        }
    }

    IEnumerator PlayRoutine()
    {



        yield return null;
    }
}