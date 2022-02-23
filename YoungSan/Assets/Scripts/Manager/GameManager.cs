using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Manager
{
    private Player player;
    public Player Player
    {
        get
        {
            if (player == null)
            {
                Player = GameObject.FindWithTag("Player").GetComponent<Player>();
            }

            return player;
        }
        set
        {
            if (player != null)
            {
                player.transform.parent = Camera.main.transform;
                player.transform.parent = null;
            }
            player = value;
            player.transform.SetParent(transform);
            Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Follow = player.transform;
        }
    }
}
