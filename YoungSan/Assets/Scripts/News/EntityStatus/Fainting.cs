using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fainting : EntityStatus
{
    private Entity entity;

    public void SetData(Entity entity)
    {
        this.entity = entity;
    }

    public override void Activate()
    {
        base.Activate();

        if (entity.gameObject.CompareTag("Boss")) return;
        entity?.GetProcessor(typeof(Processor.Move))?.AddCommand("Lock", new object[] { });
        entity?.GetProcessor(typeof(Processor.Animate))?.AddCommand("Lock", new object[] { });
        entity?.GetProcessor(typeof(Processor.Skill))?.AddCommand("Lock", new object[] { });
    }

    public override void DeActivate()
    {
        base.DeActivate();

        if (entity.gameObject.CompareTag("Boss")) return;
        entity?.GetProcessor(typeof(Processor.Move))?.AddCommand("UnLock", new object[] { });
        entity?.GetProcessor(typeof(Processor.Animate))?.AddCommand("UnLock", new object[] { });
        entity?.GetProcessor(typeof(Processor.Skill))?.AddCommand("UnLock", new object[] { });
    }
}
