using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillEffect : MonoBehaviour
{

    public abstract void ShowSkillEffect(Entity attackEntity, Entity hitEntity, Vector2 direction);
}
