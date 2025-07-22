using UnityEngine;

/// <summary>
/// The base cell object class
/// </summary>
public class CellObject : MonoBehaviour
{
    protected Vector2Int mCell;

    public virtual void init(Vector2Int cell)
    {
        mCell = cell;
    }

    public virtual void playerEntered() { }
}