using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainKingEvent : EntityEvent
{
    protected override void Awake()
    {
        base.Awake();
        DefalutAttack();
        Skill1();
        Skill2();
    }
    private void DefalutAttack()
    {
        maxAttackStack[EventCategory.DefaultAttack] = 1;
        attackProcess[EventCategory.DefaultAttack] = new AttackProcess[]{
        (inputX, inputY, position, skillData) =>
        {
            if (skillData.skillSet.entity.gameObject.CompareTag("Boss")) SuperArmour(0f, skillData.skill.length);
        }
        };
    }

    private void Skill1()
    {
        maxAttackStack[EventCategory.Skill1] = 1;
        attackProcess[EventCategory.Skill1] = new AttackProcess[]{
        (inputX, inputY, position, skillData) =>
        {
            if (skillData.skillSet.entity.gameObject.CompareTag("Boss")) SuperArmour(0f, skillData.skill.length);
            Dash(inputX, inputY, entity.clone.GetStat(StatCategory.Speed) * 16, 1f, 0.15f);

            int idx = coroutines.Count;
            Coroutine routine = this.StartCoroutine(SonicRoutine(idx, skillData.skillSet.entity, new Vector2(inputX, inputY).normalized, 1f));
            coroutines.Add((false, routine));
        }
        };
    }

    IEnumerator SonicRoutine(int idx, Entity entity, Vector2 direction, float startTime)
    {
        yield return new WaitForSeconds(startTime);
        coroutines[idx] = (true, coroutines[idx].Item2);

        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
        poolManager.GetObject("MountainKing_Sonic").GetComponent<MountainKing_Sonic>().Play(entity, direction);
    }

    private void Skill2()
    {
        maxAttackStack[EventCategory.Skill2] = 1;
        attackProcess[EventCategory.Skill2] = new AttackProcess[]{
        (inputX, inputY, position, skillData) =>
        {
            if (skillData.skillSet.entity.gameObject.CompareTag("Boss")) SuperArmour(0f, skillData.skill.length);
        }
        };
    }
}
