using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    [SerializeField] private AnimationClip[] attackAnim = new AnimationClip[0];
    HashSet<AnimationClip> anims = new HashSet<AnimationClip>();

    private void Awake()
    {
        foreach (var item in attackAnim)
        {
            anims.Add(item);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        HitBox box = collision.gameObject.GetComponent<HitBox>();

        if (box != null)
        {
            if (anims.Count == 0 || (anims.Contains(box.skillData.skill) && box.skillData.skillSet.entity.CompareTag("Player")))
            {
                Vector2 boxV = box.skillData.direction;
                Vector2 obj = new Vector2(transform.forward.x, transform.forward.z);
                Break objBreak = GetComponent<Break>();
                objBreak.minAngle += boxV.x < 0 ? 360 - Vector2.Angle(obj, boxV) : Vector2.Angle(obj, boxV);
                objBreak.maxAngle += boxV.x < 0 ? 360 - Vector2.Angle(obj, boxV) : Vector2.Angle(obj, boxV);
                objBreak.minAngle %= 360;
                objBreak.maxAngle %= 360;
                objBreak?.Play();
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}
