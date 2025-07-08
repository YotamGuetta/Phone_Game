using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    /// <summary>
    /// Adds all the stats the item gives to the player
    /// </summary>
    /// <param name="itemSO">The Item</param>
    /// <param name="reverse">Choose to subtract the stats insted</param>
    public void ApplyItemEffects(ItemAbs_SO itemSO, bool reverse)
    {

        if (reverse)
        {
            foreach (var stat in itemSO.GetItemStats())
            {
                updateStats(stat, -1);
            }
        }
        else 
        {
            ApplyItemEffects(itemSO);
        }
    }
    private void ApplyItemEffects(ItemAbs_SO itemSO)
    {
        foreach (var stat in itemSO.GetItemStats())
        {
            updateStats(stat, 1);

            //If theres a duration in the stats add a timer to remove them when the duration ends
            if (stat.GetStatType() == statType.Duration) 
            {
                if (stat.TryParseToFloat(out float duration)) 
                {
                    StartCoroutine(EffectTimer(itemSO, duration));
                }
            }
        } 

    }
    private IEnumerator EffectTimer(ItemAbs_SO itemSO, float duration) 
    {
        yield return new WaitForSeconds(duration);
        foreach (var stat in itemSO.GetItemStats())
        {
            updateStats(stat, -1);
        }
    }
    private void updateStats(IStat item, int Multiplier)
    {
        int valueI;

        //updates the player stats based on the stat type
        switch (item.GetStatType())
        {
            case statType.CurrentHelth:
                item.TryParseToInt(out valueI);
                PlayerStatsManager.Instance.UpdateCurrentHealth(valueI * Multiplier);
                break;
            case statType.MaxHealth:
                item.TryParseToInt(out valueI);
                PlayerStatsManager.Instance.UpdateMaxHealth(valueI * Multiplier);
                break;
            case statType.Speed:
                item.TryParseToInt(out valueI);
                PlayerStatsManager.Instance.UpdateSpeed(valueI * Multiplier);
                break;
            case statType.Attack:
                item.TryParseToInt(out valueI);
                PlayerStatsManager.Instance.UpdateDamage(valueI * Multiplier);
                break;
        }

    }
}

