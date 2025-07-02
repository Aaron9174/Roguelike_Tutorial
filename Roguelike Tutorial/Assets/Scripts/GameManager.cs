using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary> Gets access to an instance of the game manager </summary>
    public static GameManager mInstance { get; private set; }

    /// <summary> The turn manager reference </summary>
    public TurnManager mTurnManager { get; private set; }

    /// <summary> The board manager reference </summary>
    public BoardManager mBoardManager;

    /// <summary> Player controller reference </summary>
    public PlayerController mPlayerController;

    void Awake()
    {
        if (mInstance != null)
        {
            Destroy(gameObject);
            return;
        }

        mInstance = this;
    }

    /// <summary>
    /// Start is called once before the first execution of Update after the MonoBehaviour is created
    /// </summary>
    void Start()
    {
        mTurnManager = new TurnManager();
        mBoardManager.initialize();
        mPlayerController.spawn(mBoardManager, new Vector2Int(1, 1));
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        
    }
}
