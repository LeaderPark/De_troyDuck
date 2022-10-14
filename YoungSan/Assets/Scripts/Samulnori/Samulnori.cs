using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samulnori : MonoBehaviour
{
    public List<Entity> samulEntities = new List<Entity>();
    List<EntityEvent> samulEntityEvents = new List<EntityEvent>(4);
    public RotationAttack rotationAttack;

    public SamulnoriSound[] samulnoriSounds;

    public float radius;
    public float assembleRadius;
    public float allAttackRadius;
    public float rushAttackRadius;
    public float rushAttackInterval;

    int samulCount;
    bool samulDead;
    int deadCount;

    const int patternCount = 3;
    public System.Func<IEnumerator>[] patternList = new System.Func<IEnumerator>[patternCount];
    int patternIndex = 0;

    void OnDestroy()
    {
        for (int i = 0; i < samulEntities.Count; i++)
        {
            int n = 0;
            switch (samulEntities[i].gameObject.name)
            {
                case "Buk":
                    n = 0;
                    break;
                case "Jing":
                    n = 1;
                    break;
                case "Janggu":
                    n = 2;
                    break;
                case "Kkwaenggwari":
                    n = 3;
                    break;
            }

            bgmPrefabs[n]?.GetComponent<AudioSource>().Stop();
            bgmPrefabs[n]?.gameObject.SetActive(false);
            bgmPrefabs[n] = null;
            bgmDead[n] = true;
        }
    }

    void ClearPattern()
    {
        patternList[0] = PatternPackA;
        patternList[1] = PatternPackB;
        patternList[2] = PatternPackC;

        void Swap(int a, int b)
        {
            var temp = patternList[a];
            patternList[a] = patternList[b];
            patternList[b] = temp;
        }

        for (int i = 0; i < patternList.Length - 1; i++)
        {
            Swap(i, Random.Range(i + 1, patternList.Length - 1));
        }
        for (int i = 0; i < patternList.Length - 1; i++)
        {
            Swap(i, Random.Range(i + 1, patternList.Length - 1));
        }
    }

    IEnumerator PatternA()
    {
        Debug.Log("-A");
        if (samulDead) yield break;
        yield return OneAttackRoutine();
        if (samulDead) yield break;
        yield return AllAttackRoutine();
        if (samulDead) yield break;
        yield return OneAttackRoutine();
        if (samulDead) yield break;
    }
    IEnumerator PatternB()
    {
        Debug.Log("-B");
        if (samulDead) yield break;
        yield return RushAttackRoutine();
        if (samulDead) yield break;
        yield return AllAttackRoutine();
        if (samulDead) yield break;
    }
    IEnumerator PatternC()
    {
        Debug.Log("-C");
        if (samulDead) yield break;
        yield return AllAttackRoutine();
        if (samulDead) yield break;
        yield return AllAttackRoutine();
        if (samulDead) yield break;
    }
    IEnumerator PatternD()
    {
        Debug.Log("-D");
        if (samulDead) yield break;
        yield return OneAttackRoutine();
        if (samulDead) yield break;
        yield return RushAttackRoutine();
        if (samulDead) yield break;
        yield return OneAttackRoutine();
        if (samulDead) yield break;
    }

    IEnumerator PatternPackA()
    {
        Debug.Log("A");
        yield return PatternD();
        yield return PatternC();
        yield return PatternA();
        yield return PatternB();
    }
    IEnumerator PatternPackB()
    {
        Debug.Log("B");
        yield return PatternD();
        yield return PatternC();
        yield return PatternD();
    }
    IEnumerator PatternPackC()
    {
        Debug.Log("C");
        yield return PatternB();
        yield return PatternA();
        yield return PatternD();
    }

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

                    int n = 0;
                    switch (samulEntities[i].gameObject.name)
                    {
                        case "Buk":
                            n = 0;
                            break;
                        case "Jing":
                            n = 1;
                            break;
                        case "Janggu":
                            n = 2;
                            break;
                        case "Kkwaenggwari":
                            n = 3;
                            break;
                    }

                    bgmPrefabs[n]?.GetComponent<AudioSource>().Stop();
                    bgmPrefabs[n]?.gameObject.SetActive(false);
                    bgmPrefabs[n] = null;
                    bgmDead[n] = true;
                }
            }

            for (int i = 0; i < removes.Count; i++)
            {
                samulEntities[removes[removes.Count - i - 1]].GetComponent<BoxCollider>().isTrigger = false;
                rotationAttack.DeActive(removes[removes.Count - i - 1]);
                samulEntities.RemoveAt(removes[removes.Count - i - 1]);
                samulEntityEvents.RemoveAt(removes[removes.Count - i - 1]);
                samulCount--;
                deadCount++;
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

    GameObject[] bgmPrefabs;

    bool[] bgmDead = new bool[4];

    IEnumerator PlayRoutine()
    {
        ClearPattern();
        yield return PositionRoutine();

        bgmPrefabs = new GameObject[4];
        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;

        for (int i = 0; i < 4; i++)
        {
            bgmPrefabs[i] = poolManager.GetObject("BgmPrefab");
            switch (i)
            {
                case 0:
                    bgmPrefabs[i].GetComponent<AudioSource>().clip = samulnoriSounds[0].Buk;
                    break;
                case 1:
                    bgmPrefabs[i].GetComponent<AudioSource>().clip = samulnoriSounds[0].Jing;
                    break;
                case 2:
                    bgmPrefabs[i].GetComponent<AudioSource>().clip = samulnoriSounds[0].Janggu;
                    break;
                case 3:
                    bgmPrefabs[i].GetComponent<AudioSource>().clip = samulnoriSounds[0].Kkwaenggwari;
                    break;
            }
            bgmPrefabs[i].GetComponent<AudioSource>().volume = 1;
            bgmPrefabs[i].GetComponent<AudioSource>().Play();
        }

        while (samulCount > 0)
        {
            if (samulDead)
            {
                samulDead = false;

                for (int i = 0; i < 4; i++)
                {
                    if (!bgmDead[i])
                    {
                        bgmPrefabs[i].GetComponent<AudioSource>().Stop();
                        bgmPrefabs[i].gameObject.SetActive(false);
                        bgmPrefabs[i] = null;
                    }
                }

                yield return AssembleRoutine();
                yield return new WaitForSeconds(1);

                for (int c = 0; c < deadCount; c++)
                {
                    for (int index = 0; index < samulCount; index++)
                    {
                        samulEntities[index].clone.SetMaxStat(StatCategory.Speed, (int)(samulEntities[index].clone.GetMaxStat(StatCategory.Speed) * 1.5f));
                        samulEntities[index].clone.SetStat(StatCategory.Speed, samulEntities[index].clone.GetMaxStat(StatCategory.Speed));
                    }
                }

                deadCount = 0;

                yield return PositionRoutine();


                for (int i = 0; i < 4; i++)
                {
                    if (bgmDead[i]) continue;
                    bgmPrefabs[i] = poolManager.GetObject("BgmPrefab");
                    switch (i)
                    {
                        case 0:
                            bgmPrefabs[i].GetComponent<AudioSource>().clip = samulnoriSounds[4 - samulCount].Buk;
                            break;
                        case 1:
                            bgmPrefabs[i].GetComponent<AudioSource>().clip = samulnoriSounds[4 - samulCount].Jing;
                            break;
                        case 2:
                            bgmPrefabs[i].GetComponent<AudioSource>().clip = samulnoriSounds[4 - samulCount].Janggu;
                            break;
                        case 3:
                            bgmPrefabs[i].GetComponent<AudioSource>().clip = samulnoriSounds[4 - samulCount].Kkwaenggwari;
                            break;
                    }
                    bgmPrefabs[i].GetComponent<AudioSource>().volume = 1;
                    bgmPrefabs[i].GetComponent<AudioSource>().Play();
                }
            }
            else
            {
                yield return RotationRoutine();
                if (samulDead) continue;

                yield return patternList[patternIndex]();
                patternIndex++;

                if (patternIndex == patternCount)
                {
                    patternIndex = 0;
                    ClearPattern();
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
                    if (!samulEntities[index].isDead) samulEntityEvents[index].CallEvent(EventCategory.Move, moveDirection.x, moveDirection.y, moveDirection.x > 0, new Vector3(positions[index].x, samulEntities[index].transform.position.y, positions[index].y));
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
                    if (!samulEntities[index].isDead) samulEntityEvents[index].CallEvent(EventCategory.Move, moveDirection.x, moveDirection.y, moveDirection.x > 0, new Vector3(positions[index].x, samulEntities[index].transform.position.y, positions[index].y));
                }
                else
                {
                    if (!samulEntities[index].isDead)
                    {
                        rotationAttack.Active(index);
                        samulEntities[index].GetProcessor(typeof(Processor.Animate)).AddCommand("Lock", new object[] { });
                        samulEntities[index].GetProcessor(typeof(Processor.Animate)).AddCommand("PlayNoLock", new object[] { "BossAttack" });
                    }
                    samulEntityEvents[index].CallEvent(EventCategory.Move, 0, 0, !samulEntities[index].GetComponent<SpriteRenderer>().flipX, samulEntities[index].transform.position);
                    standardAngle -= 360 / samulCount * Time.deltaTime;
                    if (standardAngle < 0) standardAngle += 360;

                    turnCount += 100 / samulCount * Time.deltaTime;
                }
            }

            if (turnCount >= 50f)
            {
                break;
            }

            yield return null;
        }
        for (int index = 0; index < samulCount; index++)
        {
            samulEntities[index].GetProcessor(typeof(Processor.Animate)).AddCommand("UnLock", new object[] { });
        }
        rotationAttack.DeActiveAll();
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
                    if (!samulEntities[index].isDead) samulEntityEvents[index].CallEvent(EventCategory.Move, moveDirection.x, moveDirection.y, moveDirection.x > 0, new Vector3(positions[index].x, samulEntities[index].transform.position.y, positions[index].y));
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
        int randomSamul = Mathf.Clamp(Random.Range(1, samulCount + 1), 1, 2);

        int[] randoms = new int[randomSamul];

        {
            int[] temp = new int[samulCount];
            for (int i = 0; i < samulCount; i++)
            {
                temp[i] = i;
            }

            for (int c = 0; c < 2; c++)
            {
                int t = 0;
                for (int i = 0; i < samulCount - 1; i++)
                {
                    int rand = Random.Range(i + 1, samulCount);
                    t = temp[rand];
                    temp[rand] = temp[i];
                    temp[i] = t;
                }
            }

            for (int i = 0; i < randomSamul; i++)
            {
                randoms[i] = temp[i];
            }
        }

        for (int i = 0; i < randomSamul; i++)
        {
            if (!samulEntities[randoms[i]].isDead) samulEntities[randoms[i]].GetComponent<StateMachine.StateMachine>().enabled = true;
        }

        Vector2[] positions = new Vector2[samulCount];

        standardAngle -= 360 / samulCount * Time.deltaTime;

        int arrivedCount = 0;
        float attackTime = Random.Range(10, 20);

        bool attacking = true;

        while (arrivedCount != samulCount && !samulDead)
        {
            arrivedCount = 0;

            attackTime -= Time.deltaTime;
            if (attackTime <= 0)
            {
                attacking = false;
                for (int i = 0; i < randomSamul; i++)
                {
                    if (!samulEntities[randoms[i]].isDead) samulEntities[randoms[i]].GetComponent<StateMachine.StateMachine>().enabled = false;
                }
            }

            for (int index = 0; index < samulCount; index++)
            {
                Vector3 pos3 = transform.position + Quaternion.AngleAxis(standardAngle + 360 * index / samulCount, Vector3.up) * (Vector3.forward * radius);
                positions[index] = new Vector2(pos3.x, pos3.z);
            }

            float epsilon = samulEntities[0].clone.GetStat(StatCategory.Speed) / 30f;
            for (int index = 0; index < samulCount; index++)
            {
                bool check = false;
                for (int i = 0; i < randomSamul; i++)
                {
                    if (randoms[i] == index) check = true;
                }
                if (attacking && check) continue;
                else
                {
                    float distance = Vector2.Distance(new Vector2(samulEntities[index].transform.position.x, samulEntities[index].transform.position.z), positions[index]);
                    if (distance > epsilon)
                    {
                        Vector2 moveDirection = positions[index] - new Vector2(samulEntities[index].transform.position.x, samulEntities[index].transform.position.z);
                        if (!samulEntities[index].isDead) samulEntityEvents[index].CallEvent(EventCategory.Move, moveDirection.x, moveDirection.y, moveDirection.x > 0, new Vector3(positions[index].x, samulEntities[index].transform.position.y, positions[index].y));
                        arrivedCount++;
                    }
                    else
                    {
                        check = false;
                        for (int i = 0; i < randomSamul; i++)
                        {
                            if (randoms[i] == index) check = true;
                        }
                        if (check) continue;
                        if (!samulEntities[index].isDead)
                        {
                            rotationAttack.Active(index);
                            samulEntities[index].GetProcessor(typeof(Processor.Animate)).AddCommand("Lock", new object[] { });
                            samulEntities[index].GetProcessor(typeof(Processor.Animate)).AddCommand("PlayNoLock", new object[] { "BossAttack" });
                        }

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
            samulEntities[index].GetProcessor(typeof(Processor.Animate)).AddCommand("UnLock", new object[] { });
        }
        rotationAttack.DeActiveAll();
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
                    if (!samulEntities[index].isDead) samulEntityEvents[index].CallEvent(EventCategory.Move, moveDirection.x, moveDirection.y, moveDirection.x > 0, new Vector3(positions[index].x, samulEntities[index].transform.position.y, positions[index].y));
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

        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;

        for (int index = 0; index < samulCount; index++)
        {
            samulEntities[index].clone.SetMaxStat(StatCategory.Speed, (int)(samulEntities[index].clone.GetMaxStat(StatCategory.Speed) * 2f));
            samulEntities[index].clone.SetStat(StatCategory.Speed, samulEntities[index].clone.GetMaxStat(StatCategory.Speed));
        }

        for (int index = 0; index < rushOrder.Length; index++)
        {
            rushOrder[index] = samulEntities[index];
        }

        for (int index = 0; index < rushOrder.Length; index++)
        {
            rushOrder[index].GetComponent<BoxCollider>().isTrigger = true;
        }

        int shuffleIndex = 3;
        for (int index = 0; index < rushOrder.Length - 2; index++)
        {
            Entity temp = rushOrder[index];
            shuffleIndex = Random.Range(index + 1, rushOrder.Length);
            rushOrder[index] = rushOrder[shuffleIndex];
            rushOrder[shuffleIndex] = temp;
        }

        int randomRushCount = Random.Range(6 - samulCount / 2, 6 - samulCount / 2 + 4 - samulCount);

        Vector3 startPointDirection = Vector3.zero;

        for (int rushStack = 0; rushStack < randomRushCount; rushStack++)
        {
            startPointDirection = (gameManager.Player.transform.position - transform.position).normalized;

            for (int index = 0; index < positions.Length; index++)
            {
                Vector3 pos3 = transform.position + startPointDirection * (rushAttackRadius + rushAttackInterval * index);
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
                            if (!samulEntities[index].isDead) samulEntityEvents[index].CallEvent(EventCategory.Move, moveDirection.x, moveDirection.y, moveDirection.x > 0, new Vector3(positions[index].x, samulEntities[index].transform.position.y, positions[index].y));
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
                    if (rushOrder[index].isDead) continue;
                    rushOrder[index].transform.position = new Vector3(positions[index].x, rushOrder[index].transform.position.y, positions[index].y);
                }
            }

            if (samulCount == 0) break;

            for (int index = 0; index < positions.Length; index++)
            {
                Vector3 pos3 = transform.position - startPointDirection * (rushAttackRadius + rushAttackInterval * (positions.Length - 1 - index));
                positions[index] = new Vector2(pos3.x, pos3.z);
            }

            samulStack = 0;
            while (samulStack < samulCount)
            {
                samulStack = 0;
                float epsilon = samulEntities[0].clone.GetStat(StatCategory.Speed) / 30f;
                rotationAttack.DeActiveAll();
                for (int index = 0; index < samulCount; index++)
                {
                    if (!samulEntities[index].isDead)
                    {
                        rotationAttack.Active(index);
                        samulEntities[index].GetProcessor(typeof(Processor.Animate)).AddCommand("Lock", new object[] { });
                        samulEntities[index].GetProcessor(typeof(Processor.Animate)).AddCommand("PlayNoLock", new object[] { "BossAttack" });
                    }
                    if (Vector2.Distance(new Vector2(samulEntities[index].transform.position.x, samulEntities[index].transform.position.z), positions[index]) > epsilon)
                    {
                        Vector2 moveDirection = positions[index] - new Vector2(samulEntities[index].transform.position.x, samulEntities[index].transform.position.z);
                        if (!samulEntities[index].isDead) samulEntityEvents[index].CallEvent(EventCategory.Move, moveDirection.x, moveDirection.y, moveDirection.x > 0, new Vector3(positions[index].x, samulEntities[index].transform.position.y, positions[index].y));
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
            samulEntities[index].GetProcessor(typeof(Processor.Animate)).AddCommand("UnLock", new object[] { });
            rotationAttack.DeActiveAll();
        }

        for (int index = 0; index < rushOrder.Length; index++)
        {
            rushOrder[index].GetComponent<BoxCollider>().isTrigger = false;
        }
    }

    void Subjugation()
    {
        Debug.Log("공략 ㅊㅊ");
    }
}

[System.Serializable]
public struct SamulnoriSound
{
    public AudioClip Buk;
    public AudioClip Jing;
    public AudioClip Janggu;
    public AudioClip Kkwaenggwari;
}