using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Vitoria : MonoBehaviour
{

    public void GoToMainScreen()
    {
        SceneManager.LoadScene("MainScreen");
    }

    public void OnMouseEnter()
    {
        GetComponent<SpriteRenderer>().color = Color.yellow;
    }
    
    public void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = Color.yellow;
    }
}
