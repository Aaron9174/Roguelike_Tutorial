using UnityEngine;

public class TurnManager
{
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
        Debug.Log("Current turn count: " + mTurnCount);
    }
}
