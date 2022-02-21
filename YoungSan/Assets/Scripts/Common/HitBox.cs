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
            if (skillData.entity.gameObject.tag != entity?.gameObject.tag)
            {
                if (entity == null) return;
                if (entity.isDead) return;
                
                Debug.Log("Hit " + Time.time);
                switch (entity.gameObject.tag)
                {
                    case "Player": // player
                    CameraShake.Instance.Shake();
                    entity?.GetProcessor(typeof(Processor.Skill))?.AddCommand("StopSkill", new object[]{});
                    entity?.GetProcessor(typeof(Processor.HitBody))?.AddCommand("DamageOnBody", new object[]{skillData.CalculateSkillDamage(), skillData.entity});
                    DamageEffect.Instance?.OnDamageEffect();
                    skillData.skillEffect?.ShowSkillEffect(skillData.entity, entity, skillData.direction);
                    break;
                    case "Enemy": // enemy
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
