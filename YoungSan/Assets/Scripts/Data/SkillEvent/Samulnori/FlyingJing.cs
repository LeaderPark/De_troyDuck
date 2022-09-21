using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingJing : Installation
{
    public HitBox hitBox;
    public Rigidbody rigidbody;

    bool flying;

    public override void Play()
    {
        transform.position = ownerEntity.transform.position + Vector3.up * 0.8f;

        hitBox.skillData = skillData;
        hitBox.ClearTargetSet();

        flying = true;

        StartCoroutine(FlyingRoutine());
    }

    IEnumerator FlyingRoutine()
    {
        while (flying)
        {
            rigidbody.velocity = (position - transform.position).normalized * ownerEntity.clone.GetStat(StatCategory.Speed) * 4;
            yield return null;

            if (Vector3.Distance(position, transform.position) < ownerEntity.clone.GetStat(StatCategory.Speed) * 4 / 30f) flying = false;
        }

        rigidbody.velocity = Vector3.zero;

        hitBox.GetComponent<BoxCollider>().enabled = true;

        yield return new WaitForSeconds(1);

        hitBox.GetComponent<BoxCollider>().enabled = false;
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
