using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone
{
    public string Name {get; private set;}
    public Hashtable StatTable {get; private set;}


    public int GetStat(StatCategory category)
    {
        if (StatTable.ContainsKey(category))
        {
            return (int)StatTable[category];
        }

        return 0;
    }

    public Clone(EntityData data)
    {
        Name = data.entityName;
        StatTable = new Hashtable();
        
        for (int i = 0; i < data.status.stats.Count; i++)
        {
            Stat temp = data.status.stats[i];

            StatTable.Add(temp.category, Random.Range(temp.minValue, temp.maxValue + 1));
        }

        Debug.Log(string.Format("Name : {0}, HP : {1}, Atk : {2}, Speed : {3}, Stamina : {4}", Name, GetStat(StatCategory.Health), GetStat(StatCategory.Attack), GetStat(StatCategory.Speed), GetStat(StatCategory.Stamina)));
    }
}
