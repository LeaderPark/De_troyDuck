using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bell : MonoBehaviour
{
    public AnimationClip bellClip;
    public AnimationClip moveClip;

    BellState state;
    bool isRight;
    float timeStack = 0;

    enum BellState
    {
        Idle,
        Move,
        Bell
    }

    void Start()
    {
        StartCoroutine(Follow());
        StartCoroutine(Animation());
    }

    IEnumerator Animation()
    {
        Animator animator = GetComponent<Animator>();
        state = BellState.Idle;
        animator.Play("Idle");

        while (true)
        {
            switch (state)
            {
                case BellState.Move:
                    GetComponent<SpriteRenderer>().flipX = !isRight;
                    animator.Play("Move");
                    break;
                case BellState.Bell:
                    yield return RingRoutine();
                    break;
                default:
                    animator.Play("Idle");
                    break;
            }
            yield return null;
        }
    }

    IEnumerator Follow()
    {
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;

        while (true)
        {
            Player player = gameManager.Player;
            if (player == null || !player.enabled)
            {
                GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                GetComponent<SpriteRenderer>().enabled = true;
                transform.position = player.transform.position + Vector3.up * player.GetComponent<Entity>().entityData.uiPos;
            }
            yield return null;
        }
    }

    public void Ring()
    {
        SoundManager soundManager = ManagerObject.Instance.GetManager(ManagerType.SoundManager) as SoundManager;
        soundManager.SoundStart("Bell", transform);
        if (state == BellState.Bell)
        {
            timeStack = 0f;
            GetComponent<Animator>().Play(bellClip.name, 0, 0.0f);
        }
        state = BellState.Bell;
    }

    public void Move(bool isRight)
    {
        state = BellState.Move;
        this.isRight = isRight;
    }

    public void Idle()
    {
        state = BellState.Idle;
    }

    IEnumerator RingRoutine()
    {
        timeStack = 0f;
        while (timeStack < bellClip.length)
        {
            GetComponent<Animator>().Play(bellClip.name);
            timeStack += Time.deltaTime;
            yield return null;
        }
        state = BellState.Idle;
        yield return null;
    }
}