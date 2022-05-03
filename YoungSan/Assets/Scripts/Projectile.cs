using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Vector3 startPosition;
    Vector3 dirVec;


    public float speed;
    public AnimationCurve curve;
    float timeStack;



    public void SetData(Vector3 startPosition, Vector3 dirVec, SkillData skillData)
    {
        timeStack = 0;
        this.startPosition = startPosition;
        this.dirVec = dirVec;

        GetComponent<HitBox>().skillData = skillData;
        if (dirVec.z > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, Vector3.Angle(Vector3.right, dirVec));
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, -Vector3.Angle(Vector3.right, dirVec));
        }
        transform.position = startPosition + dirVec * speed * timeStack + Vector3.up * curve.Evaluate(timeStack);
    }

    void Update()
    {
        timeStack += Time.deltaTime;
        if (curve.Evaluate(timeStack) == -1) gameObject.SetActive(false);
        transform.position = startPosition + dirVec * speed * timeStack + Vector3.up * curve.Evaluate(timeStack);
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject != null)
        {
            Entity entity = other.GetComponent<Entity>();
            if (entity == null) return;
            SkillData skillData = GetComponent<HitBox>().skillData;
            if (skillData == null) return;
            if (skillData.skillSet.entity.gameObject.tag != entity.gameObject.tag)
            {
                if (entity.isDead) return;
                if (!entity.hitable) return;
                gameObject.SetActive(false);
            }
        }
    }

}
