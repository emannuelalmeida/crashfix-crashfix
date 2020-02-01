using Assets.Tiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : ScriptableObject
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    private Tile[,] tileMap;

    private string theme;
    private string basePath => $"Tiles/Prefabs/{theme}/";

    private Vector3 basePosition;

    private const float tileLength = 1.6f;

    public GridMap()
    {

    }

    public void Initialize(string theme, Vector3 basePosition)
    {
        this.theme = theme;
        this.basePosition = basePosition;

        ConstructTiles(new TileType[2, 2] { { TileType.GROUND, TileType.SOLID_WALL }, { TileType.SOLID_WALL, TileType.SOLID_WALL } });
    }

    public void ConstructTiles(TileType[,] tileArray)
    {
        Width = tileArray.GetLength(0);
        Height = tileArray.GetLength(1);

        tileMap = new Tile[Width, Height];

        for(int x = 0; x < Width; x++)
        {
            for(int y = 0; y < Height; y++)
            {
                InitializeTile(tileArray[x, y], x, y);
            }
        }
    }

    private void InitializeTile(TileType type, int x, int y)
    {
        switch (type)
        {
            case TileType.GROUND:
                break;
            case TileType.SOLID_WALL:
                Instantiate(Resources.Load(basePath + "wall"), basePosition + new Vector3(tileLength * x, 0, tileLength * y), Quaternion.identity);
                break;
            default:
                break;
        }
    }
}
