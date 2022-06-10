using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public SkillData skillData { get; set; }

    HashSet<Entity> targets;

    bool wall;

    void Awake()
    {
        targets = new HashSet<Entity>();
    }

    public void ClearTargetSet()
    {
        if (targets?.Count > 0) targets?.Clear();
        wall = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null)
        {
            Entity entity = other.GetComponent<Entity>();
            if (!wall && other.gameObject.layer == 9)
            {
                wall = true;
                skillData.skillSet.entity?.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocity", new object[] { Vector3.zero, 0 });
                skillData.skillSet.entity?.GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[] { "Idle", true });
                skillData.skillSet.entity?.GetProcessor(typeof(Processor.Skill))?.AddCommand("StopSkill", new object[] { });
                skillData.skillEffect?.ShowSkillEffect(skillData.skillSet.entity, null, skillData.direction, skillData.targetIndex);
            }
            if (entity == null) return;
            if (skillData == null) return;
            if (skillData.skillSet.entity.gameObject.tag != entity.gameObject.tag)
            {
                if (entity.isDead) return;
                if (!entity.hitable) return;

                if (targets.Contains(entity)) return;
                targets.Add(entity);

                Processor.HitBody hitBody = entity?.GetProcessor(typeof(Processor.HitBody)) as Processor.HitBody;

                switch (entity.gameObject.tag)
                {
                    case "Player": // player
                        CameraShake.Instance.Shake();
                        entity?.GetProcessor(typeof(Processor.Skill))?.AddCommand("StopSkill", new object[] { });
                        hitBody?.AddCommand("DamageOnBody", new object[] { skillData.CalculateSkillDamage(), skillData.skillSet.entity });
                        DamageEffect.Instance?.OnDamageEffect();

                        if (!hitBody.isDefencing)
                        {
                            skillData.skillEffect?.ShowSkillEffect(skillData.skillSet.entity, entity, skillData.direction, skillData.targetIndex);
                        }

                        break;
                    case "Enemy": // enemy
                        CameraShake.Instance.Shake();
                        entity?.GetProcessor(typeof(Processor.Skill))?.AddCommand("StopSkill", new object[] { });
                        hitBody?.AddCommand("DamageOnBody", new object[] { skillData.CalculateSkillDamage(), skillData.skillSet.entity });

                        if (!hitBody.isDefencing)
                        {
                            skillData.skillEffect?.ShowSkillEffect(skillData.skillSet.entity, entity, skillData.direction, skillData.targetIndex);
                        }
                        break;
                    case "Boss": // enemy
                        CameraShake.Instance.Shake();
                        hitBody?.AddCommand("DamageOnBody", new object[] { skillData.CalculateSkillDamage(), skillData.skillSet.entity });

                        if (!hitBody.isDefencing)
                        {
                            skillData.skillEffect?.ShowSkillEffect(skillData.skillSet.entity, entity, skillData.direction, skillData.targetIndex);
                        }
                        break;
                }
            }
        }
    }
}
