using Assets.Tiles;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    GridMap map;

    // Start is called before the first frame update
    void Start()
    {
        map = ScriptableObject.CreateInstance<GridMap>();
        LoadMap(1);
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

    public void LoadMap(int mapNumber)
    {
        var jsonContent = File.ReadAllText($"Assets/Resources/Maps/level_{mapNumber}.json");
        var jsonObject = JObject.Parse(jsonContent);

        int width = jsonObject.GetValue("width").Value<int>();
        int[] mapIntArray = jsonObject.SelectToken("layers[0].data").Values<int>().ToArray();
        string theme = jsonObject.SelectToken("properties[0]").Value<string>("value");
        int timeInSeconds = jsonObject.SelectToken("properties[1]").Value<int>("value");

        Debug.Log($"Loaded Level {mapNumber} with Width {width}, Theme: {theme}, Time: {timeInSeconds}");

        map.Initialize(theme, Vector3.zero);
        map.ConstructTiles(ParseIntArray(mapIntArray, width));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
