using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableTile : Tile
{
    private bool Broken;

    void Start()
    {
        IsWalkable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Break()
    {
        Broken = true;
        IsWalkable = true;
        //TODO: Switch state and animation
    }
}
