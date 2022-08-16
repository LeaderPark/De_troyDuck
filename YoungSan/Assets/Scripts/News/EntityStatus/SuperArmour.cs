using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperArmour : EntityStatus
{
    SuperArmourEffect superArmourEffect;

    public SuperArmour()
    {
        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
        superArmourEffect = poolManager.GetObject("SuperArmourEffect").GetComponent<SuperArmourEffect>();
        superArmourEffect.enabled = false;
    }

    public void SetData(Entity entity)
    {
        superArmourEffect.SetData(entity);
    }

    public override void Activate()
    {
        base.Activate();
        superArmourEffect.enabled = true;
        superArmourEffect.GetComponent<SpriteRenderer>().enabled = true;
    }

    public override void DeActivate()
    {
        base.DeActivate();
        superArmourEffect.enabled = false;
        superArmourEffect.GetComponent<SpriteRenderer>().enabled = false;
    }
}

