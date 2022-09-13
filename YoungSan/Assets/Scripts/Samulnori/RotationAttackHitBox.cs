using System.Collections;
using System.Collections.Generic;
using NCalc;
using UnityEngine;

public class RotationAttackHitBox : MonoBehaviour
{
    bool cantAttack;
    float time;

    void Update()
    {
        time -= Time.deltaTime;

        if (time <= 0)
        {
            cantAttack = false;
            GetComponent<HitBox>().ClearTargetSet();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (cantAttack) return;
        if (other.gameObject != null)
        {
            Entity entity = other.GetComponent<Entity>();
            if (entity == null) return;
            SkillData skillData = GetComponent<HitBox>().skillData;
            if (skillData == null) return;
            if (skillData.skillSet.entity.gameObject.tag != entity.gameObject.tag)
            {
                if (entity.isDead) return;
                if (!entity.hitable) return;
                cantAttack = true;
                time = 0.5f;
            }
        }
    }
}
