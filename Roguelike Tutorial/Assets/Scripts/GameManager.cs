using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    // TODO: add constant docs
    private const int STARTING_FOOD_DEFAULT = 10;
    private const int STARTING_LEVEL_COUNT = 0;

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
    private int mFoodAmount = STARTING_FOOD_DEFAULT;

    /// <summary>
    /// The game over panel displayed when the player loses
    /// </summary>
    private VisualElement mGameOverPanel;

    /// <summary>
    /// TODO: docs
    /// </summary>
    private Label mGameOverLabel;

    /// <summary>
    /// TODO: docs
    /// </summary>
    private Button mRestartBtn;

    /// <summary>
    /// TODO: docs
    /// </summary>
    private Button mExitBtn;

    /// <summary>
    /// The current level count
    /// </summary>
    private int mLevelCount = STARTING_LEVEL_COUNT;

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
        mGameOverLabel = mGameOverPanel.Q<Label>("GameOverLevelMessage");
        mRestartBtn = mGameOverPanel.Q<Button>("RestartButton");
        mRestartBtn.clicked += restartGame;
        mExitBtn = mGameOverPanel.Q<Button>("ExitButton");
        mExitBtn.clicked += exitGame;

        toggleUI(false);
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
    /// TODO: these style params don't work, update them
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
        bool playerIsDead = mFoodAmount <= 0;
        if (playerIsDead)
        {
            mGameOverLabel.text = "Survived " + mLevelCount + " days";
            mLevelCount = STARTING_LEVEL_COUNT;
            mPlayerController.mIsGameOver = true;
            toggleUI(true);
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

    /// <summary>
    /// TODO: docs
    /// </summary>
    private void restartGame()
    {
        toggleUI(false);

        generateNewLevel();

        mPlayerController.mIsGameOver = false;
        mFoodAmount = STARTING_FOOD_DEFAULT;
        mFoodLabel.text = "Food: " + mFoodAmount;
        mLevelCount = STARTING_LEVEL_COUNT;
    }

    /// <summary>
    /// Toggles the UI visibility
    /// TODO: update param style
    /// <param name="enable"> If true, makes the UI visible. Otherwise, hides the UI. </param>
    /// </summary>
    private void toggleUI(bool enable)
    {
        Visibility v = enable ? Visibility.Visible : Visibility.Hidden;
        mExitBtn.style.visibility = v;
        mRestartBtn.style.visibility = v;
        mGameOverPanel.style.visibility = v;
    }

    /// <summary>
    /// TODO: docs
    /// TODO: look into macros(?) for C#
    /// </summary>
    private void exitGame()
    {
        // This line is for quitting in the Unity Editor
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        // This line is for quitting in a built application
#else
                Application.Quit();
#endif
        Debug.Log("Application Quit.");
    }

}
