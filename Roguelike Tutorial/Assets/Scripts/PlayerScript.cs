using UnityEditor;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Vector2Int mCellPosition;
    private BoardManager mBoardState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spawn(BoardManager boardManager, Vector2Int cell)
    {
        mBoardState = boardManager;
        mCellPosition = cell;

        // TODO: Do stuff on movement
        transform.position = boardManager.cellToWorld(cell);
    }
}
