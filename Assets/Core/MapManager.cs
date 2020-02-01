using Assets.Tiles;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    GridMap map;

    TimeSpan timeRemaining;

    bool gameStarted;

    void Start()
    {
        map = ScriptableObject.CreateInstance<GridMap>();
        LoadMap(1);

        Debug.Log($"Screen W: {Screen.width} H: {Screen.height}");
        Debug.Log($"Tile   W: {map.TotalW}   H: {map.TotalH}");

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

    public void LoadMap(int mapNumber)
    {
        var jsonContent = File.ReadAllText($"Assets/Resources/Maps/level_{mapNumber}.json");
        var jsonObject = JObject.Parse(jsonContent);

        int width = jsonObject.GetValue("width").Value<int>();
        int[] mapIntArray = jsonObject.SelectToken("layers[0].data").Values<int>().ToArray();
        string theme = jsonObject.SelectToken("properties[0]").Value<string>("value");
        int timeInSeconds = jsonObject.SelectToken("properties[1]").Value<int>("value");

        timeRemaining = TimeSpan.FromSeconds(timeInSeconds);
        Debug.Log($"Loaded Level {mapNumber} with Width {width}, Theme: {theme}, Time: {timeInSeconds}");

        map.Initialize(theme, Vector3.zero);
        map.ConstructTiles(ParseIntArray(mapIntArray, width));

        gameStarted = true;
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (gameStarted && timeRemaining.TotalSeconds > 0)
        {
            timeRemaining = timeRemaining.Subtract(TimeSpan.FromSeconds(Time.fixedDeltaTime));
            Debug.Log(timeRemaining.TotalSeconds.ToString("0"));
        }
    }
}
