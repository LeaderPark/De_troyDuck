using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public SkillData skillData {get; set;}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null)
        {
            Entity entity = other.GetComponent<Entity>();
            if (skillData.entity.gameObject.layer != entity?.gameObject.layer)
            {
                skillData.skillEffect?.ShowSkillEffect(skillData.entity, entity);
                entity?.GetProcessor(typeof(Processor.HitBody))?.AddCommand("DamageOnBody", new object[]{skillData.CalculateSkillDamage(), skillData.entity});
            }
        }
    }
}
