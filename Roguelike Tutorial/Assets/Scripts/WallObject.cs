
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    public Tile mObstacleTile;

    public override void init(Vector2Int cell)
    {
        base.init(cell);

        GameManager.mInstance.mBoardManager.setBoardTile(mObstacleTile, cell);
    }

    public override bool playerWantsToEnter()
    {
        return false;
    }
}