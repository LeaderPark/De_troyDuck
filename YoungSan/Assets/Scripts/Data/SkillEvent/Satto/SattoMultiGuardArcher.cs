using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SattoMultiGuardArcher : Installation
{

    Vector3[] spawnPosition;
    Vector3[] targetPosition;
    Vector3[] moveDir;
    public float maxDistance;
    public float spawnDistance;
    public float spearInterval;

    public int count;

    private Animator[] animator;
    private Rigidbody[] rigid;

    private Transform[] guardArchers;

    public Transform source;

    public AudioClip sound;

    void Awake()
    {
        int realCount = count * 2;
        guardArchers = new Transform[realCount];
        animator = new Animator[realCount];
        rigid = new Rigidbody[realCount];
        spawnPosition = new Vector3[realCount];
        targetPosition = new Vector3[realCount];
        moveDir = new Vector3[realCount];
        for (int i = 0; i < realCount; i++)
        {
            if (i != realCount - 1)
            {
                guardArchers[i] = GameObject.Instantiate(source.gameObject).transform;
                guardArchers[i].transform.parent = transform;
            }
            else
            {
                guardArchers[i] = source;
            }
            animator[i] = guardArchers[i].GetComponent<Animator>();
            rigid[i] = guardArchers[i].GetComponent<Rigidbody>();
            guardArchers[i].GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.white * 0.6f);
            guardArchers[i].gameObject.SetActive(true);
        }
    }

    public override void Play()
    {
        for (int i = 0; i < count; i++)
        {
            Transform[] spears = { guardArchers[i * 2], guardArchers[i * 2 + 1] };

            for (int j = 0; j < spears.Length; j++)
            {
                Transform item = spears[j];
                int index = i * 2 + j;

                item.GetComponent<SpriteRenderer>().enabled = false;

                targetPosition[index] = position;

                Vector3 dirVec = (targetPosition[index] - ownerEntity.transform.position).normalized;

                if (Vector3.Distance(position, ownerEntity.transform.position) > maxDistance)
                {
                    targetPosition[index] = dirVec * maxDistance + ownerEntity.transform.position;
                }

                spawnPosition[index] = Quaternion.AngleAxis((j == 0) ? -90 : 90, Vector3.up) * (-dirVec) * (i + spearInterval / 2) * spearInterval + ownerEntity.transform.position + -dirVec * spawnDistance;

                RaycastHit hit;

                Vector3 pos = spawnPosition[index];
                if (Physics.Raycast(new Ray(new Vector3(pos.x, 1000, pos.z), Vector3.down), out hit, 2000, LayerMask.GetMask(new string[] { "Ground" })))
                {
                    pos.y = hit.point.y;
                }
                item.transform.position = pos;

                moveDir[index] = dirVec;

                if (moveDir[index].x > 0)
                {
                    item.GetComponent<SpriteRenderer>().flipX = false;
                }
                else
                {
                    item.GetComponent<SpriteRenderer>().flipX = true;
                }


                StartCoroutine(PlaySpawn(index));
                StartCoroutine(End());
            }
        }
    }

    private IEnumerator End()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    private IEnumerator PlaySpawn(int index)
    {
        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
        Cloud cloud = poolManager.GetObject("Cloud").GetComponent<Cloud>();
        cloud.transform.position = spawnPosition[index] + Vector3.back * 0.1f;
        animator[index].Play("Idle");
        yield return cloud.Play();
        guardArchers[index].GetComponent<SpriteRenderer>().enabled = true;
        yield return cloud.End();
        animator[index].Play("Attack");

        SoundManager soundManager = ManagerObject.Instance.GetManager(ManagerType.SoundManager) as SoundManager;
        soundManager.SoundStart(sound.name, guardArchers[index].transform);

        StartCoroutine(Projectile(moveDir[index].x, moveDir[index].z, "Arrow", skillData, spawnPosition[index], 1f));
        yield return new WaitForSeconds(1f);
        animator[index].Play("Idle");
    }


    private IEnumerator Projectile(float inputX, float inputY, string objectName, SkillData skillData, Vector3 position, float startTime)
    {
        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
        yield return new WaitForSeconds(startTime);

        Projectile projectile = poolManager.GetObject(objectName).GetComponent<Projectile>();
        projectile.SetData(position, new Vector3(inputX, 0, inputY).normalized, skillData, false);
    }

}
