using UnityEngine;
using System;

[System.Serializable]
public class StatGen <T> : IStat
{
    public statType StatType;
    public T Stat;

    public int GetAsInt() 
    {
        int.TryParse(Stat.ToString(), out int value);
        return value;
    }

    public bool TryParseToInt(out int value)
    {
        if (Stat is int i) { value = i; return true; }
        value = 0;
        return false;
    }

    public bool TryParseToFloat(out float value)
    {
        if (Stat is float f) { value = f; return true; }
        if (Stat is int i) { value = i; return true; }
        value = 0;
        return false;

    }
    public override string ToString()
    {
        return StatType.ToString() + ": "+ Stat;
    }

    public statType GetStatType()
    {
        return StatType;
    }

}
public interface IStat
{
    bool TryParseToInt(out int value);
    bool TryParseToFloat(out float value);
    statType GetStatType();

}
    public enum statType 
{
    MaxHealth,
    CurrentHelth,
    Attack,
    Speed,
    Duration,
    StackSize
}