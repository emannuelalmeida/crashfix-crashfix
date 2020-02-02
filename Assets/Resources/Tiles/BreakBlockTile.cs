using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BreakBlockTile : Tile
{
    // Start is called before the first frame update
    void Start()
    {
        IsBroken = false;
        IsWalkable = false;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Break()
    {
        if (!IsBroken)
        {
            IsBroken = true;
            IsWalkable = true;

            GetComponentsInChildren<MonoBehaviour>(true).Single(x => x.name == "vase_mesh").GetComponent<MeshRenderer>().enabled = false;
            GetComponentsInChildren<MonoBehaviour>(true).Single(x => x.name == "vase_broken_mesh").GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
