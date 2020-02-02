using Assets.Tiles;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using System;
using Assets.Actors;

public class GridMap : ScriptableObject
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    public Breaker Breaker { get; private set; }
    public Fixer Fixer { get; private set; }

    public float TotalW => tileLength * (Width - 1);

    public float TotalH => tileLength * (Height - 1);

    private Tile[,] tileMap;

    public string Theme { get; private set; }
    private string BasePath => $"Tiles/Prefabs/{Theme}/";

    public Vector3 BasePosition { get; private set; }
    public float TileLength { get { return tileLength; } }

    private const float tileLength = 1.6f;
    private System.Random rand;

    private MapManager mapManager;

    public GridMap()
    {
        rand = new System.Random(System.DateTime.Now.Millisecond);
    }

    internal void setMapManager(MapManager mapManager)
    {
        this.mapManager = mapManager;
    }

    public void Initialize(string theme, Vector3 basePosition, MapManager manager)
    {
        Theme = theme;
        BasePosition = basePosition;
        mapManager = manager;
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
                resourceName = "vase";
                break;
            case TileType.BREAK_START:
                resourceName = "ground";
                var breaker = Resources.Load("Actors/breaker");
                var breakerObj = Instantiate(breaker,
                            BasePosition + new Vector3(tileLength * x - tileLength / 2f, 2.4f, tileLength * y - tileLength / 2f),
                            Quaternion.identity) as GameObject;
                Breaker = breakerObj.GetComponent<Breaker>();
                Breaker.Initialize(mapManager, new Position(x, y));
                break;
            case TileType.FIX_BLOCK:
                resourceName = "crate_broken";
                break;
            case TileType.FIX_START:
                resourceName = "ground";
                var fixer = Resources.Load("Actors/fixer");
                var fixerObj = Instantiate(fixer,
                            BasePosition + new Vector3(tileLength * x - tileLength / 2f, 2.4f, tileLength * y - tileLength / 2f),
                            Quaternion.identity) as GameObject;
                Fixer = fixerObj.GetComponent<Fixer>();
                Fixer.Initialize(mapManager, new Position(x, y));
                break;
            case TileType.BOTH_ACT_BLOCK:
                resourceName = "crate";
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

            var tileObject = Instantiate(Resources.Load(BasePath + resourceName),
                        BasePosition + new Vector3(tileLength * x, 0, tileLength * y),
                        Quaternion.identity);
            var tile = (tileObject as GameObject).GetComponent<Tile>();

            tileMap[x, y] = tile;
        }
        catch (Exception ex)
        {


            Debug.LogError(ex.Message);
            var tileObject = Instantiate(Resources.Load(BasePath + "ground" + rand.Next(1, 5)),
                        BasePosition + new Vector3(tileLength * x, 0, tileLength * y),
                        Quaternion.identity);

            var tile = (tileObject as GameObject).GetComponent<Tile>();

            tileMap[x, y] = tile;
        }

    }

    public Tile GetTile(int x, int y)
    {
        try
        {
            return tileMap[x, y];
        }
        catch { return null; }
    }
}
