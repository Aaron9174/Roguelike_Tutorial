using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private Tilemap tilemap;

    public int width;
    public int height;
    public Tile[] groundTiles;
    public Tile[] wallTiles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tilemap = this.gameObject.GetComponentInChildren<Tilemap>();
        for (int y = 0; y < height; ++y)
        {
            bool isOnHeightEdge = isCoordinateOnEdge(y, 0, height);
            for (int x = 0; x < width; ++x)
            {
                bool isOnWidthEdge = isCoordinateOnEdge(x, 0, width);
                if (isOnHeightEdge || isOnWidthEdge)
                {
                    int tileNumber = Random.Range(0, wallTiles.Length);
                    tilemap.SetTile(new Vector3Int(x, y, 0), wallTiles[tileNumber]);
                }
                else
                {
                    int tileNumber = Random.Range(0, groundTiles.Length);
                    tilemap.SetTile(new Vector3Int(x, y, 0), groundTiles[tileNumber]);
                }
            }
        }
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
        if (coordinate == min || coordinate == (max-1))
        {
            isEdge = true;
        }
        return isEdge;
    }
}
