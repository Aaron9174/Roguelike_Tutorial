using UnityEngine;

public class TurnManager
{
    /// <summary> Runs once per tick (once per turn) </summary>
    public event System.Action OnTick;

    /// <summary> Turn count total </summary>
    private int mTurnCount;

    public TurnManager()
    {
        mTurnCount = 1;
    }

    /// <summary> Increments the turn count </summary>
    public void tick()
    {
        mTurnCount++;
        OnTick?.Invoke();
    }
}
