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
