using Assets.Tiles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    GridMap map;

    public Position FixPosition { get; set; }

    public Position BreakPosition { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        map = ScriptableObject.CreateInstance<GridMap>();
        map.Initialize("Default", Vector3.zero);

        var mapArray = new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 1, 2, 2, 2, 2, 2, 1, 2, 1, 2, 1, 2, 2, 1, 2, 1, 1, 1, 1, 1, 2, 1, 2, 1, 2, 2, 1, 2, 2, 2, 1, 2, 2, 2, 1, 2, 1, 2, 2, 1, 1, 1, 1, 1, 2, 2, 2, 1, 2, 1, 2, 2, 1, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1, 1, 2, 1, 2, 2, 1, 2, 2, 2, 2, 1, 1, 1, 1, 2, 1, 2, 2, 1, 2, 2, 2, 2, 1, 2, 2, 1, 2, 1, 2, 2, 1, 2, 2, 2, 2, 1, 2, 2, 2, 2, 1, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };


        map.ConstructTiles(ParseIntArray(mapArray, 13));
    }

    private TileType[,] ParseIntArray(int[] mapArray, int width)
    {
        var typeArray = new TileType[13, 13];

        for (int i = 0; i < mapArray.Length; i++)
        {
            typeArray[i % width, i / width] = (TileType)mapArray[i];
        }

        return typeArray;
    }

    private void Move(Position delta)
    {
        MoveActor(delta, true); // First, move the breaker
        MoveActor(delta, false); // Now, moving the fixer
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
        DrawActors();
    }

    private void DrawActors()
    {
        Instantiate(Resources.Load("actors/breaker"),
                    map.basePosition + new Vector3(map.TileLength * BreakPosition.X, 0, map.TileLength * BreakPosition.Y),
                    Quaternion.identity);

        Instantiate(Resources.Load("actors/fixer"),
                    map.basePosition + new Vector3(map.TileLength * FixPosition.X, 0, map.TileLength * FixPosition.Y),
                    Quaternion.identity);
    }

    private void MoveActor(Position delta, bool breaker)
    {
        Position position = breaker ? BreakPosition : FixPosition;

        if (map.IsWalkablePosition(position + delta))
        {
            position += delta;
            //After moving, check whether there is something to do on this block
        }

        else
        {
            //Check whether you can perform some action there
        }
    }

    private void ProcessInput()
    {
       float yDelta = Input.GetAxis("Vertical") * Time.deltaTime;
       float xDelta = Input.GetAxis("Horizontal") * Time.deltaTime;

       Move(new Position { X = (int)xDelta, Y = (int)yDelta });
    }
}
