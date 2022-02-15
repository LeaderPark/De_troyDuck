using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager
{
    private Player player;
    public Player Player
    {
        get
        {
            if (player == null)
            {
                player = FindObjectOfType<Player>();
            }

            return player;
        }
        set
        {
            player = value;
        }
    }
}
