using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BothActTile : Tile
{
    public bool StartsBroken = false;

    // Start is called before the first frame update
    void Start()
    {
        IsBroken = false;
        IsWalkable = false;

        if (StartsBroken)
        {
            Break();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Fix()
    {
        if (IsBroken)
        {
            IsBroken = false;
            IsWalkable = false;

            GetComponentsInChildren<MonoBehaviour>(true).Single(x => x.name == "crate").GetComponent<MeshRenderer>().enabled = true;
            GetComponentsInChildren<MonoBehaviour>(true).Single(x => x.name == "crate_broken").GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public override void Break()
    {
        if (!IsBroken)
        {
            IsBroken = true;
            IsWalkable = true;

            GetComponentsInChildren<MonoBehaviour>(true).Single(x => x.name == "crate").GetComponent<MeshRenderer>().enabled = false;
            GetComponentsInChildren<MonoBehaviour>(true).Single(x => x.name == "crate_broken").GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
