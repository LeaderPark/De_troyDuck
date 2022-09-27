using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingJing : Installation
{
    public Rigidbody rigidbody;

    bool flying;

    public override void Play()
    {
        transform.position = ownerEntity.transform.position + Vector3.up * 0.8f;

        flying = true;

        StartCoroutine(FlyingRoutine());
    }

    IEnumerator FlyingRoutine()
    {
        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
        while (flying)
        {
            rigidbody.velocity = (position - transform.position).normalized * ownerEntity.clone.GetStat(StatCategory.Speed) * 4;
            yield return null;

            if (Vector3.Distance(position, transform.position) < ownerEntity.clone.GetStat(StatCategory.Speed) * 4 / 30f) flying = false;
        }

        rigidbody.velocity = Vector3.zero;

        SoundWave soundWave = poolManager.GetObject("SoundWave").GetComponent<SoundWave>();

        soundWave.transform.position = transform.position;

        soundWave.SetData(null, transform.position, skillData);

        yield return new WaitForSeconds(2f);

        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject != null)
        {
            if (other.gameObject.layer == 9)
            {
                flying = false;
            }
        }
    }
}
