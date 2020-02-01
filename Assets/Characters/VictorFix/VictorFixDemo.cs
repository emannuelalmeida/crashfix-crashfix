using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictorFixDemo : MonoBehaviour
{

    private Animator anim;

    void Awake()
    {
        this.anim = GetComponent<Animator>();
    }
    
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            anim.SetTrigger("idle");
            return;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetTrigger("walk");
            return;
        }
        if(Input.GetKeyDown(KeyCode.E)) {
            anim.SetTrigger("action");
            return;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            anim.SetTrigger("victory");
            return;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            anim.SetTrigger("defeat");
            return;
        }
    }
}
