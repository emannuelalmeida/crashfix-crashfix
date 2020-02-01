using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    public bool IsWalkable { get; set; }

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

    }

    public virtual void Break()
    {

    }

    public virtual void OnStepWrench()
    {

    }

    public virtual void OnStepHammer()
    {

    }
}

