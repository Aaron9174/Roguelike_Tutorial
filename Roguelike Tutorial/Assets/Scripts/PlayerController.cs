using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2Int mCellPosition;
    private BoardManager mBoardState;

    /// <summary>
    /// TODO: docs
    /// </summary>
    public bool mIsGameOver = false;

    // Update is called once per frame
    void Update()
    {
        controlPlayer();
    }

    public void spawn(BoardManager boardManager, Vector2Int cell)
    {
        mBoardState = boardManager;

        transform.position = boardManager.cellToWorld(cell);
        mCellPosition = cell;
    }

    private void controlPlayer()
    {
        if (mIsGameOver)
        {
            return;
        }

        Vector2Int newCellPosition = mCellPosition;
        bool moveDetected = false;
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            moveDetected = true;
            newCellPosition.y += 1;
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            moveDetected = true;
            newCellPosition.y -= 1;
        }
        else if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            moveDetected = true;
            newCellPosition.x -= 1;
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            moveDetected = true;
            newCellPosition.x += 1;
        }

        if (moveDetected)
        {
            BoardManager.CellData cellData = mBoardState.getCellData(newCellPosition);

            // This if for edge walls
            if (cellData == null || !cellData.mIsPassable)
            {
                return;
            }
            // Ground tiles 
            else if (cellData.mContainedObject == null)
            {
                performMove(newCellPosition);
            }
            // Food or wall objects
            else if (cellData.mContainedObject.playerWantsToEnter())
            {
                performMove(newCellPosition);

                cellData.mContainedObject.playerEntered();
            }
        }
    }

    private void performMove(Vector2Int newCellPosition)
    {
        GameManager.mInstance.mTurnManager.tick();
        moveTo(newCellPosition);
    }

    private void moveTo(Vector2Int newCellPosition)
    {
        transform.position = mBoardState.cellToWorld(newCellPosition);
        mCellPosition = newCellPosition;
    }
}
