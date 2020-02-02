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
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core
{
    public class MapManager : MonoBehaviour
{
    private GridMap map;
    private GameState gameState = GameState.Start;

    public TimeSpan TimeRemaining { get; private set; }

    int TimeInSeconds { get; set; }
    private int InitialTimeInSeconds { get; set; }

    private bool gameStarted;
    private int currentMap;

    static float Pow2(float x) => Mathf.Pow(x, 2.0f);

    static float Hipotenuse(float x, float y) => Mathf.Sqrt(Pow2(x) + Pow2(y));

    static Vector3 PositionCam(float height, float width) => new Vector3(width / 2.0f, (Hipotenuse(height, width) / 2.0f) * Mathf.Sqrt(3.0f), height / 2.0f);

    // Start is called before the first frame update
    void Start()
    {
        map = ScriptableObject.CreateInstance<GridMap>();
        currentMap = 0;
        StartNextLevelOrWin();
    }

    private void AdjustCameraPosition(GridMap map)
    {
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

    public void RestartCurrentLevel()
    {
            string nextMapUrl = $"Maps/level_{currentMap}";
            var mapData = Resources.Load<TextAsset>(nextMapUrl);
            restartButton(0f, false);
    }

    private void StartNextLevelOrWin()
    {
        currentMap++;
        string nextMapUrl = $"Maps/level_{currentMap}";
        var mapData = Resources.Load<TextAsset>(nextMapUrl);

        if (mapData != null)
            LoadMap(mapData);
        else
            WinGameScreen();
    }

    private void WinGameScreen()
    {
        //
    }

    private void LoadMap(TextAsset mapData)
    {
        var jsonObject = JObject.Parse(mapData.text);

        int width = jsonObject.GetValue("width").Value<int>();
        int[] mapIntArray = jsonObject.SelectToken("layers[0].data").Values<int>().ToArray();
        string theme = jsonObject.SelectToken("properties[0]").Value<string>("value");
        InitialTimeInSeconds = jsonObject.SelectToken("properties[1]").Value<int>("value");

        TimeRemaining = TimeSpan.FromSeconds(InitialTimeInSeconds);
        Debug.Log($"Loaded Level {mapData.name} with Width {width}, Theme: {theme}, Time: {InitialTimeInSeconds}");

        map.ClearCurrentMap();
        map.Initialize(theme, Vector3.zero, this);
        map.ConstructTiles(ParseIntArray(mapIntArray, width));
        AdjustCameraPosition(map);

        gameState = GameState.Playing;
        gameStarted = true;
    }

    public void FixTile(int x, int y)
    {
        map.GetTile(x, y)?.Fix();
    }

    public void BreakTile(int x, int y)
    {
        map.GetTile(x, y)?.Break();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted && TimeRemaining.TotalSeconds > 0)
        {
            TimeRemaining = TimeRemaining.Subtract(TimeSpan.FromSeconds(Time.deltaTime));
            var timer = GameObject.Find("Timer").GetComponent<Text>();
            timer.text = $"{TimeRemaining.Minutes.ToString("00")}:{TimeRemaining.Seconds.ToString("00")}";

            if (TimeRemaining <= TimeSpan.FromSeconds(0.17 * TimeInSeconds))
                timer.color = Color.red;
            else if (TimeRemaining <= TimeSpan.FromSeconds(0.5 * TimeInSeconds))
                timer.color = Color.yellow;
            else
                timer.color = Color.white;
        }
        else if (TimeRemaining.TotalSeconds < 1) 
            gameOver();
    }

    private void gameOver()
    {
        gameState = GameState.GameOver;
        restartButton(1f, true);
    }

    private void restartButton(float alpha, bool raycast)
    {
        var restart = GameObject.Find("RestartButton");
        var images = GameObject.FindObjectsOfType<Image>();
        foreach (var image in images)
        {
            if (image.name == "RestartButton")
            {
                image.raycastTarget = raycast;
                var temp = image.color;
                temp.a = alpha;
                image.color = temp;
                break;
            }
        }
    }

    public bool CanMove(int x, int y)
    {
        try
        {
            return map.IsWalkablePosition(new Position(x, y));
        }
        catch
        {
            return false;
        }
    }

    public Vector3 GetTilePosition(int x, int y)
    {
        return map.BasePosition + new Vector3(map.TileLength * x, 0, map.TileLength * y);
    }

    public bool IsExitTile(PlayerActor character)
    {
        Tile tile = map.GetTile(character.Position.X, character.Position.Y);
        Debug.Log($"Pos: {character.Position.X},{character.Position.Y}  Char:{character is Breaker}, tile: {tile.GetType()}");
        return tile.IsExitTile(character);
    }

    public bool IsItVictory()
    {
        return gameState.Equals(GameState.Victory);
    }

    public bool IsItGameOver()
    {
        return gameState.Equals(GameState.GameOver);
    }

    public void CheckVictoryCondition()
    {
        if (IsExitTile(map.Breaker) && IsExitTile(map.Fixer))
        {
            gameState = GameState.Victory;
            gameStarted = false;
        }
    }
}

}
