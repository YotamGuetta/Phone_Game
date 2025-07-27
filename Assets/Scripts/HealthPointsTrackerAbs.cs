using UnityEngine;

public abstract class HealthPointsTrackerAbs : MonoBehaviour
{
    public abstract int CurrentHealth { get; set; }
    public abstract int MaxHealth { get; set; }
    protected abstract void uniteDied();
}
