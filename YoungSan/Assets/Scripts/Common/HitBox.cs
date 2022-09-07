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
                skillData.skillEffect?.ShowSkillEffect(skillData.skillSet.entity, null, skillData.direction, skillData.targetIndex);
            }
            if (entity == null) return;
            if (skillData == null) return;
            if (skillData.skillSet.entity.gameObject.tag != entity.gameObject.tag)
            {
                if (entity.isDead) return;
                if (!entity.hitable) return;
                if (!entity.entityStatusAilment) return;

                if (targets.Contains(entity)) return;
                targets.Add(entity);

                Processor.HitBody hitBody = entity?.GetProcessor(typeof(Processor.HitBody)) as Processor.HitBody;

                EntityStatus blocking = entity.entityStatusAilment.GetEntityStatus(typeof(Blocking));

                if (!blocking.Activated())
                {
                    switch (entity.gameObject.tag)
                    {
                        case "Player": // player
                            DamageEffect.Instance?.OnDamageEffect();

                            break;
                        case "Enemy": // enemy

                            break;
                        case "Boss": // enemy

                            break;
                    }

                    EntityStatus superArmour = entity.entityStatusAilment.GetEntityStatus(typeof(SuperArmour));
                    if (!superArmour.Activated())
                    {
                        entity?.GetProcessor(typeof(Processor.Skill))?.AddCommand("StopSkill", new object[] { });
                    }
                    CameraShake.Instance.Shake();
                    hitBody?.AddCommand("DamageOnBody", new object[] { skillData.CalculateSkillDamage(), skillData.skillSet.entity });

                    if (!superArmour.Activated())
                    {
                        skillData.skillEffect?.ShowSkillEffect(skillData.skillSet.entity, entity, skillData.direction, skillData.targetIndex);
                    }
                }
            }
        }
    }
}
