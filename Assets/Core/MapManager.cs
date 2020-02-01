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

    public Position FixPosition { get; set; }

    public Position BreakPosition { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        map = ScriptableObject.CreateInstance<GridMap>();
        LoadMap(1);
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

    private void Move(Position delta)
    {
        MoveActor(delta, true); // First, move the breaker
        MoveActor(delta, false); // Now, moving the fixer
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

    private void FixedUpdate()
    {
        if (gameStarted && timeRemaining.TotalSeconds > 0)
        {
            timeRemaining = timeRemaining.Subtract(TimeSpan.FromSeconds(Time.fixedDeltaTime));
            Debug.Log(timeRemaining.TotalSeconds.ToString("0"));
        }
    }
}
