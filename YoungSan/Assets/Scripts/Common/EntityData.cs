using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityData", menuName = "ScriptableObjects/EntityData", order = 1)]
public class EntityData : ScriptableObject
{
    [Space(40)]
    public string entityName;
    [Space(10)]
    public Sprite[] skillIcon;
    [Space(10)]
    public SkillContent[] skillContents;
    [Space(10)]
    public Status status;
    [Space(10)]
    public Status playerStatus;
    [Space(10)]
    public GameObject prefab;
    [Space(10)]
    public float uiPos;


}

[System.Serializable]
public struct SkillContent
{
    [Multiline]
    public string text;
}