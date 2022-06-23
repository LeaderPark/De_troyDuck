using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBox : MonoBehaviour
{
    public SkillAreaData skillAreaData { get; set; }

    public AreaDirection areaDirection { get; set; }

    Player player;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null)
        {
            GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
            if (other.gameObject.GetComponent<Player>() == gameManager.Player)
            {
                player = gameManager.Player;
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

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject != null)
        {
            GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
            if (other.gameObject.GetComponent<Player>() == gameManager.Player || player == other.gameObject.GetComponent<Player>())
            {
                player = null;
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
