using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperArmour : EntityStatus
{
    SuperArmourEffect superArmourEffect;

    Entity entity;

    public SuperArmour()
    {
    }

    public void SetData(Entity entity)
    {
        this.entity = entity;
    }

    public override void Activate()
    {
        base.Activate();
        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
        superArmourEffect = poolManager.GetObject("SuperArmourEffect").GetComponent<SuperArmourEffect>();
        superArmourEffect.SetData(entity);
        superArmourEffect.GetComponent<SpriteRenderer>().enabled = true;
    }

    public override void DeActivate()
    {
        base.DeActivate();
        superArmourEffect.GetComponent<SpriteRenderer>().enabled = false;
        superArmourEffect.gameObject.SetActive(false);
    }
}

