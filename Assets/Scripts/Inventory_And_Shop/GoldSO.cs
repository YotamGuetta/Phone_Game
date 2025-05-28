using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gold Item", menuName = "ItemsSO/New Gold Item")]
public class GoldSO : ItemAbs_SO
{
    [SerializeField]private StatGen<int> stackSize = new StatGen<int> {StatType = statType.StackSize, Stat = 2 };
    public override List<IStat> GetItemStats()
    {
        return new List<IStat> { stackSize };
    }

    public override bool IsStackable()
    {
        return false;
    }

    public int GoldAmount { get { return stackSize.Stat; } set { stackSize.Stat = value; } }
}
