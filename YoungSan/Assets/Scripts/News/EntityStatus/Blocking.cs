using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocking : EntityStatus
{
    BlockingEffect blockingEffect;

    Entity entity;

    public Blocking()
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
        blockingEffect = poolManager.GetObject("BlockingEffect").GetComponent<BlockingEffect>();
        blockingEffect.SetData(entity);
        blockingEffect.GetComponent<MeshRenderer>().enabled = true;
    }

    public override void DeActivate()
    {
        base.DeActivate();
        blockingEffect.GetComponent<MeshRenderer>().enabled = false;
        blockingEffect.gameObject.SetActive(false);
    }
}
