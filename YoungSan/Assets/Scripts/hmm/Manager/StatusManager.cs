using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{

    private static StatusManager instance;
    public static StatusManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("StatusManager");
                instance = obj.AddComponent<StatusManager>();
            }
            
            return instance;
        }
    }

    private Hashtable statusTable;


    public Hashtable StatusTable {get; set;}
}
