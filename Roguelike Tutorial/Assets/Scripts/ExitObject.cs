using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class ExitObject : CellObject
{
    public Tile mExitTile;

    public override void init(Vector2Int cell)
    {
        base.init(cell);

        GameManager.mInstance.mBoardManager.setBoardTile(mExitTile, cell);
    }

    public override void playerEntered()
    {
        // TODO: start a new level
        Debug.Log("Reached an exit tile!");
    }
}
