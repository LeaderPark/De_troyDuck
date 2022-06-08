using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Installation : MonoBehaviour
{
    protected Entity ownerEntity;
    protected Vector3 position;
    protected SkillData skillData;


    public void SetData(Entity ownerEntity, Vector3 position, SkillData skillData)
    {
        this.ownerEntity = ownerEntity;
        this.position = position;
        this.skillData = skillData;

        Play();
    }

    public virtual void Play()
    {

    }
}
