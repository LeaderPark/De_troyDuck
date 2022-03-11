using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerObject : MonoBehaviour
{
    private static ManagerObject instance;
    public static ManagerObject Instance 
    {
        get
        {
            if (instance == null)
            {
                Instantiate(Resources.Load<GameObject>("ManagerObject"));
            }
            return instance;
        }
    }


    Hashtable ManagerTable {get; set;}


    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        ManagerTable = new Hashtable();

        LoadManagers();
    }

    private void LoadManagers()
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Manager");
        foreach (GameObject item in prefabs)
        {
            ManagerTable.Add((ManagerType)System.Enum.Parse(typeof(ManagerType), item.name), GameObject.Instantiate(item, transform).GetComponent<Manager>());
        }
    }

    public Manager GetManager(ManagerType type)
    {
        if (ManagerTable.ContainsKey(type))
        {
            return ManagerTable[type] as Manager;
        }
        return null;
    }

    public void SetTimeScale(float timeScale, float time)
    {
        StartCoroutine(ControlTimeScale(timeScale, time));
    }

    IEnumerator ControlTimeScale(float timeScale, float time)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1f;
    }

}
