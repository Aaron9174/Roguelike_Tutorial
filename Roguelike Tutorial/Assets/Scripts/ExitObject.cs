using UnityEngine;
using UnityEngine.Tilemaps;

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
        GameManager.mInstance.generateNewLevel();
    }
}
