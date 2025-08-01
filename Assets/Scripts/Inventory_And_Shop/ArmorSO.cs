using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Item", menuName = "ItemsSO/New Armor Item")]

public class ArmorSO : ItemAbs_SO
{
    [SerializeField]
    private itemType type;

    [SerializeField]
    private List<StatGen<int>> integerStats = new List<StatGen<int>>
    {
        new StatGen<int> { StatType = statType.Attack, Stat = 0 },
        new StatGen<int> { StatType = statType.Speed, Stat = 0 },
        new StatGen<int> { StatType = statType.MaxHealth, Stat = 0 }

    };

    public override itemType GetItemType()
    {
        return type;
    }

    //Adds all the values that are diffrent from 0
    public override List<IStat> GetItemStats()
    {
        List<IStat> statsList = new List<IStat>();

        foreach (IStat item in integerStats)
        {
            if (item.TryParseToInt(out int intValue) && intValue > 0)
            {
                statsList.Add(item);
            }
        }
        // Get all instance, non-public, and public fields
        var fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        foreach (var field in fields)
        {
            if (typeof(IStat).IsAssignableFrom(field.FieldType))
            {
                var value = field.GetValue(this) as IStat;
                if (value == null) continue;

                if (value.TryParseToInt(out int intValue) && intValue > 0)
                {
                    statsList.Add(value);
                }
                else if (value.TryParseToFloat(out float floatValue) && floatValue > 0f)
                {
                    statsList.Add(value);
                }
            }
        }

        return statsList;
    }
    public override bool IsStackable()
    {
        return false;
    }

}

