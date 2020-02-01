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

        Camera.main.transform.position = PositionCam(map.Width, map.Height);
    }

    static float Pow2(float x) => Mathf.Pow(x, 2.0f);

    static float Hipotenuse(float x, float y) => Mathf.Sqrt(Pow2(x) + Pow2(y));

    static Vector3 PositionCam(float height, float width)
    {
        var widthPadding = (width * 8 - 5 * height) / 10;

        var heightPadding = widthPadding;

        return new Vector3(
        (width + widthPadding) / 2.0f, 2 +
        Hipotenuse(height + heightPadding, width + widthPadding) * Mathf.Sqrt(3) / 2.0f,
        (height + heightPadding) / 2.0f);

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

    void Update()
    {

    }
}
