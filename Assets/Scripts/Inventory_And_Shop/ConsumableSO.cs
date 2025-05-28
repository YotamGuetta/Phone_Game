using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "ItemsSO/New Consumable Item")]
public class ConsumableSO: ItemAbs_SO
{
    [SerializeField]
    private List<StatGen<int>> integerStats = new List<StatGen<int>>
    {
        new StatGen<int> { StatType = statType.Attack, Stat = 0 },
        new StatGen<int> { StatType = statType.Speed, Stat = 0 },
        new StatGen<int> { StatType = statType.MaxHealth, Stat = 0 },
        new StatGen<int> { StatType = statType.CurrentHelth, Stat = 0 }

    };

    [SerializeField] private StatGen<float> duration = new StatGen<float> { StatType = statType.Duration, Stat = 0 };

    [SerializeField] private int stackSize = 3;
    //Adds all the values that are diffrent from 0
    public override List<IStat> GetItemStats()
    {
        List<IStat> statsList = new List<IStat>();

        // Get all instance, non-public, and public fields
        var fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        foreach (IStat item in integerStats)
        {
                if (item.TryParseToInt(out int intValue) && intValue > 0)
                {
                    statsList.Add(item);
                }
        }

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


    public float Duration { get { return duration.Stat; } }
    public int StackSize { get { return stackSize; } set { stackSize = value; } }
    public bool isTemporary() 
    {
        return duration.Stat > 0;
    }

    public override bool IsStackable()
    {
        return true;
    }
}
