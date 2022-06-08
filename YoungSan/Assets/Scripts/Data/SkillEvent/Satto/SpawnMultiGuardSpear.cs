using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMultiGuardSpear : Installation
{

    Vector3[] spawnPosition;
    Vector3[] targetPosition;
    Vector3[] moveDir;
    public float maxDistance;
    public float attackDistance;
    public float spearInterval;

    public int count;

    private Animator[] animator;
    private Rigidbody[] rigid;

    private HitBox[] leftHitBox;
    private HitBox[] rightHitBox;

    private SattoMultiChild[] guardSpears;

    public Transform source;

    void Awake()
    {
        int realCount = count * 2;
        guardSpears = new SattoMultiChild[realCount];
        animator = new Animator[realCount];
        rigid = new Rigidbody[realCount];
        leftHitBox = new HitBox[realCount];
        rightHitBox = new HitBox[realCount];
        spawnPosition = new Vector3[realCount];
        targetPosition = new Vector3[realCount];
        moveDir = new Vector3[realCount];
        for (int i = 0; i < realCount; i++)
        {
            if (i != realCount - 1)
            {
                guardSpears[i] = GameObject.Instantiate(source.gameObject).GetComponent<SattoMultiChild>();
                guardSpears[i].transform.parent = transform;
            }
            else
            {
                guardSpears[i] = source.GetComponent<SattoMultiChild>();
            }
            animator[i] = guardSpears[i].GetComponent<Animator>();
            rigid[i] = guardSpears[i].GetComponent<Rigidbody>();
            leftHitBox[i] = guardSpears[i].transform.GetChild(0).GetChild(0).GetComponent<HitBox>();
            rightHitBox[i] = guardSpears[i].transform.GetChild(0).GetChild(1).GetComponent<HitBox>();
            guardSpears[i].gameObject.SetActive(true);
        }
    }

    public override void Play()
    {
        for (int i = 0; i < count; i++)
        {
            SattoMultiChild[] spears = { guardSpears[i * 2], guardSpears[i * 2 + 1] };

            for (int j = 0; j < spears.Length; j++)
            {
                SattoMultiChild item = spears[j];
                int index = i * 2 + j;

                item.GetComponent<SpriteRenderer>().enabled = false;

                targetPosition[index] = position;

                Vector3 dirVec = (targetPosition[index] - ownerEntity.transform.position).normalized;

                if (Vector3.Distance(position, ownerEntity.transform.position) > maxDistance)
                {
                    targetPosition[index] = dirVec * maxDistance + ownerEntity.transform.position;
                }

                spawnPosition[index] = Quaternion.AngleAxis((j == 0) ? -90 : 90, Vector3.up) * (-dirVec) * attackDistance + targetPosition[index] + dirVec * (i - count / 2) * spearInterval;

                item.transform.position = spawnPosition[index];

                moveDir[index] = (targetPosition[index] + dirVec * (i - count / 2) * spearInterval - spawnPosition[index]).normalized;

                if (moveDir[index].x > 0)
                {
                    item.GetComponent<SpriteRenderer>().flipX = false;
                }
                else
                {
                    item.GetComponent<SpriteRenderer>().flipX = true;
                }

                leftHitBox[index].skillData = skillData;
                rightHitBox[index].skillData = skillData;

                leftHitBox[index].transform.parent.gameObject.SetActive(false);

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
        guardSpears[index].GetComponent<SpriteRenderer>().enabled = true;
        yield return cloud.End();
        animator[index].Play("Attack");
        StartCoroutine(HitBox(index));

        StartCoroutine(Dash(8, 0.4f, 0.4f, index));
        yield return new WaitForSeconds(1f);
        animator[index].Play("Idle");
        leftHitBox[index].ClearTargetSet();
        rightHitBox[index].ClearTargetSet();
    }

    private IEnumerator HitBox(int index)
    {
        while (true)
        {
            if (guardSpears[index].active)
            {
                leftHitBox[index].transform.parent.gameObject.SetActive(true);
                if (moveDir[index].x > 0)
                {
                    leftHitBox[index].gameObject.SetActive(false);
                    rightHitBox[index].gameObject.SetActive(true);
                }
                else
                {
                    leftHitBox[index].gameObject.SetActive(true);
                    rightHitBox[index].gameObject.SetActive(false);
                }
            }
            else
            {
                leftHitBox[index].transform.parent.gameObject.SetActive(false);
            }
            yield return null;
        }
    }

    private IEnumerator Dash(float speed, float startTime, float time, int index)
    {
        yield return new WaitForSeconds(startTime);
        rigid[index].velocity = moveDir[index] * speed;
        yield return new WaitForSeconds(time);
        rigid[index].velocity = Vector3.zero;
    }
}
