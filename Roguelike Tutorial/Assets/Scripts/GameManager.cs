using UnityEngine;
using UnityEngine.UIElements;

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

    public UIDocument uiDocument;
    private Label mFoodLabel;

    /// <summary> The food amount the player currently has </summary>
    private int mFoodAmount = 100;

    private int levelCount = 1;

    /// <summary>
    /// The Unity Awake lifecycle hook
    /// </summary>
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
        mTurnManager.OnTick += onTurnTick;

        mBoardManager.initialize();
        mPlayerController.spawn(mBoardManager, new Vector2Int(1, 1));

        VisualElement ve = uiDocument.rootVisualElement;
        mFoodLabel = ve.Q<Label>("FoodLabel");
        mFoodLabel.text = "Food : " + mFoodAmount;
    }

    /// <summary> Used as a delegate to run once per turn </summary>
    void onTurnTick()
    {
        changeFood(-1);
        mFoodLabel.text = "Food: " + mFoodAmount;
    }

    /// <summary>
    /// Changes the food amount
    /// </summary>
    /// <param name="amount"> The amount to change by </param>
    public void changeFood(int amount)
    {
        mFoodAmount += amount;
    }

    /// <summary>
    /// Genereates a new level by
    /// - Cleaning up the current board state
    /// - Initializing a new board
    /// - Spawning the player in the starting position
    /// - Incrementing the level count
    /// </summary>
    public void generateNewLevel()
    {
        mBoardManager.cleanUpBoard();
        mBoardManager.initialize();
        mPlayerController.spawn(mBoardManager, new Vector2Int(1, 1));
        levelCount++;
    }

}
