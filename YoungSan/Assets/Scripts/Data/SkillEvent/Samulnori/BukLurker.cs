using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BukLurker : MonoBehaviour
{
    public AnimationClip lurker;
    public Animator animator;

    public void SetData(Vector3 position, SkillData skillData)
    {
        transform.position = position;
        GetComponent<HitBox>().skillData = skillData;
        GetComponent<HitBox>().ClearTargetSet();
        animator.Play("BukLurker");
        StartCoroutine(DissolveRoutine());
    }

    IEnumerator DissolveRoutine()
    {
        yield return new WaitForSeconds(lurker.length);

        gameObject.SetActive(false);
    }

    void OnTriggerStay(Collider other)
    {

        if (other.gameObject != null)
        {
            if (other.gameObject.layer == 9)
            {
                gameObject.SetActive(false);
            }
            Entity entity = other.GetComponent<Entity>();
            if (entity == null) return;
            SkillData skillData = GetComponent<HitBox>().skillData;
            if (skillData == null) return;
            if (skillData.skillSet.entity.gameObject.tag != entity.gameObject.tag)
            {
                if (entity.isDead) return;
                if (!entity.hitable) return;
            }
        }
    }
}
