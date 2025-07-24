
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    public Tile mObstacle;

    public Tile mObstaclePartialDmg;

    private Tile mOriginalTile;

    public int maxHealth = 3;

    private int mHealthPoints;

    public override void init(Vector2Int cell)
    {
        base.init(cell);

        mOriginalTile = GameManager.mInstance.mBoardManager.getCellTile(cell);
        mHealthPoints = maxHealth;

        GameManager.mInstance.mBoardManager.setBoardTile(mObstacle, cell);
    }

    public override bool playerWantsToEnter()
    {
        mHealthPoints--;
        switch (mHealthPoints)
        {
            case 1:
                GameManager.mInstance.mBoardManager.setBoardTile(mObstaclePartialDmg, mCell);
                return false;
            case 0:
            default:
            GameManager.mInstance.mBoardManager.setBoardTile(mOriginalTile, mCell);
            Destroy(gameObject);
                return true;
        }
    }
}