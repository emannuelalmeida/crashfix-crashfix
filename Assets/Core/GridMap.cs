using Assets.Tiles;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using System;

public class GridMap : ScriptableObject
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    public float TotalW => tileLength * (Width - 1);

    public float TotalH => tileLength * (Height - 1);

    private Tile[,] tileMap;

    private string theme;
    private string basePath => $"Tiles/Prefabs/{theme}/";

    public Vector3 basePosition { get; private set; }
    public float TileLength { get { return tileLength; } }

    private const float tileLength = 1.6f;
    private System.Random rand;

    private MapManager mapManager;

    public GridMap()
    {
        rand = new System.Random(System.DateTime.Now.Millisecond);
    }

    public void Initialize(string theme, Vector3 basePosition, MapManager manager)
    {
        this.theme = theme;
        this.basePosition = basePosition;
        this.mapManager = manager;
    }

    public void ConstructTiles(TileType[,] tileArray)
    {
        Width = tileArray.GetLength(0);
        Height = tileArray.GetLength(1);

        tileMap = new Tile[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                InitializeTile(tileArray[x, y], x, y);
            }
        }
    }

    public bool IsWalkablePosition(Position position)
    {
        return tileMap[position.X, position.Y].IsWalkable;
    }

    private void InitializeTile(TileType type, int x, int y)
    {
        string resourceName;

        switch (type)
        {
            case TileType.GROUND:
                resourceName = "ground";
                break;
            case TileType.SOLID_WALL:
                resourceName = "wall";
                break;
            case TileType.BREAK_BLOCK:
                resourceName = "break_block";
                break;
            case TileType.BREAK_START:
                resourceName = "ground";
                var breaker = Resources.Load("Actors/breaker");
                var breakerObj = Instantiate(breaker,
                            basePosition + new Vector3(tileLength * x - tileLength / 2f, 2.4f, tileLength * y - tileLength / 2f),
                            Quaternion.identity) as GameObject;
                breakerObj.GetComponent<PlayerActor>().Initialize(mapManager, new Position(x, y));
                break;
            case TileType.FIX_BLOCK:
                resourceName = "fix_block";
                break;
            case TileType.FIX_START:
                resourceName = "ground";
                var fixer = Resources.Load("Actors/fixer");
                var fixerObj = Instantiate(fixer,
                            basePosition + new Vector3(tileLength * x - tileLength / 2f, 2.4f, tileLength * y - tileLength / 2f),
                            Quaternion.identity) as GameObject;
                fixerObj.GetComponent<PlayerActor>().Initialize(mapManager, new Position(x, y));
                break;
            case TileType.BOTH_ACT_BLOCK:
                resourceName = "both_act";
                break;
            case TileType.BUTTON:
                resourceName = "button";
                break;
            case TileType.DOOR:
                resourceName = "door";
                break;
            case TileType.HOLE:
                resourceName = "hole";
                break;
            case TileType.BREAK_EXIT:
                resourceName = "break_exit";
                break;
            case TileType.FIX_EXIT:
                resourceName = "fix_exit";
                break;
            case TileType.EXIT_BOTH:
                resourceName = "exit_both";
                break;
            default:
                return;
        }

        try
        {
            if (resourceName == "ground")
                resourceName += rand.Next(1, 5);

            var tileObject = Instantiate(Resources.Load(basePath + resourceName),
                        basePosition + new Vector3(tileLength * x, 0, tileLength * y),
                        Quaternion.identity);
            var tile = (tileObject as GameObject).GetComponent<Tile>();

            tileMap[x, y] = tile;
        }
        catch (Exception ex)
        {


            Debug.LogError(ex.Message);
            var tileObject = Instantiate(Resources.Load(basePath + "ground" + rand.Next(1, 5)),
                        basePosition + new Vector3(tileLength * x, 0, tileLength * y),
                        Quaternion.identity);

            var tile = (tileObject as GameObject).GetComponent<Tile>();

            tileMap[x, y] = tile;
        }


    }

    public Tile GetTile(int x, int y)
    {
        return tileMap[x, y];
    }
}
