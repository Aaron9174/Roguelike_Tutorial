
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    public Tile mObstacleTile;

    private Tile mOriginalTile;

    public int maxHealth = 3;

    private int mHealthPoints;

    public override void init(Vector2Int cell)
    {
        base.init(cell);

        mOriginalTile = GameManager.mInstance.mBoardManager.getCellTile(cell);
        mHealthPoints = maxHealth;

        GameManager.mInstance.mBoardManager.setBoardTile(mObstacleTile, cell);
    }

    public override bool playerWantsToEnter()
    {
        mHealthPoints--;
        if (mHealthPoints > 1)
        {
            return false;
        }

        GameManager.mInstance.mBoardManager.setBoardTile(mOriginalTile, mCell);
        Destroy(gameObject);
        return true;
    }
}