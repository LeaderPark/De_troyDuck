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
                    yield return MoveRoutine();
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
        if (state == BellState.Bell)
        {
            timeStack = 0f;
            GetComponent<Animator>().Play(bellClip.name, 0, 0.0f);
        }
        state = BellState.Bell;
    }

    public void Move(bool isRight)
    {
        if (state == BellState.Move)
        {
            timeStack = 0f;
            GetComponent<Animator>().Play(moveClip.name, 0, 0.0f);
        }
        state = BellState.Move;
        this.isRight = isRight;
    }

    IEnumerator MoveRoutine()
    {
        timeStack = 0f;
        while (timeStack < bellClip.length)
        {
            GetComponent<Animator>().Play(moveClip.name);
            timeStack += Time.deltaTime;
            GetComponent<SpriteRenderer>().flipX = !isRight;
            yield return null;
        }
        state = BellState.Idle;
        yield return null;
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
