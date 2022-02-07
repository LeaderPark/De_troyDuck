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
                if (entity == null)
                switch (entity.gameObject.layer)
                {
                    case 6: // player
                    CameraShake.Instance.Shake();
                    entity?.GetProcessor(typeof(Processor.Skill))?.AddCommand("StopSkill", new object[]{});
                    entity?.GetProcessor(typeof(Processor.HitBody))?.AddCommand("DamageOnBody", new object[]{skillData.CalculateSkillDamage(), skillData.entity});
                    skillData.skillEffect?.ShowSkillEffect(skillData.entity, entity, skillData.direction);
                    break;
                    case 7: // enemy
                    CameraShake.Instance.Shake();
                    entity?.GetProcessor(typeof(Processor.Skill))?.AddCommand("StopSkill", new object[]{});
                    entity?.GetProcessor(typeof(Processor.HitBody))?.AddCommand("DamageOnBody", new object[]{skillData.CalculateSkillDamage(), skillData.entity});
                    skillData.skillEffect?.ShowSkillEffect(skillData.entity, entity, skillData.direction);
                    break;
                }
            }
        }
    }
}
