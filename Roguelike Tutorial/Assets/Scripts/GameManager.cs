using UnityEngine;

public class GameManager : MonoBehaviour
{
    private TurnManager mTurnManager;
    public BoardManager mBoardManager;

    public PlayerController mPlayerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mTurnManager = new TurnManager();
        mBoardManager.initialize();
        mPlayerController.spawn(mBoardManager, new Vector2Int(1, 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
