using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

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
        public CellObject mContainedObject;
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
    public FoodObject[] mFoodPrefabs;

    /// <summary> This is the minimum food amount a level can start with </summary>
    public int mMinFoodAmount;

    /// <summary> This is the maximum food amount a level can start with </summary>
    public int mMaxFoodAmount;

    /// <summary> The wall prefab to spawn </summary>
    public WallObject[] mBreakableWallPrefabs;

    /// <summary> This is the minimum amount of walls a level can start with </summary>
    public int mMinWallAmount;

    /// <summary> This is the maximum amount of walls a level can start with </summary>
    public int mMaxWallAmount;

    /// <summary>
    /// The exit prefab, only one spawned per level
    /// </summary>
    public ExitObject mExitPrefab;

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

        // Generate the exit on the gameboard
        generateExit();

        // Generate the walls on the gameboard
        generateWalls();

        // Generate food on the gameboard
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

    public CellData getCellData(Vector2Int pos)
    {
        return mBoardData[pos.x, pos.y];
    }

    public void setBoardTile(Tile tile, Vector2Int pos)
    {
        tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), tile);
    }

    public Tile getCellTile(Vector2Int cell)
    {
        return tilemap.GetTile<Tile>(new Vector3Int(cell.x, cell.y, 0));
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
    /// Generates the exit tile in the board state
    /// </summary>
    void generateExit()
    {
        ExitObject exit = Instantiate(mExitPrefab);

        // Exit is always the top right hand corner of the playable map
        Vector2Int exitCell = new Vector2Int(mWidth-2, mHeight-2);
        addObject(exit, exitCell);
        mEmptyCells.Remove(exitCell);
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
            int prefabIndex = Random.Range(0, mFoodPrefabs.Length);
            FoodObject newFood = Instantiate(mFoodPrefabs[prefabIndex]);

            // Get a random empty cell, add the food to that tile, and then remove the empty cell
            int emptyCellIndex = Random.Range(0, mEmptyCells.Count);
            Vector2Int emptyCell = mEmptyCells[emptyCellIndex];
            addObject(newFood, emptyCell);
            mEmptyCells.Remove(emptyCell);
        }
    }

    /// <summary>
    /// Generates a configured number of walls at the start of each level
    /// </summary>
    void generateWalls()
    {
        int wallAmount = Random.Range(mMinWallAmount, mMaxWallAmount);
        for (int i = 0; i < wallAmount; i++)
        {
            int wallIndex = Random.Range(0, mBreakableWallPrefabs.Length);
            WallObject wall = Instantiate(mBreakableWallPrefabs[wallIndex]);

            // Grab a random empty cell,add wall to fill it, then remove the empty cell
            int emptyCellIndex = Random.Range(0, mEmptyCells.Count);
            Vector2Int emptyCell = mEmptyCells[emptyCellIndex];
            addObject(wall, emptyCell);
            mEmptyCells.Remove(emptyCell);
        }
    }

    /// <summary>
    /// Adds an object to an empty cell
    /// </summary>
    /// <param name="obj"> The object to add </param>
    /// <param name="emptyCell"> The position of the empty cell to fill </param>
    void addObject(CellObject obj, Vector2Int emptyCell)
    {
        obj.init(emptyCell);
        obj.transform.position = cellToWorld(emptyCell);
        CellData cell = mBoardData[emptyCell.x, emptyCell.y];
        cell.mContainedObject = obj;
    }
}
