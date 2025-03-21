using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poisoning : EntityStatus
{
    private TickDamage tickDamage;

    private Entity sourceEntity;
    private Entity targetEntity;
    private float delay;
    private float time;
    private string tickDamageForm;

    public Poisoning()
    {
    }

    public void SetData(Entity sourceEntity, Entity targetEntity, float delay, float time, string tickDamageForm)
    {
        this.sourceEntity = sourceEntity;
        this.targetEntity = targetEntity;
        this.delay = delay;
        this.time = time;
        this.tickDamageForm = tickDamageForm;
    }

    public override void Activate()
    {
        base.Activate();

        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
        tickDamage = poolManager.GetObject("Poisoning").GetComponent<TickDamage>();
        tickDamage.SetData(sourceEntity, targetEntity, delay, time, tickDamageForm);
    }

    public override void DeActivate()
    {
        base.DeActivate();
        tickDamage.GetComponent<UnityEngine.VFX.VisualEffect>().enabled = false;
        tickDamage.gameObject.SetActive(false);
    }
}
