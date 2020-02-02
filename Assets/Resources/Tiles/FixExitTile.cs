using Assets.Actors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitFixTile : Tile
{
    // Start is called before the first frame update
    void Start()
    {
        IsWalkable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool IsExitTile(PlayerActor playerActor)
    {
        if (playerActor is Fixer)
            return true;

        return false;
    }

}
