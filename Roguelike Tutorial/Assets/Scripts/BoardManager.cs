using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;
using System.Data;

/// <summary>
/// Manages the state of the board
/// </summary>
public class BoardManager : MonoBehaviour
{
    /// <summary>
    /// Represents data within a cell on the game board
    /// </summary>
    public class CellData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="isPassable"> If true, the cell is passable by the player, otherwise the cell is unpassable. </param>
        public CellData(bool isPassable)
        {
            mIsPassable = isPassable;
        }

        /// <summary>
        /// If true, the cell is passable by the player. Otherwise, the player cannot pass through this cell.
        /// </summary>
        public bool mIsPassable;

        /// <summary>
        /// If the cell contains a game object, it will be stored here
        /// E.G. Food items
        /// </summary>
        public GameObject mContainedObject;
    }

    /// <summary>
    /// The width of the board
    /// </summary>
    public int mWidth;

    /// <summary>
    /// The height of the board
    /// </summary>
    public int mHeight;

    /// <summary>
    /// The ground tile sprites to choose from
    /// <remarks> All of these should be passable cells cells </remarks>
    /// </summary>
    public Tile[] mGroundTiles;

    /// <summary>
    /// The wall tile sprites to choose from
    /// <remarks> All of these should NOT be passable cells </remarks>
    /// </summary>
    public Tile[] mWallTiles;

    /// <summary>
    /// Stores the possible gameobjects + sprites to render for the food
    /// </summary>
    public GameObject[] mFoodPrefabs;

    /// <summary> This is the minimum food amount a level can start with </summary>
    public int mMinFoodAmount;

    /// <summary> This is the maximum food amound a level can start with </summary>
    public int mMaxFoodAmount;

    /// <summary>
    /// A 2D array containing all of the cell data of the current board state
    /// </summary>
    private CellData[,] mBoardData;

    /// <summary>
    /// The tilemap object used to render the sprites associated with the game board
    /// </summary>
    private Tilemap tilemap;

    /// <summary>
    /// Stores the current empty cells of the board
    /// </summary>
    private List<Vector2Int> mEmptyCells;

    /// <summary>
    /// Initialize the gameboard by: <para/>
    /// - Getting a reference to the tilemap attached to the child of the gameobject this script belongs to <para/>
    /// - Creating the gameboard based on the publically provided width and height <para/>
    /// - Creates the proper cell data for each spot in the game board based on whether it's on an edge or not <para/>
    /// - Sets the proper sprite in the tilemap based on whether it's on an edge or not <para/>
    /// </summary>
    public void initialize()
    {
        tilemap = this.gameObject.GetComponentInChildren<Tilemap>();
        mBoardData = new CellData[mWidth, mHeight];
        mEmptyCells = new List<Vector2Int>();

        for (int y = 0; y < mHeight; ++y)
        {
            bool isOnHeightEdge = isCoordinateOnEdge(y, 0, mHeight);
            for (int x = 0; x < mWidth; ++x)
            {
                bool isOnWidthEdge = isCoordinateOnEdge(x, 0, mWidth);
                if (isOnHeightEdge || isOnWidthEdge)
                {
                    int tileNumber = Random.Range(0, mWallTiles.Length);
                    tilemap.SetTile(new Vector3Int(x, y, 0), mWallTiles[tileNumber]);

                    mBoardData[x, y] = new CellData(false);
                }
                else
                {
                    int tileNumber = Random.Range(0, mGroundTiles.Length);
                    tilemap.SetTile(new Vector3Int(x, y, 0), mGroundTiles[tileNumber]);

                    mBoardData[x, y] = new CellData(true);
                    mEmptyCells.Add(new Vector2Int(x, y));
                }
            }
        }

        // Remove the starting point from empty list, since the player will spawn here
        mEmptyCells.Remove(new Vector2Int(1, 1));

        // Generate food on the game board
        generateFood();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // TODO: Remove this if not used
    }

    /// <summary>
    /// Determines if the cell in the game board is passable or not
    /// </summary>
    /// <param name="pos"> The position in the gameboard </param>
    /// <returns> True if passable, false if not passable </returns>
    public bool isPassable(Vector2Int pos)
    {
        return mBoardData[pos.x, pos.y].mIsPassable;
    }

    /**
     * Converts a 2D cell position to world coordinates
     * <param name="cellPosition"> The cell position to convert </param>
     * <returns> the world coordinates </returns>
     */
    public Vector3 cellToWorld(Vector2Int cellPosition)
    {
        return tilemap.GetCellCenterWorld((Vector3Int)cellPosition);
    }

    /**
     * Determines if the coordinate is on an edge of a range
     * <param name="coordinate"> The coordinate to test </param>
     * <param name="min"> The min value of the range </param>
     * <param name="max"> The max value of the range </param>
     * <returns> True if on an edge, false otherwise </returns>
     */
    bool isCoordinateOnEdge(int coordinate, int min, int max)
    {
        bool isEdge = false;
        if (coordinate == min || coordinate == (max - 1))
        {
            isEdge = true;
        }
        return isEdge;
    }

    /// <summary>
    /// Generates food in the game board by <para />
    /// - Iterating over a set amount of food for the level <para />
    /// - Choosing a random spot passable and non-walled cell in the board data <para />
    /// - Creates a new food object and assigns it to that board data cell
    /// </summary>
    void generateFood()
    {
        int foodAmount = Random.Range(mMinFoodAmount, mMaxFoodAmount);
        for (int i = 0; i < foodAmount; i++)
        {
            int emptyCellIndex = Random.Range(0, mEmptyCells.Count);
            Vector2Int emptyCell = mEmptyCells[emptyCellIndex];
            CellData cell = mBoardData[emptyCell.x, emptyCell.y];

            int prefabIndex = Random.Range(0, mFoodPrefabs.Length);
            GameObject newFood = Instantiate(mFoodPrefabs[prefabIndex]);
            newFood.transform.position = cellToWorld(emptyCell);
            cell.mContainedObject = newFood;

            // Cell now has a food item, and is no longer empty
            mEmptyCells.Remove(emptyCell);
        }
    }
}
