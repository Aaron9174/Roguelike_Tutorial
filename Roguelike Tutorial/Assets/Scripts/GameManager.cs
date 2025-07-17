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
        mFoodAmount--;
        mFoodLabel.text = "Food: " + mFoodAmount;
        Debug.Log("Current amount of food: " + mFoodAmount);
    }
}
