using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BothExitTile : Tile
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
        return true;
    }

}
