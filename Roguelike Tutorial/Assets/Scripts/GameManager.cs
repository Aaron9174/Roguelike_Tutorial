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

    /// <summary>
    /// The UI document for displaying the games UI
    /// </summary>
    public UIDocument uiDocument;

    /// <summary>
    /// The food label
    /// </summary>
    private Label mFoodLabel;

    /// <summary> The food amount the player currently has </summary>
    public int mFoodAmount;

    /// <summary>
    /// The game over panel displayed when the player loses
    /// </summary>
    private VisualElement mGameOverPanel;

    /// <summary>
    /// TODO: docs
    /// </summary>
    private Label mGameOverLabel;

    /// <summary>
    /// The current level count
    /// </summary>
    private int mLevelCount = 1;

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

        mGameOverPanel = ve.Q<VisualElement>("GameOverPanel");
        mGameOverPanel.style.visibility = Visibility.Hidden;
        mGameOverLabel = mGameOverPanel.Q<Label>("GameOverLevelMessage");
    }

    /// <summary> Used as a delegate to run once per turn </summary>
    void onTurnTick()
    {
        changeFood(-1);
        mFoodLabel.text = "Food: " + mFoodAmount;
        checkGameOver();
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
    /// TODO: docs
    /// </summary>
    /// <returns></returns>
    private void checkGameOver()
    {
        if (mFoodAmount <= 0)
        {
            mGameOverPanel.style.visibility = Visibility.Visible;
            mGameOverLabel.text = "Max Level Achieved: " + mLevelCount;
            mLevelCount = 1;
        }
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
        mLevelCount++;
    }

}
