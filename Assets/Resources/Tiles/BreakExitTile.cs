using Assets.Actors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakExitTile : Tile
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool IsExitTile(PlayerActor playerActor)
    {
        if (playerActor is Breaker)
            return true;

        return false;
    }
}
