using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone
{
    public string Name {get; private set;}

    private Hashtable StatTable {get; set;}
    private Hashtable MaxStatTable {get; set;}

    private Entity entity;

    public int GetStat(StatCategory category)
    {
        if (StatTable.ContainsKey(category))
        {
            return (int)StatTable[category];
        }

        return 0;
    }

    public int GetMaxStat(StatCategory category)
    {
        if (MaxStatTable.ContainsKey(category))
        {
            return (int)MaxStatTable[category];
        }

        return 0;
    }

    public void AddStat(StatCategory category, int value)
    {
        if (StatTable.ContainsKey(category))
        {
            StatTable[category] = (int)Mathf.Clamp((int)StatTable[category] + value, 0, (int)MaxStatTable[category]);

            UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
            uiManager.statbar.UpdateStatBar();
            uiManager.statbar.UpdateStatText();
        }
    }

    public void SubStat(StatCategory category, int value)
    {
        if (StatTable.ContainsKey(category))
        {
            int temp = (int)StatTable[category] - value;
            StatTable[category] = (int)Mathf.Clamp(temp, 0, (int)MaxStatTable[category]);

            UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
            uiManager.statbar.UpdateStatBar();
            uiManager.statbar.UpdateStatText();

            if(category == StatCategory.Health)
            uiManager.EnemyHpBarUpdate(entity);

        }
    }

    public void SetMaxStat(StatCategory category, int value)
    {
        if (MaxStatTable.ContainsKey(category))
        {
            MaxStatTable[category] = value;
            SetStat(category, GetStat(category));

            UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
            uiManager.statbar.SetStatBar();
            uiManager.statbar.UpdateStatText();
        }
    }


    public void SetStat(StatCategory category, int value)
    {
        if (StatTable.ContainsKey(category))
        {
            StatTable[category] = (int)Mathf.Clamp(value, 0, (int)MaxStatTable[category]);

            UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
            uiManager.statbar.SetStatBar();
            uiManager.statbar.UpdateStatText();
        }
    }

    public Clone(Entity entity, EntityData data)
    {
        Name = data.entityName;
        this.entity = entity;
        MaxStatTable = new Hashtable();
        
        for (int i = 0; i < data.status.stats.Count; i++)
        {
            Stat temp = data.status.stats[i];

            MaxStatTable.Add(temp.category, Random.Range(temp.minValue, temp.maxValue + 1));
        }


        StatTable = new Hashtable(MaxStatTable);
        Debug.Log(string.Format("Name : {0}, HP : {1}, Atk : {2}, Speed : {3}, Stamina : {4}", Name, GetStat(StatCategory.Health), GetStat(StatCategory.Attack), GetStat(StatCategory.Speed), GetStat(StatCategory.Stamina)));
    }
}
