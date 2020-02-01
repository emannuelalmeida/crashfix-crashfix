using Assets.Tiles;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    GridMap map;

    // Start is called before the first frame update
    void Start()
    {
        map = ScriptableObject.CreateInstance<GridMap>();
        map.Initialize("Default", Vector3.zero);

        var mapArray = new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 5, 7, 7, 7, 7, 7, 1, 1, 7, 7, 1, 2, 2, 7, 2, 2, 2, 2, 2, 1, 2, 7, 2, 1, 2, 2, 7, 2, 1, 7, 7, 1, 7, 2, 8, 2, 1, 2, 2, 7, 2, 2, 2, 7, 2, 2, 2, 8, 2, 1, 2, 2, 7, 1, 1, 1, 1, 2, 2, 2, 8, 2, 1, 2, 2, 8, 2, 2, 2, 7, 3, 8, 8, 8, 1, 8, 2, 2, 8, 2, 2, 2, 2, 2, 2, 8, 8, 2, 8, 2, 2, 8, 2, 2, 2, 2, 1, 1, 1, 1, 2, 8, 2, 2, 8, 2, 2, 2, 2, 8, 2, 2, 1, 2, 8, 2, 2, 8, 2, 2, 2, 2, 8, 2, 2, 2, 2, 8, 2, 2, 8, 8, 8, 8, 1, 1, 8, 8, 8, 8, 6, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };


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

    // Update is called once per frame
    void Update()
    {
        
    }
}
