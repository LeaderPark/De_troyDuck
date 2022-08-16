using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocking : EntityStatus
{
    BlockingEffect blockingEffect;

    public Blocking()
    {
        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
        blockingEffect = poolManager.GetObject("BlockingEffect").GetComponent<BlockingEffect>();
        blockingEffect.enabled = false;
    }

    public void SetData(Entity entity)
    {
        blockingEffect.SetData(entity);
    }

    public override void Activate()
    {
        base.Activate();
        blockingEffect.enabled = true;
        blockingEffect.GetComponent<MeshRenderer>().enabled = true;
    }

    public override void DeActivate()
    {
        base.DeActivate();
        blockingEffect.enabled = false;
        blockingEffect.GetComponent<MeshRenderer>().enabled = false;
    }
}
