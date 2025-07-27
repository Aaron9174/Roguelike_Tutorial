using UnityEngine;

/// <summary>
/// The base cell object class
/// </summary>
public class CellObject : MonoBehaviour
{
    /// <summary>
    /// The passed in cell position
    /// </summary>
    protected Vector2Int mCell;

    public virtual void init(Vector2Int cell)
    {
        mCell = cell;
    }

    public virtual void playerEntered() { }

    public virtual bool playerWantsToEnter() { return true; }
}