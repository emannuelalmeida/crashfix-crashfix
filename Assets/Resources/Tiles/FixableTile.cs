using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixableTile : Tile
{
    // Start is called before the first frame update
    void Start()
    {
        IsWalkable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Fix()
    {
        IsWalkable = true;
        //TODO: Adicionar troca de estado visual
    }
}
