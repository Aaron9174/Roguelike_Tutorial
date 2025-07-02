using System;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2Int mCellPosition;
    private BoardManager mBoardState;

    /// <summary>
    /// Start is called once before the first execution of Update after the MonoBehaviour is created
    /// </summary>
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        controlPlayer();
    }

    public void spawn(BoardManager boardManager, Vector2Int cell)
    {
        mBoardState = boardManager;
        mCellPosition = cell;

        // TODO: Do stuff on movement
        transform.position = boardManager.cellToWorld(cell);

        mCellPosition = cell;
    }

    private void controlPlayer()
    {
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

        if (moveDetected && mBoardState.isPassable(newCellPosition))
        {
            GameManager.mInstance.mTurnManager.tick();
            moveTo(newCellPosition);
        }
    }

    private void moveTo(Vector2Int newCellPosition)
    {
        transform.position = mBoardState.cellToWorld(newCellPosition);
        mCellPosition = newCellPosition;
    }
}
