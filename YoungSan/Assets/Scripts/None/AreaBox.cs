using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBox : MonoBehaviour
{
    public SkillAreaData skillAreaData {get; set;}
    private Player player;

    public AreaDirection areaDirection {get; set;}
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                player = other.gameObject.GetComponent<Player>();
                if (player != null)
                {
                    if (areaDirection == AreaDirection.Left)
                    {
                        skillAreaData.inLeftSkillArea = true;
                    }
                    else
                    {
                        skillAreaData.inRightSkillArea = true;
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject != null)
        {
            if (player == other.gameObject.GetComponent<Player>())
            {
                if (areaDirection == AreaDirection.Left)
                {
                    skillAreaData.inLeftSkillArea = false;
                }
                else
                {
                    skillAreaData.inRightSkillArea = false;
                }
            }
        }
    }
}

public enum AreaDirection
{
    Left,
    Right
}
