using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    public bool IsWalkable { get; set; }
    public bool IsBroken { get; set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void Fix()
    {
        Debug.Log("Tile fixed!");
    }

    public virtual void Break()
    {
        Debug.Log("Tile broken!");
    }

    public virtual bool IsExitTile(PlayerActor playerActor)
    {
        return false;
    }
}
