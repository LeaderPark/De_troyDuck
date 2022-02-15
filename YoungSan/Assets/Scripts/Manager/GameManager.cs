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

    public (bool, float) GetGroundY(Vector2 xz)
    {
        RaycastHit hit;
        if (Physics.Raycast(new Ray(new Vector3(xz.x, 100, xz.y), Vector3.down), out hit, 2000, LayerMask.GetMask(new string[] { "Ground" })))
		{
            return (true, hit.point.y);
		}
        return (false, 0);
    }
}
