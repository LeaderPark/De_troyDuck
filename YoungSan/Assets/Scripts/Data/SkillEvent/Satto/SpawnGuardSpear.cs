using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGuardSpear : Installation
{

    Vector3 spawnPosition;
    Vector3 targetPosition;
    Vector3 moveDir;
    public float maxDistance;
    public float attackDistance;


    public bool active;

    private Animator animator;
    private Rigidbody rigid;

    public HitBox leftHitBox;
    public HitBox rightHitBox;

    public AudioClip sound;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.white * 0.6f);
    }

    public override void Play()
    {
        GetComponent<SpriteRenderer>().enabled = false;

        targetPosition = position;

        Vector3 dirVec = (targetPosition - ownerEntity.transform.position).normalized;

        if (Vector3.Distance(position, ownerEntity.transform.position) > maxDistance)
        {
            targetPosition = dirVec * maxDistance + ownerEntity.transform.position;
        }

        spawnPosition = Quaternion.AngleAxis(Random.Range(-60f, 60f), Vector3.up) * (-dirVec) * attackDistance + targetPosition;

        RaycastHit hit;

        Vector3 pos = spawnPosition;
        if (Physics.Raycast(new Ray(new Vector3(pos.x, 1000, pos.z), Vector3.down), out hit, 2000, LayerMask.GetMask(new string[] { "Ground" })))
        {
            pos.y = hit.point.y;
        }

        transform.position = pos;

        moveDir = (targetPosition - spawnPosition).normalized;

        if (moveDir.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        leftHitBox.skillData = skillData;
        rightHitBox.skillData = skillData;

        leftHitBox.transform.parent.gameObject.SetActive(false);

        StartCoroutine(PlaySpawn());
    }

    private IEnumerator PlaySpawn()
    {
        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
        Cloud cloud = poolManager.GetObject("Cloud").GetComponent<Cloud>();
        cloud.transform.position = spawnPosition + Vector3.back * 0.1f;
        animator.Play("Idle");
        yield return cloud.Play();
        GetComponent<SpriteRenderer>().enabled = true;
        yield return cloud.End();
        animator.Play("Attack");
        StartCoroutine(HitBox());

        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        if (skillData.skillSet.entity.gameObject.layer != 6) moveDir = (gameManager.Player.transform.position - spawnPosition).normalized;

        if (moveDir.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        StartCoroutine(Dash(6, 0.4f, 0.4f));
        yield return new WaitForSeconds(1f);
        animator.Play("Idle");
        leftHitBox.ClearTargetSet();
        rightHitBox.ClearTargetSet();
        gameObject.SetActive(false);
    }

    private IEnumerator HitBox()
    {
        while (true)
        {
            if (active)
            {
                leftHitBox.transform.parent.gameObject.SetActive(true);
                if (moveDir.x > 0)
                {
                    leftHitBox.gameObject.SetActive(false);
                    rightHitBox.gameObject.SetActive(true);
                }
                else
                {
                    leftHitBox.gameObject.SetActive(true);
                    rightHitBox.gameObject.SetActive(false);
                }
            }
            else
            {
                leftHitBox.transform.parent.gameObject.SetActive(false);
            }
            yield return null;
        }
    }

    private IEnumerator Dash(float speed, float startTime, float time)
    {
        SoundManager soundManager = ManagerObject.Instance.GetManager(ManagerType.SoundManager) as SoundManager;
        yield return new WaitForSeconds(startTime);
        soundManager.SoundStart(sound.name, transform);
        rigid.velocity = moveDir * speed;

        RaycastHit hit;

        Vector3 pos = transform.position;
        if (Physics.Raycast(new Ray(new Vector3(pos.x, 1000, pos.z), Vector3.down), out hit, 2000, LayerMask.GetMask(new string[] { "Ground" })))
        {
            pos.y = hit.point.y;
        }
        transform.position = pos;

        yield return new WaitForSeconds(time);
        rigid.velocity = Vector3.zero;
    }
}
