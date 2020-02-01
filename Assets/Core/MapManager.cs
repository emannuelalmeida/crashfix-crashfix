using Assets.Actors;
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
    private GridMap map;

    public TimeSpan TimeRemaining { get; private set; }

    private bool gameStarted;

    static float Pow2(float x) => Mathf.Pow(x, 2.0f);

    static float Hipotenuse(float x, float y) => Mathf.Sqrt(Pow2(x) + Pow2(y));

    static Vector3 PositionCam(float height, float width) => new Vector3(width / 2.0f, (Hipotenuse(height, width) / 2.0f) * Mathf.Sqrt(3.0f), height / 2.0f);

    // Start is called before the first frame update
    void Start()
    {
        map = ScriptableObject.CreateInstance<GridMap>();
        LoadMap(1);
        Camera.main.transform.position = PositionCam(map.TotalH, map.TotalW);
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

        TimeRemaining = TimeSpan.FromSeconds(timeInSeconds);
        Debug.Log($"Loaded Level {mapNumber} with Width {width}, Theme: {theme}, Time: {timeInSeconds}");

        map.Initialize(theme, Vector3.zero, this);
        map.ConstructTiles(ParseIntArray(mapIntArray, width));

        gameStarted = true;
    }

    public void FixTile(int x, int y)
    {
        map.GetTile(x, y).Fix();
    }

    public void BreakTile(int x, int y)
    {
        map.GetTile(x, y).Break();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted && TimeRemaining.TotalSeconds > 0)
        {
            TimeRemaining = TimeRemaining.Subtract(TimeSpan.FromSeconds(Time.deltaTime));
        }
    }  
    
    public bool CanMove(int x, int y)
    {
        return map.IsWalkablePosition(new Position(x, y));
    } 

    public Vector3 GetTilePosition(int x, int y)
    {
        return map.basePosition + new Vector3(map.TileLength * x, 0, map.TileLength * y);
    }

    public void OnStepTile(int x, int y, PlayerActor character)
    {
        Tile tile = map.GetTile(x, y);

        if (character is Fixer)
            tile.OnStepFixer();
        else
            tile.OnStepBreaker();
    }
}
