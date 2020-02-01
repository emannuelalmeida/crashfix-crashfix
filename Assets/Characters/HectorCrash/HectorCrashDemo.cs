using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HectorCrashDemo : MonoBehaviour
{
    
    private Animator anim;

    void Awake()
    {
        this.anim = GetComponent<Animator>();
    }
    
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetTrigger("idle");
            return;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            anim.SetTrigger("walk");
            return;
        }
        if(Input.GetKeyDown(KeyCode.D)) {
            anim.SetTrigger("action");
            return;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            anim.SetTrigger("victory");
            return;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            anim.SetTrigger("defeat");
            return;
        }
    }
}
