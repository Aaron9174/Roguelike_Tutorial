using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public PlayerScript mPlayer;

    public class CellData
    {
        public CellData(bool isPassable)
        {
            mIsPassable = isPassable;
        }

        public bool mIsPassable;
    }
    private CellData[,] mBoardData;

    private Tilemap tilemap;

    public int mWidth;
    public int mHeight;
    public Tile[] mGroundTiles;
    public Tile[] mWallTiles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tilemap = this.gameObject.GetComponentInChildren<Tilemap>();
        mBoardData = new CellData[mWidth, mHeight];

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
                }
            }
        }

        mPlayer.spawn(this, new Vector2Int(1, 1));        
    }

    // Update is called once per frame
    void Update()
    {

    }

    /**
     * Determines if the coordinate is on an edge of a range
     * <param name="coordinate"> The coordinate to test
     * <param name="min"> The min value of the range
     * <param name="max"> The max value of the range
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

    /**
     * Converts a 2D cell position to world coordinates
     * <param name="cellPosition"> The cell position to convert
     * <returns> the world coordinates 
     */
    public Vector3 cellToWorld(Vector2Int cellPosition)
    {
        return tilemap.GetCellCenterWorld((Vector3Int)cellPosition); 
    }
}
