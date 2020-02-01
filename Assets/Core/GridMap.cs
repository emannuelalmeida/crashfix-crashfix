﻿using Assets.Tiles;
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

    public Vector3 basePosition { get; private set; }
    public float TileLength { get { return tileLength; } }

    private const float tileLength = 1.6f;

    public GridMap()
    {

    }

    public void Initialize(string theme, Vector3 basePosition)
    {
        this.theme = theme;
        this.basePosition = basePosition;
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

    public bool IsWalkablePosition(Position position){
        return tileMap[position.X, position.Y].IsWalkable;
    }

    private void InitializeTile(TileType type, int x, int y)
    {
        string resourceName = null;

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
                resourceName = "break_start";
                break;
            case TileType.FIX_BLOCK:
                resourceName = "fix_block";
                break;
            case TileType.FIX_START:
                resourceName = "fix_start";
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

        Instantiate(Resources.Load(basePath + resourceName),
                    basePosition + new Vector3(tileLength * x, 0, tileLength * y),
                    Quaternion.identity);
    }

}
