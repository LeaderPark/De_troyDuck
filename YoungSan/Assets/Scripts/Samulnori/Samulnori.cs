using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samulnori : MonoBehaviour
{
    public List<Entity> samulEntities = new List<Entity>();
    List<EntityEvent> samulEntityEvents = new List<EntityEvent>(4);

    public float radius;
    public float assembleRadius;
    public float allAttackRadius;
    public float rushAttackRadius;
    public float rushAttackInterval;

    int samulCount;
    bool samulDead;

    void Awake()
    {
        samulCount = 4;

        int shuffleIndex = 3;
        for (int index = 0; index < samulEntities.Count - 2; index++)
        {
            Entity temp = samulEntities[index];
            shuffleIndex = Random.Range(index + 1, samulEntities.Count);
            samulEntities[index] = samulEntities[shuffleIndex];
            samulEntities[shuffleIndex] = temp;
        }

        foreach (Entity entity in samulEntities)
        {
            samulEntityEvents.Add(entity.GetComponent<EntityEvent>());
        }
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
                samulEntityEvents.RemoveAt(removes[removes.Count - i - 1]);
                samulCount--;
                samulDead = true;
            }

            removes.Clear();

            if (samulCount == 0)
            {
                Subjugation();
                break;
            }

            yield return null;
        }
    }

    float standardAngle = 0;

    IEnumerator PlayRoutine()
    {
        yield return PositionRoutine();

        while (samulCount > 0)
        {
            if (samulDead)
            {
                samulDead = false;
                yield return AssembleRoutine();
                yield return new WaitForSeconds(1);

                for (int index = 0; index < samulCount; index++)
                {
                    samulEntities[index].clone.SetMaxStat(StatCategory.Speed, (int)(samulEntities[index].clone.GetMaxStat(StatCategory.Speed) * 1.5f));
                    samulEntities[index].clone.SetStat(StatCategory.Speed, samulEntities[index].clone.GetMaxStat(StatCategory.Speed));
                }
                yield return PositionRoutine();
            }
            else
            {
                yield return RotationRoutine();
                if (samulDead) continue;
                if (Random.Range(0, 2) == 0)
                {
                    yield return OneAttackRoutine();
                    if (samulDead) continue;
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        yield return AllAttackRoutine();
                        if (samulDead) continue;
                    }
                    else
                    {
                        yield return RushAttackRoutine();
                        if (samulDead) continue;
                    }
                }
            }
            yield return null;
        }

        yield return null;
    }

    IEnumerator PositionRoutine()
    {
        Vector2[] positions = new Vector2[samulCount];
        standardAngle = Random.Range(0, 180);
        for (int index = 0; index < samulCount; index++)
        {
            Vector3 pos3 = transform.position + Quaternion.AngleAxis(standardAngle + 360 * index / samulCount, Vector3.up) * (Vector3.forward * radius);
            positions[index] = new Vector2(pos3.x, pos3.z);
        }

        bool dontArrived = true;
        while (dontArrived)
        {
            dontArrived = false;
            float epsilon = samulEntities[0].clone.GetStat(StatCategory.Speed) / 30f;
            for (int index = 0; index < samulCount; index++)
            {
                if (Vector2.Distance(new Vector2(samulEntities[index].transform.position.x, samulEntities[index].transform.position.z), positions[index]) > epsilon)
                {
                    Vector2 moveDirection = positions[index] - new Vector2(samulEntities[index].transform.position.x, samulEntities[index].transform.position.z);
                    if (!samulDead) samulEntityEvents[index].CallEvent(EventCategory.Move, moveDirection.x, moveDirection.y, moveDirection.x > 0, new Vector3(positions[index].x, samulEntities[index].transform.position.y, positions[index].y));
                    dontArrived = true;
                }
                else
                {
                    samulEntityEvents[index].CallEvent(EventCategory.Move, 0, 0, !samulEntities[index].GetComponent<SpriteRenderer>().flipX, samulEntities[index].transform.position);
                }
            }
            yield return null;
        }
    }

    IEnumerator RotationRoutine()
    {
        Vector2[] positions = new Vector2[samulCount];
        standardAngle -= 360 / samulCount * Time.deltaTime;
        float turnCount = 0;
        while (!samulDead)
        {
            for (int index = 0; index < samulCount; index++)
            {
                Vector3 pos3 = transform.position + Quaternion.AngleAxis(standardAngle + 360 * index / samulCount, Vector3.up) * (Vector3.forward * radius);
                positions[index] = new Vector2(pos3.x, pos3.z);
            }

            float epsilon = samulEntities[0].clone.GetStat(StatCategory.Speed) / 30f;
            for (int index = 0; index < samulCount; index++)
            {
                float distance = Vector2.Distance(new Vector2(samulEntities[index].transform.position.x, samulEntities[index].transform.position.z), positions[index]);
                if (distance > epsilon)
                {
                    Vector2 moveDirection = positions[index] - new Vector2(samulEntities[index].transform.position.x, samulEntities[index].transform.position.z);
                    if (!samulDead) samulEntityEvents[index].CallEvent(EventCategory.Move, moveDirection.x, moveDirection.y, moveDirection.x > 0, new Vector3(positions[index].x, samulEntities[index].transform.position.y, positions[index].y));
                }
                else
                {
                    samulEntities[index].GetProcessor(typeof(Processor.Animate)).AddCommand("LockTime", new object[] { 0f });
                    samulEntityEvents[index].CallEvent(EventCategory.Move, 0, 0, !samulEntities[index].GetComponent<SpriteRenderer>().flipX, samulEntities[index].transform.position);
                    standardAngle -= 360 / samulCount * Time.deltaTime;
                    if (standardAngle < 0) standardAngle += 360;

                    turnCount += 100 / samulCount * Time.deltaTime;
                }
            }

            if (turnCount >= 50f)
            {
                yield break;
            }

            yield return null;
        }
    }

    IEnumerator AssembleRoutine()
    {
        Vector2[] positions = new Vector2[samulCount];
        for (int index = 0; index < samulCount; index++)
        {
            Vector3 pos3 = transform.position + (samulEntities[index].transform.position - transform.position).normalized * assembleRadius;
            positions[index] = new Vector2(pos3.x, pos3.z);
        }

        bool dontArrived = true;
        while (dontArrived)
        {
            dontArrived = false;
            float epsilon = samulEntities[0].clone.GetStat(StatCategory.Speed) / 30f;
            for (int index = 0; index < samulCount; index++)
            {
                if (Vector2.Distance(new Vector2(samulEntities[index].transform.position.x, samulEntities[index].transform.position.z), positions[index]) > epsilon)
                {
                    Vector2 moveDirection = positions[index] - new Vector2(samulEntities[index].transform.position.x, samulEntities[index].transform.position.z);
                    if (!samulDead) samulEntityEvents[index].CallEvent(EventCategory.Move, moveDirection.x, moveDirection.y, moveDirection.x > 0, new Vector3(positions[index].x, samulEntities[index].transform.position.y, positions[index].y));
                    dontArrived = true;
                }
                else
                {
                    samulEntityEvents[index].CallEvent(EventCategory.Move, 0, 0, !samulEntities[index].GetComponent<SpriteRenderer>().flipX, samulEntities[index].transform.position);
                }
            }

            if (!dontArrived)
            {
                for (int index = 0; index < samulCount; index++)
                {
                    Vector3 attackDirection = transform.position - samulEntities[index].transform.position;

                    samulEntityEvents[index].CallEvent(EventCategory.DefaultAttack, attackDirection.x, attackDirection.z, !samulEntities[index].GetComponent<SpriteRenderer>().flipX, transform.position);
                }
            }

            yield return null;
        }
    }

    IEnumerator OneAttackRoutine()
    {
        int randomSamul = Random.Range(0, samulCount);
        samulEntities[randomSamul].GetComponent<StateMachine.StateMachine>().enabled = true;

        Vector2[] positions = new Vector2[samulCount];

        standardAngle -= 360 / samulCount * Time.deltaTime;

        int arrivedCount = 0;
        float attackTime = Random.Range(10, 20);

        bool attacking = true;

        while (arrivedCount != samulCount)
        {
            arrivedCount = 0;

            attackTime -= Time.deltaTime;
            if (attackTime <= 0)
            {
                attacking = false;
                samulEntities[randomSamul].GetComponent<StateMachine.StateMachine>().enabled = false;
            }

            for (int index = 0; index < samulCount; index++)
            {
                Vector3 pos3 = transform.position + Quaternion.AngleAxis(standardAngle + 360 * index / samulCount, Vector3.up) * (Vector3.forward * radius);
                positions[index] = new Vector2(pos3.x, pos3.z);
            }

            float epsilon = samulEntities[0].clone.GetStat(StatCategory.Speed) / 30f;
            for (int index = 0; index < samulCount; index++)
            {
                if (attacking && randomSamul == index) continue;
                else
                {
                    float distance = Vector2.Distance(new Vector2(samulEntities[index].transform.position.x, samulEntities[index].transform.position.z), positions[index]);
                    if (distance > epsilon)
                    {
                        Vector2 moveDirection = positions[index] - new Vector2(samulEntities[index].transform.position.x, samulEntities[index].transform.position.z);
                        if (!samulDead) samulEntityEvents[index].CallEvent(EventCategory.Move, moveDirection.x, moveDirection.y, moveDirection.x > 0, new Vector3(positions[index].x, samulEntities[index].transform.position.y, positions[index].y));
                        arrivedCount++;
                    }
                    else
                    {
                        samulEntities[index].GetProcessor(typeof(Processor.Animate)).AddCommand("LockTime", new object[] { 0f });
                        samulEntityEvents[index].CallEvent(EventCategory.Move, 0, 0, !samulEntities[index].GetComponent<SpriteRenderer>().flipX, samulEntities[index].transform.position);
                        standardAngle -= 360 / samulCount * Time.deltaTime;
                        if (standardAngle < 0) standardAngle += 360;
                    }
                }
            }

            yield return null;
        }
        for (int index = 0; index < samulCount; index++)
        {
            samulEntities[index].GetComponent<StateMachine.StateMachine>().enabled = false;
        }
    }

    IEnumerator AllAttackRoutine()
    {
        Vector2[] positions = new Vector2[samulCount];
        for (int index = 0; index < samulCount; index++)
        {
            Vector3 pos3 = transform.position + Quaternion.AngleAxis(standardAngle + 360 * index / samulCount, Vector3.up) * (Vector3.forward * allAttackRadius);
            positions[index] = new Vector2(pos3.x, pos3.z);
        }

        bool dontArrived = true;
        while (dontArrived)
        {
            dontArrived = false;
            float epsilon = samulEntities[0].clone.GetStat(StatCategory.Speed) / 30f;
            for (int index = 0; index < samulCount; index++)
            {
                if (Vector2.Distance(new Vector2(samulEntities[index].transform.position.x, samulEntities[index].transform.position.z), positions[index]) > epsilon)
                {
                    Vector2 moveDirection = positions[index] - new Vector2(samulEntities[index].transform.position.x, samulEntities[index].transform.position.z);
                    if (!samulDead) samulEntityEvents[index].CallEvent(EventCategory.Move, moveDirection.x, moveDirection.y, moveDirection.x > 0, new Vector3(positions[index].x, samulEntities[index].transform.position.y, positions[index].y));
                    dontArrived = true;
                }
                else
                {
                    samulEntityEvents[index].CallEvent(EventCategory.Move, 0, 0, !samulEntities[index].GetComponent<SpriteRenderer>().flipX, samulEntities[index].transform.position);
                }
            }

            if (!dontArrived)
            {
                for (int index = 0; index < samulCount; index++)
                {
                    Vector3 attackDirection = transform.position - samulEntities[index].transform.position;

                    samulEntityEvents[index].CallEvent(EventCategory.DefaultAttack, attackDirection.x, attackDirection.z, !samulEntities[index].GetComponent<SpriteRenderer>().flipX, transform.position);
                }
            }

            yield return null;
        }
    }

    IEnumerator RushAttackRoutine()
    {
        Entity[] rushOrder = new Entity[samulCount];
        Vector2[] positions = new Vector2[rushOrder.Length];

        for (int index = 0; index < samulCount; index++)
        {
            samulEntities[index].clone.SetMaxStat(StatCategory.Speed, (int)(samulEntities[index].clone.GetMaxStat(StatCategory.Speed) * 2f));
            samulEntities[index].clone.SetStat(StatCategory.Speed, samulEntities[index].clone.GetMaxStat(StatCategory.Speed));
        }

        for (int index = 0; index < rushOrder.Length; index++)
        {
            rushOrder[index] = samulEntities[index];
        }

        int shuffleIndex = 3;
        for (int index = 0; index < rushOrder.Length - 2; index++)
        {
            Entity temp = rushOrder[index];
            shuffleIndex = Random.Range(index + 1, rushOrder.Length);
            rushOrder[index] = rushOrder[shuffleIndex];
            rushOrder[shuffleIndex] = temp;
        }

        int randomRushCount = Random.Range(3, 5);

        for (int rushStack = 0; rushStack < randomRushCount; rushStack++)
        {
            float randomAngle = Random.Range(0f, 360f);

            for (int index = 0; index < positions.Length; index++)
            {
                Vector3 pos3 = transform.position + Quaternion.AngleAxis(randomAngle, Vector3.up) * (Vector3.forward * (rushAttackRadius + rushAttackInterval * index));
                positions[index] = new Vector2(pos3.x, pos3.z);
            }

            int samulStack = 0;
            if (rushStack == 0)
            {
                while (samulStack < samulCount)
                {
                    samulStack = 0;
                    float epsilon = samulEntities[0].clone.GetStat(StatCategory.Speed) / 30f;
                    for (int index = 0; index < samulCount; index++)
                    {
                        if (Vector2.Distance(new Vector2(samulEntities[index].transform.position.x, samulEntities[index].transform.position.z), positions[index]) > epsilon)
                        {
                            Vector2 moveDirection = positions[index] - new Vector2(samulEntities[index].transform.position.x, samulEntities[index].transform.position.z);
                            if (!samulDead) samulEntityEvents[index].CallEvent(EventCategory.Move, moveDirection.x, moveDirection.y, moveDirection.x > 0, new Vector3(positions[index].x, samulEntities[index].transform.position.y, positions[index].y));
                        }
                        else
                        {
                            samulEntityEvents[index].CallEvent(EventCategory.Move, 0, 0, !samulEntities[index].GetComponent<SpriteRenderer>().flipX, samulEntities[index].transform.position);
                            samulStack++;
                        }
                    }

                    yield return null;
                }
            }
            else
            {
                for (int index = 0; index < positions.Length; index++)
                {
                    samulEntities[index].transform.position = new Vector3(positions[index].x, samulEntities[index].transform.position.y, positions[index].y);
                }
            }

            for (int index = 0; index < positions.Length; index++)
            {
                Vector3 pos3 = transform.position + Quaternion.AngleAxis(180 + randomAngle, Vector3.up) * (Vector3.forward * (rushAttackRadius + rushAttackInterval * (positions.Length - 1 - index)));
                positions[index] = new Vector2(pos3.x, pos3.z);
            }

            samulStack = 0;
            while (samulStack < samulCount)
            {
                samulStack = 0;
                float epsilon = samulEntities[0].clone.GetStat(StatCategory.Speed) / 30f;
                for (int index = 0; index < samulCount; index++)
                {
                    if (Vector2.Distance(new Vector2(samulEntities[index].transform.position.x, samulEntities[index].transform.position.z), positions[index]) > epsilon)
                    {
                        Vector2 moveDirection = positions[index] - new Vector2(samulEntities[index].transform.position.x, samulEntities[index].transform.position.z);
                        if (!samulDead) samulEntityEvents[index].CallEvent(EventCategory.Move, moveDirection.x, moveDirection.y, moveDirection.x > 0, new Vector3(positions[index].x, samulEntities[index].transform.position.y, positions[index].y));
                    }
                    else
                    {
                        samulEntityEvents[index].CallEvent(EventCategory.Move, 0, 0, !samulEntities[index].GetComponent<SpriteRenderer>().flipX, samulEntities[index].transform.position);
                        samulStack++;
                    }
                }

                yield return null;
            }
        }

        for (int index = 0; index < samulCount; index++)
        {
            samulEntities[index].clone.SetMaxStat(StatCategory.Speed, (int)(samulEntities[index].clone.GetMaxStat(StatCategory.Speed) * 0.5f));
            samulEntities[index].clone.SetStat(StatCategory.Speed, samulEntities[index].clone.GetMaxStat(StatCategory.Speed));
        }
    }

    void Subjugation()
    {
        Debug.Log("공략 ㅊㅊ");
    }
}