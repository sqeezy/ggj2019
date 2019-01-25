using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Tile TilePrefab;
    public int XWidth;
    public int YHeight;


    private Tile[,] _grid;

    // Start is called before the first frame update
    private void Start()
    {
        _grid = new Tile[XWidth, YHeight];
        for (var xIndex = 0; xIndex < XWidth; xIndex++)
        {
            for (var yIndex = 0; yIndex < YHeight; yIndex++)
            {
                Tile tile = Instantiate(TilePrefab);
                tile.transform.position = new Vector3(xIndex, yIndex, 0);
                _grid[xIndex, yIndex] = tile;
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}