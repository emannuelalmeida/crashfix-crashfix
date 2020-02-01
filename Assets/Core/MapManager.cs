using Assets.Tiles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    GridMap map;

    void Start()
    {
        map = ScriptableObject.CreateInstance<GridMap>();
        map.Initialize("Default", Vector3.zero);

        var mapArray = new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 5, 7, 7, 7, 7, 7, 1, 1, 7, 7, 1, 2, 2, 7, 2, 2, 2, 2, 2, 1, 2, 7, 2, 1, 2, 2, 7, 2, 1, 7, 7, 1, 7, 2, 8, 2, 1, 2, 2, 7, 2, 2, 2, 7, 2, 2, 2, 8, 2, 1, 2, 2, 7, 1, 1, 1, 1, 2, 2, 2, 8, 2, 1, 2, 2, 8, 2, 2, 2, 7, 3, 8, 8, 8, 1, 8, 2, 2, 8, 2, 2, 2, 2, 2, 2, 8, 8, 2, 8, 2, 2, 8, 2, 2, 2, 2, 1, 1, 1, 1, 2, 8, 2, 2, 8, 2, 2, 2, 2, 8, 2, 2, 1, 2, 8, 2, 2, 8, 2, 2, 2, 2, 8, 2, 2, 2, 2, 8, 2, 2, 8, 8, 8, 8, 1, 1, 8, 8, 8, 8, 6, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };

        map.ConstructTiles(ParseIntArray(mapArray, 13));

        Camera.main.transform.position = PositionCam(map.TotalH, map.TotalW);
    }

    static float Pow2(float x) => Mathf.Pow(x, 2.0f);

    static float Hipotenuse(float x, float y) => Mathf.Sqrt(Pow2(x) + Pow2(y));

    static Vector3 PositionCam(float height, float width) => new Vector3(width / 2.0f, (Hipotenuse(height, width)/ 2.0f) * Mathf.Sqrt(3.0f), height / 2.0f);

    private TileType[,] ParseIntArray(int[] mapArray, int width)
    {
        var typeArray = new TileType[13, 13];

        for (int i = 0; i < mapArray.Length; i++)
        {
            typeArray[i % width, i / width] = (TileType)mapArray[i];
        }

        return typeArray;
    }

    void Update()
    {

    }
}
