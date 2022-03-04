using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Manager
{
    Dictionary<string, List<GameObject>> PoolObjects;
    Hashtable PoolObjectTable {get; set;}
    private GameObject canvas;

    void Awake()
    {
        PoolObjects = new Dictionary<string, List<GameObject>>();
        PoolObjectTable = new Hashtable();
        canvas = transform.Find("Canvas").gameObject;
        LoadPoolObjects();
    }

    private void LoadPoolObjects()
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("PoolObject");
        foreach (GameObject item in prefabs)
        {
            PoolObjects[item.name] = new List<GameObject>();
            PoolObjectTable.Add(item.name, item);
        }
    }

    public GameObject GetObject(string name)
    {
        if (PoolObjects.ContainsKey(name))
        {
            foreach (GameObject item in PoolObjects[name])
            {
                if (!item.activeSelf)
                {
                    item.SetActive(true);
                    return item;
                }
            }
            if (PoolObjectTable.ContainsKey(name))
            {
                PoolObjects[name].Add(GameObject.Instantiate((GameObject)PoolObjectTable[name], transform));
                return PoolObjects[name][PoolObjects[name].Count - 1];
            }
        }
        return null;
    }

    public GameObject GetUIObject(string name)
    {
        if (PoolObjects.ContainsKey(name))
        {
            foreach (GameObject item in PoolObjects[name])
            {
                if (!item.activeSelf)
                {
                    item.SetActive(true);
                    return item;
                }
            }
            if (PoolObjectTable.ContainsKey(name))
            {
                PoolObjects[name].Add(GameObject.Instantiate((GameObject)PoolObjectTable[name], canvas.transform));
                return PoolObjects[name][PoolObjects[name].Count - 1];
            }
        }
        return null;
    }
}
